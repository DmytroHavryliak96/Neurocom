using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;

namespace Neurocom.BL.NetworkLibrary
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
    #region Функції активації та їхні похідні
    // enum для перелічення можливих функцій активації
    public enum TransferFunction
    {
        None,
        Sigmoid,
        BipolarSigmoid,
        Linear
    }

    public static class TransferFunctions
    {
        // Функція активації
        public static double Evaluate(TransferFunction tFunc, double input)
        {
            switch (tFunc)
            {
                case TransferFunction.Sigmoid:
                    return sigmoid(input);
                case TransferFunction.BipolarSigmoid:
                    return bipolarsigmoid(input);
                case TransferFunction.Linear:
                    return linear(input);
                case TransferFunction.None:
                default:
                    return 0.0;
            }
        }

        // Похідна функції активації
        public static double DerivativeEvaluate(TransferFunction tFunc, double input)
        {
            switch (tFunc)
            {
                case TransferFunction.Linear:
                    return linear_derivative(input);
                case TransferFunction.Sigmoid:
                    return sigmoid_derivative(input);
                case TransferFunction.BipolarSigmoid:
                    return bipolarsigmoid_derivative(input);
                case TransferFunction.None:
                default:
                    return 0.0;

            }

        }

        // лінійна функція
        private static double linear(double x)
        {
            return x;
        }

        // похідна лінійної функції
        private static double linear_derivative(double x)
        {
            return 1.0;
        }

        // Сигмоїда
        private static double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        // Похідна сигмоїди
        private static double sigmoid_derivative(double x)
        {
            return sigmoid(x) * (1 - sigmoid(x));
        }

        private static double bipolarsigmoid(double x)
        {
            return 2.0 / (1.0 + Math.Exp(-x)) - 1;
        }

        private static double bipolarsigmoid_derivative(double x)
        {
            return 0.5 * (1 + bipolarsigmoid(x)) * (1 - bipolarsigmoid(x));
        }

    }
    #endregion

    public class BackPropagationNetwork
    {
        #region Поля
        public int layerCount; // прихований шар + вихідний шар
        public int inputSize; // к-сть нейронів у 0 шарі
        public int[] layerSize; // величини к-сті нейронів у прихованому та вихідному шарах 
        private TransferFunction[] transferFunction; // масив функцій активації

        public double[][] layerOtput; // вихідні дані шару
        public double[][] layerInput; // вхідні дані шару
        private double[][] bias; // відхилення
        private double[][] delta; // дельта помилки
        private double[][] previosBiasDelta; // дельта попереднього відхилення

        private double[][][] weight; // ваги, де [0] - шар, [1] - попередній нейрон, [2] - поточний нейрон
        private double[][][] previousWeightDelta; // дельта попередньої ваги
        private XmlDocument doc = null; // документ типу xml
        public bool loaded = true;


        #endregion

        #region Конструктор
        public BackPropagationNetwork(int[] layerSizes, TransferFunction[] TransferFunctions)
        {
            // Перевірка вхідних даних
            if (TransferFunctions.Length != layerSizes.Length || TransferFunctions[0] != TransferFunction.None)
                throw new ArgumentException("The network cannot be created with these parameters");

            // Ініціалізація шарів мережі
            layerCount = layerSizes.Length - 1;
            inputSize = layerSizes[0];
            layerSize = new int[layerCount];
            transferFunction = new TransferFunction[layerCount];

            for (int i = 0; i < layerCount; i++)
                layerSize[i] = layerSizes[i + 1];

            for (int i = 0; i < layerCount; i++)
                transferFunction[i] = TransferFunctions[i + 1];

            // Визначення вимірів масивів
            bias = new double[layerCount][];
            previosBiasDelta = new double[layerCount][];
            delta = new double[layerCount][];
            layerOtput = new double[layerCount][];
            layerInput = new double[layerCount][];

            weight = new double[layerCount][][];
            previousWeightDelta = new double[layerCount][][];

            // Заповнення двовимірних масивів
            for (int l = 0; l < layerCount; l++)
            {
                bias[l] = new double[layerSize[l]];
                previosBiasDelta[l] = new double[layerSize[l]];
                delta[l] = new double[layerSize[l]];
                layerOtput[l] = new double[layerSize[l]];
                layerInput[l] = new double[layerSize[l]];

                weight[l] = new double[l == 0 ? inputSize : layerSize[l - 1]][];
                previousWeightDelta[l] = new double[l == 0 ? inputSize : layerSize[l - 1]][];

                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                {
                    weight[l][i] = new double[layerSize[l]];
                    previousWeightDelta[l][i] = new double[layerSize[l]];
                }
            }

            // Ініціалізація ваг
            for (int l = 0; l < layerCount; l++)
            {
                for (int i = 0; i < layerSize[l]; i++)
                {
                    bias[l][i] = Gaussian.GetRandomGaussian();
                    previosBiasDelta[l][i] = 0.0;
                    layerInput[l][i] = 0.0;
                    layerOtput[l][i] = 0.0;
                    delta[l][i] = 0.0;
                }

                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                {
                    for (int j = 0; j < layerSize[l]; j++)
                    {
                        weight[l][i][j] = Gaussian.GetRandomGaussian();
                        previousWeightDelta[l][i][j] = 0.0;
                    }
                }
            }
        }

        public BackPropagationNetwork(string xml)
        {
            //loaded = false;

            Load(xml);

            //loaded = true;
        }

        public BackPropagationNetwork()
        {
        }

        #endregion

        #region Methods
        public void Run(ref double[] input, out double[] output)
        {
            // Перевірка, чи введені дані відповідають кількості нейронів у вхідному шарі
            if (input.Length != inputSize)
                throw new ArgumentException("Input data isn't of the correct dimension");

            // Вихідне значення функції
            output = new double[layerSize[layerCount - 1]];

            // Нормалізація вхідних значень
            double max = input.Max();

            // Запуск мережі
            for (int l = 0; l < layerCount; l++)
            {
                for (int j = 0; j < layerSize[l]; j++)
                {
                    double sum = 0.0;

                    for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                        sum += weight[l][i][j] * (l == 0 ? input[i] : layerOtput[l - 1][i]);

                    sum += bias[l][j];
                    layerInput[l][j] = sum;

                    /*if (l == layerCount - 1)
                        layerOtput[l][j] = sum;
                    else*/
                    layerOtput[l][j] = TransferFunctions.Evaluate(transferFunction[l], sum);

                }
            }

            // копіюємо вихід мережі у вихідний масив
            for (int i = 0; i < layerSize[layerCount - 1]; i++)
            {
                output[i] = layerOtput[layerCount - 1][i];
            }

        }

        // Функція навчання
        public double Train(ref double[] input, ref double[] desired, double TrainingRate, double Momentum)
        {
            // Перевірка вхідних параметрів
            if (input.Length != inputSize)
                throw new ArgumentException("Invalid input parameter", "input");

            if (desired.Length != layerSize[layerCount - 1])
                throw new ArgumentException("Invalid input parameter", "desired");

            // Локальні змінні
            double error = 0.0, sum = 0.0, weigtdelta = 0.0, biasDelta = 0.0;
            double[] output = new double[layerSize[layerCount - 1]];

            // Запуск мережі
            Run(ref input, out output);

            //Розмножуємо похибку у зворотньму порядку
            for (int l = layerCount - 1; l >= 0; l--)
            {
                //Вихідний шар
                if (l == layerCount - 1)
                {
                    for (int k = 0; k < layerSize[l]; k++)
                    {
                        delta[l][k] = output[k] - desired[k];
                        error += Math.Pow(delta[l][k], 2);
                        delta[l][k] *= TransferFunctions.DerivativeEvaluate(transferFunction[l], layerInput[l][k]);
                    }

                }
                //Прихований шар
                else
                {
                    for (int i = 0; i < layerSize[l]; i++)
                    {
                        sum = 0.0;
                        for (int j = 0; j < layerSize[l + 1]; j++)
                        {
                            sum += weight[l + 1][i][j] * delta[l + 1][j];
                        }
                        sum *= TransferFunctions.DerivativeEvaluate(transferFunction[l], layerInput[l][i]);
                        delta[l][i] = sum;
                    }
                }
            }

            // Оновлення ваг та відхилень
            for (int l = 0; l < layerCount; l++)
                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                    for (int j = 0; j < layerSize[l]; j++)
                    {
                        weigtdelta = TrainingRate * delta[l][j] * (l == 0 ? input[i] : layerOtput[l - 1][i]) + Momentum * previousWeightDelta[l][i][j];
                        weight[l][i][j] -= weigtdelta;

                        previousWeightDelta[l][i][j] = weigtdelta;
                    }

            for (int l = 0; l < layerCount; l++)
                for (int i = 0; i < layerSize[l]; i++)
                {
                    biasDelta = TrainingRate * delta[l][i] + Momentum * previosBiasDelta[l][i];
                    bias[l][i] -= biasDelta;

                    previosBiasDelta[l][i] = biasDelta;
                }

            return error;
        }

        // Функція для навчання тестової вибірки
        public void TrainNetwork(double[][] patterns, double[][] answers, double min_error, double TrainingRate, double Momentum)
        {
            int amountOfPatterns = patterns.GetUpperBound(0) + 1;
            Random rnd = new Random();
            double error;
            do
            {
                List<BackPropagationContainer> TrainingSet = new List<BackPropagationContainer>();

                for (int k = 0; k < amountOfPatterns; k++)
                {
                    BackPropagationContainer obj = new BackPropagationContainer();
                    obj.trainVector = patterns[k];
                    obj.answer = answers[k];
                    TrainingSet.Add(obj);
                }

                error = 0.0;

                for (int k = 0; k < amountOfPatterns; k++)
                {
                    int index = rnd.Next(amountOfPatterns - k);
                    error += Train(ref TrainingSet[index].trainVector, ref TrainingSet[index].answer, TrainingRate, Momentum);
                    TrainingSet.RemoveAt(index);
                }
                error /= amountOfPatterns;
            } while (error > min_error);

        }

        /* public int getAnswer(double[] testVector, double[] output, string taskName)
         {
             switch (taskName)
             {
                 case "Kerogen":
                     {
                         return getCluster(testVector);
                     }
                 case "Layer":
                     {
                         return getClusterPlast(testVector);
                     }
                 default:
                     {
                         return 0;
                         break;
                     }
             }
         }*/

        public int getCluster(double[] input, double[] output)
        {
            Run(ref input, out output);
            if ((output[0] >= -1.0) && (output[0] < -0.3))
                return 1;
            else
            {
                if ((output[0] >= -0.3) && (output[0] < 0.3))
                    return 2;
                else
                    return 3;
            }
        }

        public int getClusterPlast(double[] input, double[] output)
        {
            Run(ref input, out output);
            if ((output[0] >= 0.0) && (output[0] < 0.5))
                return 1;
            else
                return 2;
        }

        // Зберігання ваг
        public string Save()
        {
            using (TextWriter textWriter = new Utf8StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter))
                {
                    // Починаємо документ
                    writer.WriteStartElement("NeuralNetwork");
                    writer.WriteAttributeString("Type", "BackPropagation");

                    // Параметри мережі
                    writer.WriteStartElement("Parameters");
                    writer.WriteElementString("Name", Name);
                    writer.WriteElementString("inputSize", inputSize.ToString());
                    writer.WriteElementString("layerCount", layerCount.ToString());

                    // розміри шарів
                    writer.WriteStartElement("Layers");
                    for (int l = 0; l < layerCount; l++)
                    {
                        writer.WriteStartElement("Layer");
                        writer.WriteAttributeString("Index", l.ToString());
                        writer.WriteAttributeString("Size", layerSize[l].ToString());
                        writer.WriteAttributeString("Type", transferFunction[l].ToString());
                        writer.WriteEndElement(); // end Layer tag
                    }
                    writer.WriteEndElement(); // end Layers tag
                    writer.WriteEndElement(); // end Parameters tag


                    // Ваги та відилення
                    writer.WriteStartElement("Weights");

                    for (int l = 0; l < layerCount; l++)
                    {
                        writer.WriteStartElement("Layer");
                        writer.WriteAttributeString("Index", l.ToString());

                        for (int j = 0; j < layerSize[l]; j++)
                        {
                            writer.WriteStartElement("Node");
                            writer.WriteAttributeString("Index", j.ToString());
                            writer.WriteAttributeString("Bias", bias[l][j].ToString());

                            for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                            {
                                writer.WriteStartElement("Axon");
                                writer.WriteAttributeString("Index", i.ToString());

                                writer.WriteString(weight[l][i][j].ToString());

                                writer.WriteEndElement(); // end of Axon tag
                            }

                            writer.WriteEndElement(); // end of Node tag
                        }
                        writer.WriteEndElement(); // end of Layer tag
                    }

                    writer.WriteEndElement(); // end of Weights tag

                    writer.WriteEndElement(); // end Neural network tag 
                }
                return textWriter.ToString();
            }

        }



        // Зчитування ваг
        public void Load(string xml)
        {

            doc = new XmlDocument();
            doc.LoadXml(xml);

            string BasePath = "", NodePath = "";
            double value;

            // Завантажуємо дані з xml
            if (xPathValue("NeuralNetwork/@Type") != "BackPropagation")
            {
                loaded = false;
                return;
            }

            BasePath = "NeuralNetwork/Parameters/";
            Name = xPathValue(BasePath + "Name");

            int.TryParse(xPathValue(BasePath + "inputSize"), out inputSize);
            int.TryParse(xPathValue(BasePath + "layerCount"), out layerCount);

            layerSize = new int[layerCount];
            transferFunction = new TransferFunction[layerCount];

            BasePath = "NeuralNetwork/Parameters/Layers/Layer";
            for (int l = 0; l < layerCount; l++)
            {
                int.TryParse(xPathValue(BasePath + "[@Index='" + l.ToString() + "']/@Size"), out layerSize[l]);
                Enum.TryParse<TransferFunction>(xPathValue(BasePath + "[@Index='" + l.ToString() + "']/@Type"), out transferFunction[l]);
            }

            // Визначення вимірів масивів
            bias = new double[layerCount][];
            previosBiasDelta = new double[layerCount][];
            delta = new double[layerCount][];
            layerOtput = new double[layerCount][];
            layerInput = new double[layerCount][];

            weight = new double[layerCount][][];
            previousWeightDelta = new double[layerCount][][];

            // Заповнення двовимірних масивів
            for (int l = 0; l < layerCount; l++)
            {
                bias[l] = new double[layerSize[l]];
                previosBiasDelta[l] = new double[layerSize[l]];
                delta[l] = new double[layerSize[l]];
                layerOtput[l] = new double[layerSize[l]];
                layerInput[l] = new double[layerSize[l]];

                weight[l] = new double[l == 0 ? inputSize : layerSize[l - 1]][];
                previousWeightDelta[l] = new double[l == 0 ? inputSize : layerSize[l - 1]][];

                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                {
                    weight[l][i] = new double[layerSize[l]];
                    previousWeightDelta[l][i] = new double[layerSize[l]];
                }
            }

            // Ініціалізація ваг
            for (int l = 0; l < layerCount; l++)
            {
                BasePath = "NeuralNetwork/Weights/Layer[@Index='" + l.ToString() + "']/";
                for (int i = 0; i < layerSize[l]; i++)
                {
                    NodePath = "Node[@Index='" + i.ToString() + "']/@Bias";
                    double.TryParse(xPathValue(BasePath + NodePath), out value);

                    bias[l][i] = value;
                    previosBiasDelta[l][i] = 0.0;
                    layerInput[l][i] = 0.0;
                    layerOtput[l][i] = 0.0;
                    delta[l][i] = 0.0;
                }

                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                {
                    for (int j = 0; j < layerSize[l]; j++)
                    {
                        NodePath = "Node[@Index='" + j.ToString() + "']/Axon[@Index='" + i.ToString() + "']";
                        double.TryParse(xPathValue(BasePath + NodePath), out value);
                        weight[l][i][j] = value;
                        previousWeightDelta[l][i][j] = 0.0;
                    }
                }
            }
            doc = null; // звільнення пам'яті

        }

        // Функція повернення ваг
        public double[][] GetWeights(int layer)
        {
            double[][] array = new double[layer == 0 ? inputSize : layerSize[layer - 1]][];

            for (int i = 0; i < (layer == 0 ? inputSize : layerSize[layer - 1]); i++)
            {
                array[i] = new double[layerSize[layer]];
            }

            for (int i = 0; i < (layer == 0 ? inputSize : layerSize[layer - 1]); i++)
            {
                for (int j = 0; j < layerSize[layer]; j++)
                    array[i][j] = weight[layer][i][j];
            }

            return array;
        }

        // Функція встановлення ваг
        public void SetWeights(double[] weights)
        {
            int counter = 0;
            for (int l = 0; l < layerCount; l++)
            {
                for (int i = 0; i < (l == 0 ? inputSize : layerSize[l - 1]); i++)
                {
                    for (int j = 0; j < layerSize[l]; j++)
                    {
                        weight[l][i][j] = weights[counter++];
                    }
                }
            }
        }

        // Функція для парсингу xml-документа
        private string xPathValue(string xPath)
        {
            XmlNode node = doc.SelectSingleNode(xPath);

            if (node == null)
                throw new ArgumentException("Cannot find specified node", xPath);

            return node.InnerText;
        }
        #endregion

        #region Public data

        public string Name = "Default";

        #endregion
    }

    public class LVQ
    {
        private int NUMBER_OF_CLUSTERS; // кількість кластерів = кількості нейронів у вихідному шарі
        private int VEC_LEN; // довжина навчального вектора (кількість параметрів)
        private int TRAINING_PATTERNS; // кількість навчальних векторів

        private double MIN_ERROR; // максимальна похибка
        private double DECAY_RATE; // швидкість зменшення швидкості навчання

        private double alpha; // швидкість навчання
        private double[] distances; // масив відстаней від навч. вектора до кожного з нейронів (постійно обнуляється)

        private double[][] weights;// масив ваг мережі

        private double[][] mPattern; // масив навчальної вибірки
        private int[] mTarget; // масив правильних відповідей (так звані номера кластерів)

        public bool loaded = true;
        private XmlDocument doc = null;

        public string Name = "Default";
        public Random rnd = new Random();

        // Конструктор
        public LVQ(double[][] patterns, int[] clusters, double min_error, double alpha, double decay_rate, int num_of_clusters)
        {

            VEC_LEN = patterns[0].Length;
            TRAINING_PATTERNS = patterns.GetUpperBound(0) + 1;
            NUMBER_OF_CLUSTERS = num_of_clusters;

            InitializePatterns(patterns);
            InitializeClusters(clusters);

            this.alpha = alpha;
            MIN_ERROR = min_error;
            DECAY_RATE = decay_rate;

            distances = new double[num_of_clusters * 3];
            InitializeWeights();
        }

        public LVQ(string xml)
        {
            //loaded = false;

            Load(xml);

            //loaded = true;
        }

        public LVQ()
        {

        }

        // ініціалізація навчальної вибірки
        private void InitializePatterns(double[][] patterns)
        {

            mPattern = new double[TRAINING_PATTERNS][];

            for (int i = 0; i < TRAINING_PATTERNS; i++)
                mPattern[i] = new double[VEC_LEN];

            for (int i = 0; i < TRAINING_PATTERNS; i++)
                for (int j = 0; j < VEC_LEN; j++)
                    mPattern[i][j] = patterns[i][j];
        }

        // ініціалізація правильних відповідей (кластерів)
        private void InitializeClusters(int[] clusters)
        {
            mTarget = new int[TRAINING_PATTERNS];

            for (int i = 0; i < TRAINING_PATTERNS; i++)
            {
                mTarget[i] = clusters[i];
            }
        }

        // Ініціалізація ваг відповідно до навчальної вибірки
        private void InitializeWeights()
        {
            weights = new double[NUMBER_OF_CLUSTERS * 3][];
            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                weights[i] = new double[VEC_LEN];

            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                for (int j = 0; j < VEC_LEN; j++)
                    weights[i][j] = mPattern[i][j];

        }

        // Навчання мережі
        public void TrainNetwork()
        {
            Random rnd = new Random();
            double error;
            int amount = 0;
            do
            {
                error = 0.0;

                // Створення навчальної вибірки
                List<LVQContainer> TrainingSet = new List<LVQContainer>();
                for (int i = 0; i < TRAINING_PATTERNS; i++)
                {
                    LVQContainer obj = new LVQContainer();
                    obj.trainVector = mPattern[i];
                    obj.cluster = i;
                    TrainingSet.Add(obj);
                }

                for (int i = 0; i < TRAINING_PATTERNS; i++)
                {
                    int index = rnd.Next(TRAINING_PATTERNS - i);
                    computeInput(TrainingSet[index].trainVector);
                    int winner = FindWinner(distances);
                    error += UpdateWeights(TrainingSet[index].cluster, winner);
                    TrainingSet.RemoveAt(index);
                }
                amount++;
                // Зменшуємо швидкість навчання
                alpha = DECAY_RATE * alpha;
            } while (error > MIN_ERROR);
            //Console.WriteLine("amount of cycles {0}", amount);
        }

        // Оновлюємо ваги
        public double UpdateWeights(int TrainVectorNumber, int winner)
        {
            double[] previousWeight2 = new double[VEC_LEN];
            for (int i = 0; i < VEC_LEN; i++)
            {
                previousWeight2[i] = weights[winner][i];
            }

            int cluster; // кластер нейрона - переможця
            // перевірка кластера
            if (winner >= 0 && winner < 3)
                cluster = 0;
            else
            {
                if (winner >= 3 && winner < 6)
                    cluster = 1;
                else
                    cluster = 2;
            }

            for (int i = 0; i < VEC_LEN; i++)
            {
                // якщо нейрон-переможець із того кластеру, що й правильна відповідь
                if (cluster == mTarget[TrainVectorNumber])
                    weights[winner][i] += (alpha * (mPattern[TrainVectorNumber][i] - weights[winner][i]));

                // якщо нейрон-переможець з іншого кластера, ніж правильна відповідь
                else
                    weights[winner][i] -= (alpha * (mPattern[TrainVectorNumber][i] - weights[winner][i]));

            }

            double distance = 0; // різниця ваг до оновлення та після оновлення (так звана похибка для нейрона) 

            // знаходимо величину зміни ваг
            for (int i = 0; i < VEC_LEN; i++)
            {
                distance += Math.Pow(previousWeight2[i] - weights[winner][i], 2);
            }

            distance = Math.Sqrt(distance);

            return distance;
        }

        public int getAnswer(double[] testVector, string taskName)
        {
            switch (taskName)
            {
                case "Kerogen":
                    {
                        return getCluster(testVector);
                    }
                case "Layer":
                    {
                        return getClusterPlast(testVector);
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        // метод для тестування  
        public int getCluster(double[] TestVector)
        {
            computeInput(TestVector);
            int winner = FindWinner(distances);
            int cluster;
            if (winner >= 0 && winner < 3)
                cluster = 0;
            else
            {
                if (winner >= 3 && winner < 6)
                    cluster = 1;
                else
                    cluster = 2;
            }

            return cluster + 1;
        }

        public int getClusterPlast(double[] TestVector)
        {
            computeInput(TestVector);
            int winner = FindWinner(distances);
            int cluster;
            if (winner >= 0 && winner < 3)
                cluster = 0;
            else
                cluster = 1;

            return cluster + 1;
        }

        // розраховуємо відстані від кожного нейрона вихідного шару до навчального вектора
        public void computeInput(double[] trainVector)
        {
            clearArray(distances);
            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                for (int j = 0; j < VEC_LEN; j++)
                    distances[i] += Math.Pow(weights[i][j] - trainVector[j], 2);
        }

        // знаходимо нейрон-переможець
        private int FindWinner(double[] distances)
        {
            double min = distances.Min();
            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                if (min == distances[i])
                    return i;
            return 0;
        }

        // обнульовуємо відстані
        private void clearArray(double[] distances)
        {
            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                distances[i] = 0.0;
        }

        // функція для збереження ваг
        public string Save()
        {

            using (TextWriter textWriter = new Utf8StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(textWriter))
                {

                    // починаємо документ
                    writer.WriteStartElement("NeuralNetwork");
                    writer.WriteAttributeString("Type", "Learning Vector Quantization");

                    // Параметрі мережі
                    writer.WriteStartElement("Parameters");
                    writer.WriteElementString("Name", "Name");
                    writer.WriteElementString("VEC_LEN", VEC_LEN.ToString());
                    writer.WriteElementString("NUMBER_OF_CLUSTERS", NUMBER_OF_CLUSTERS.ToString());

                    writer.WriteEndElement(); // end of Parameters tag 

                    // Ваги та відхилення
                    writer.WriteStartElement("Weights");

                    for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                    {
                        writer.WriteStartElement("Neuron");
                        writer.WriteAttributeString("Index", i.ToString());
                        for (int k = 0; k < VEC_LEN; k++)
                        {

                            writer.WriteStartElement("Axon");
                            writer.WriteAttributeString("Index", k.ToString());

                            writer.WriteString(weights[i][k].ToString());

                            writer.WriteEndElement(); // end of Axon tag
                        }


                        writer.WriteEndElement(); // end of Neuron tag
                    }

                    writer.WriteEndElement(); // end of Weights tag

                    writer.WriteEndElement(); // end of NeuralNetwork tag
                }
                return textWriter.ToString();
            }
        }

        // функція для завантаження ваг
        public void Load(string xml)
        {

            doc = new XmlDocument();
            doc.LoadXml(xml);

            string BasePath = "", NodePath = "";
            double value;

            // Завантажуємо дані з xml
            if (xPathValue("NeuralNetwork/@Type") != "Learning Vector Quantization")
            {
                loaded = false;
                return;
            }

            BasePath = "NeuralNetwork/Parameters/";
            Name = xPathValue(BasePath + "Name");

            int.TryParse(xPathValue(BasePath + "VEC_LEN"), out VEC_LEN);
            int.TryParse(xPathValue(BasePath + "NUMBER_OF_CLUSTERS"), out NUMBER_OF_CLUSTERS);

            // ініціалізуємо ваги
            weights = new double[NUMBER_OF_CLUSTERS * 3][];
            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
                weights[i] = new double[VEC_LEN];

            for (int i = 0; i < NUMBER_OF_CLUSTERS * 3; i++)
            {
                BasePath = "NeuralNetwork/Weights/Neuron[@Index='" + i.ToString() + "']/";

                for (int k = 0; k < VEC_LEN; k++)
                {
                    NodePath = "Axon[@Index='" + k.ToString() + "']";
                    double.TryParse(xPathValue(BasePath + NodePath), out value);
                    weights[i][k] = value;
                }

            }

            doc = null;
            distances = new double[NUMBER_OF_CLUSTERS * 3];
        }

        // метод для парсингу xml-документа
        private string xPathValue(string xPath)
        {
            XmlNode node = doc.SelectSingleNode(xPath);

            if (node == null)
                throw new ArgumentException("Cannot find specified node ", xPath);
            return node.InnerText;
        }
    }

    // клас - контейнер для мережі lvq
    public class LVQContainer
    {
        public double[] trainVector;
        public int cluster;
    }

    // клас - контейнер для мережі BackPropagation
    public class BackPropagationContainer
    {
        public double[] trainVector;
        public double[] answer;
    }

    // Клас для нормалізаціїї даних
    public static class Normalize
    {
        public static void NormalizeParameters(double[][] param)
        {
            for (int i = 0; i < param.GetUpperBound(0) + 1; i++)
            {
                for (int k = 0; k < param[i].Length; k++)
                {
                    param[i][k] /= 100;
                }
            }
        }

        public static void NormalizeTest(double[][] param)
        {
            int length = param[0].Length;
            for (int i = 0; i < param.GetUpperBound(0) + 1; i++)
            {
                double summary = param[i].Sum();

                if (summary < 1.0)
                {
                    double difference = 1.0 - summary;
                    param[i][length - 1] += difference;
                }
                else
                {
                    if (summary > 1.0)
                    {
                        double difference = summary - 1.0;
                        for (int k = length - 1; k >= 0; k--)
                        {
                            if (param[i][k] > difference)
                            {
                                param[i][k] = param[i][k] - difference;
                                break;
                            }
                            else
                            {
                                difference = difference - param[i][k];
                                param[i][k] = 0.0;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public static double[][] FormAnswersBackPropagation(double[][] answers)
        {
            double[][] returnAnswer = new double[answers.Length][];
            for (int i = 0; i < answers.Length; i++)
                returnAnswer[i] = new double[1];

            for (int i = 0; i < answers.GetUpperBound(0) + 1; i++)
            {
                if (answers[i][0] == 1)
                    returnAnswer[i][0] = -1;
                if (answers[i][0] == 2)
                    returnAnswer[i][0] = 0;
                if (answers[i][0] == 3)
                    returnAnswer[i][0] = 1;
            }
            return returnAnswer;
        }

        public static double[][] FormAnswersBackPropagationPlast(double[][] answers)
        {
            double[][] returnAnswer = new double[answers.Length][];
            for (int i = 0; i < answers.Length; i++)
                returnAnswer[i] = new double[1];

            for (int i = 0; i < answers.GetUpperBound(0) + 1; i++)
            {
                if (answers[i][0] == 1)
                    returnAnswer[i][0] = 0;
                if (answers[i][0] == 2)
                    returnAnswer[i][0] = 1;
            }
            return returnAnswer;
        }

        public static int[] FormAnswersLVQ(double[][] answers)
        {
            int[] returnAnswers = new int[answers.GetUpperBound(0) + 1];
            for (int i = 0; i < answers.Length; i++)
            {
                returnAnswers[i] = Convert.ToInt32(answers[i][0] - 1);
            }
            return returnAnswers;
        }
    }

    // Клас для створення випадкових чисел з нормальним розподілом
    public static class Gaussian
    {
        private static Random gen = new Random();

        public static double GetRandomGaussian()
        {
            return GetRandomGaussian(0.0, 1.0);
        }

        public static double GetRandomGaussian(double mean, double stddev)
        {
            double rVal1, rVal2;
            GetRandomGaussian(mean, stddev, out rVal1, out rVal2);
            return rVal1;
        }

        public static void GetRandomGaussian(double mean, double stddev, out double val1, out double val2)
        {
            double u, v, s, t;

            do
            {
                u = 2 * gen.NextDouble() - 1;
                v = 2 * gen.NextDouble() - 1;
            } while (u * u + v * v > 1 || (u == 0 && v == 0));

            s = u * u + v * v;
            t = Math.Sqrt((-2.0 * Math.Log(s)) / s);

            val1 = stddev * t * u + mean;
            val2 = stddev * t * v + mean;


        }
    }

    // клас Геном для генетичного алгоритму
    public class Genome
    {
        public Genome()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Genome(int length)
        {
            m_length = length;
            m_genes = new double[length];
            CreateGenes();
        }
        public Genome(int length, bool createGenes)
        {
            m_length = length;
            m_genes = new double[length];
            if (createGenes)
                CreateGenes();
        }

        public Genome(ref double[] genes)
        {
            m_length = genes.Length;
            m_genes = new double[m_length];
            Array.Copy(genes, m_genes, m_length);
        }

        public Genome DeepCopy()
        {
            Genome g = new Genome(m_length, false);
            Array.Copy(m_genes, g.m_genes, m_length);
            return g;
        }

        private void CreateGenes()
        {
            for (int i = 0; i < m_genes.Length; i++)
                m_genes[i] = m_random.NextDouble();
        }

        public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
        {
            int pos = (int)(m_random.NextDouble() * (double)m_length);
            child1 = new Genome(m_length, false);
            child2 = new Genome(m_length, false);
            for (int i = 0; i < m_length; i++)
            {
                if (i < pos)
                {
                    child1.m_genes[i] = m_genes[i];
                    child2.m_genes[i] = genome2.m_genes[i];
                }
                else
                {
                    child1.m_genes[i] = genome2.m_genes[i];
                    child2.m_genes[i] = m_genes[i];
                }
            }
        }

        public void Mutate()
        {
            for (int pos = 0; pos < m_length; pos++)
            {
                if (m_random.NextDouble() < m_mutationRate)
                    m_genes[pos] = (m_genes[pos] + m_random.NextDouble()) / 2.0;
            }
        }

        public double[] Genes()
        {
            return m_genes;
        }

        public void Output()
        {
            foreach (double valeur in m_genes)
            {
                System.Console.WriteLine("{0:F4}", valeur);
            }
            System.Console.Write("------\n");
        }

        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < m_length; i++)
                values[i] = m_genes[i];
        }

        public double[] m_genes;
        private int m_length;
        private double m_fitness;
        static Random m_random = new Random();

        private static double m_mutationRate;

        public double Fitness
        {
            get
            {
                return m_fitness;
            }
            set
            {
                m_fitness = value;
            }
        }

        public static double MutationRate
        {
            get
            {
                return m_mutationRate;
            }
            set
            {
                m_mutationRate = value;
            }
        }

        public int Length
        {
            get
            {
                return m_length;
            }
        }
    }


    public delegate double GAFunction(params double[] values);

    // генетичний алгоритм
    /// <summary>
	/// Genetic Algorithm class
	/// </summary>
	public class GA
    {
        /// <summary>
        /// Default constructor sets mutation rate to 5%, crossover to 80%, population to 100,
        /// and generations to 2000.
        /// </summary>
        public GA()
        {
            InitialValues();
            m_mutationRate = 0.05;
            m_crossoverRate = 0.80;
            m_populationSize = 100;
            m_generationSize = 2000;
            m_strFitness = "";
        }

        public GA(double crossoverRate,
                  double mutationRate,
                  int populationSize,
                  int generationSize,
                  int genomeSize)
        {
            InitialValues();
            m_mutationRate = mutationRate;
            m_crossoverRate = crossoverRate;
            m_populationSize = populationSize;
            m_generationSize = generationSize;
            m_genomeSize = genomeSize;
            m_strFitness = "";
        }

        public GA(int genomeSize)
        {
            InitialValues();
            m_genomeSize = genomeSize;
        }


        public void InitialValues()
        {
            m_elitism = false;
        }


        /// <summary>
        /// Method which starts the GA executing.
        /// </summary>
        public void Go()
        {
            /// -------------
            /// Preconditions
            /// -------------
			if (getFitness == null)
                throw new ArgumentNullException("Need to supply fitness function");
            if (m_genomeSize == 0)
                throw new IndexOutOfRangeException("Genome size not set");
            /// -------------

            //  Create the fitness table.
            m_fitnessTable = new List<double>();
            m_thisGeneration = new List<Genome>(m_generationSize);
            m_nextGeneration = new List<Genome>(m_generationSize);
            Genome.MutationRate = m_mutationRate;


            CreateGenomes();
            RankPopulation();


            for (int i = 0; i < m_generationSize; i++)
            {
                CreateNextGeneration();
                RankPopulation();
            }

        }

        /// <summary>
        /// After ranking all the genomes by fitness, use a 'roulette wheel' selection
        /// method.  This allocates a large probability of selection to those with the 
        /// highest fitness.
        /// </summary>
        /// <returns>Random individual biased towards highest fitness</returns>
        private int RouletteSelection()
        {
            double randomFitness = m_random.NextDouble() * m_totalFitness;
            int idx = -1;
            int mid;
            int first = 0;
            int last = m_populationSize - 1;
            mid = (last - first) / 2;

            //  ArrayList's BinarySearch is for exact values only
            //  so do this by hand.
            while (idx == -1 && first <= last)
            {
                if (randomFitness < m_fitnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > m_fitnessTable[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                //  lies between i and i+1
                if ((last - first) == 1)
                    idx = last;
            }
            return idx;
        }

        /// <summary>
		/// Rank population and sort in order of fitness.
		/// </summary>
		private void RankPopulation()
        {
            m_totalFitness = 0.0;
            foreach (Genome g in m_thisGeneration)
            {
                g.Fitness = FitnessFunction(g.Genes());
                m_totalFitness += g.Fitness;
            }
            m_thisGeneration.Sort(delegate (Genome x, Genome y)
            { return Comparer<double>.Default.Compare(x.Fitness, y.Fitness); });

            //  now sorted in order of fitness.
            double fitness = 0.0;
            m_fitnessTable.Clear();
            foreach (Genome t in m_thisGeneration)
            {
                fitness += t.Fitness;
                m_fitnessTable.Add(t.Fitness);
            }
        }

        /// <summary>
		/// Create the *initial* genomes by repeated calling the supplied fitness function
		/// </summary>
		private void CreateGenomes()
        {
            for (int i = 0; i < m_populationSize; i++)
            {
                Genome g = new Genome(m_genomeSize);
                m_thisGeneration.Add(g);
            }
        }

        private void CreateNextGeneration()
        {
            m_nextGeneration.Clear();
            Genome g = null;
            if (m_elitism)
                g = m_thisGeneration[m_populationSize - 1].DeepCopy();

            for (int i = 0; i < m_populationSize; i += 2)
            {
                int pidx1 = RouletteSelection();
                int pidx2 = RouletteSelection();
                Genome parent1, parent2, child1, child2;
                parent1 = m_thisGeneration[pidx1];
                parent2 = m_thisGeneration[pidx2];

                if (m_random.NextDouble() < m_crossoverRate)
                {
                    parent1.Crossover(ref parent2, out child1, out child2);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }
                child1.Mutate();
                child2.Mutate();

                m_nextGeneration.Add(child1);
                m_nextGeneration.Add(child2);
            }
            if (m_elitism && g != null)
                m_nextGeneration[0] = g;

            m_thisGeneration.Clear();
            foreach (Genome ge in m_nextGeneration)
                m_thisGeneration.Add(ge);
        }


        private double m_mutationRate;
        private double m_crossoverRate;
        private int m_populationSize;
        private int m_generationSize;
        private int m_genomeSize;
        private double m_totalFitness;
        private string m_strFitness;
        private bool m_elitism;

        private List<Genome> m_thisGeneration;
        private List<Genome> m_nextGeneration;
        private List<double> m_fitnessTable;

        static Random m_random = new Random();



        static private GAFunction getFitness;
        public GAFunction FitnessFunction
        {
            get
            {
                return getFitness;
            }
            set
            {
                getFitness = value;
            }
        }


        //  Properties
        public int PopulationSize
        {
            get
            {
                return m_populationSize;
            }
            set
            {
                m_populationSize = value;
            }
        }

        public int Generations
        {
            get
            {
                return m_generationSize;
            }
            set
            {
                m_generationSize = value;
            }
        }

        public int GenomeSize
        {
            get
            {
                return m_genomeSize;
            }
            set
            {
                m_genomeSize = value;
            }
        }

        public double CrossoverRate
        {
            get
            {
                return m_crossoverRate;
            }
            set
            {
                m_crossoverRate = value;
            }
        }
        public double MutationRate
        {
            get
            {
                return m_mutationRate;
            }
            set
            {
                m_mutationRate = value;
            }
        }

        public string FitnessFile
        {
            get
            {
                return m_strFitness;
            }
            set
            {
                m_strFitness = value;
            }
        }

        /// <summary>
        /// Keep previous generation's fittest individual in place of worst in current
        /// </summary>
        public bool Elitism
        {
            get
            {
                return m_elitism;
            }
            set
            {
                m_elitism = value;
            }
        }

        public void GetBest(out double[] values, out double fitness)
        {
            Genome g = m_thisGeneration[m_populationSize - 1];
            values = new double[g.Length];
            g.GetValues(ref values);
            fitness = g.Fitness;
        }

        public void GetWorst(out double[] values, out double fitness)
        {
            GetNthGenome(0, out values, out fitness);
        }

        public void GetNthGenome(int n, out double[] values, out double fitness)
        {
            /// Preconditions
            /// -------------
			if (n < 0 || n > m_populationSize - 1)
                throw new ArgumentOutOfRangeException("n too large, or too small");
            /// -------------
			Genome g = m_thisGeneration[n];
            values = new double[g.Length];
            g.GetValues(ref values);
            fitness = g.Fitness;
        }
    }
}