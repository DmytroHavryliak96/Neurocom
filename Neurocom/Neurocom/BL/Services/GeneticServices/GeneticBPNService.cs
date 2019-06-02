using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Ninject;
using Neurocom.DAO.Repositories;
using Neurocom.BL.Services.ControllerServices;
using Neurocom.BL.NetworkLibrary;

namespace Neurocom.BL.Services.GeneticServices
{
    public class GeneticBPNService : BackPropagationServices, INetworkService
    {
        private IGeneticService gaService;
        private IUnitOfWork Database;
        private IAnswerService answer;

        private Func<string, IUnitOfWork, IAnswerService> func;

        private int outputCount;

        public GeneticBPNService() : base()
        {
            func = (tablename, db) =>
            {
                switch (tablename)
                {
                    case "Kerogen":
                        return new KerogenAnswerService(db);
                    case "Layer":
                        return new LayerAnswerService(db);
                    default:
                        throw new ArgumentException("cannot find specified task");
                }
            };
        }

        public GeneticBPNService(string xml) : base(xml)
        {

        }

        public void CreateNetwork()
        {
            bpn = new BackPropagationNetwork(layerSizes, TFuncs);
            bpn.SetBiasToZero();
        }

        public void InitializeService(NetworkInitializer initializer)
        {
            gaService = new GeneticService();
            Database = new EFUnitOfWork();
            answer = func(initializer.taskName, Database);

            outputCount = answer.GetNumOfClusters();

            BPNInitializer input = (BPNInitializer)initializer;
            this.parameters = input.parameters;
            this.hidden = input.hidden;
            this.learningRate = input.learningRate;
            this.Momentum = input.Momentum;
            this.minError = input.minError;
            layerSizes = new int[3] { this.parameters, this.hidden, outputCount };
            TFuncs = new TransferFunction[3] {TransferFunction.None,
                                                               TransferFunction.Sigmoid,
                                                               TransferFunction.Sigmoid};

            double[][] inputs = answer.GetInputs();
            double[][] outputs = answer.GetAnswers();

            gaService.InitializeGA(initializer, 
                (weights)=> {

                    double fitness = 0;

                    bpn.SetWeights(weights);

                    int truePositive = 0;
                    int others = 0;

                    for (int i = 0; i < inputs.GetUpperBound(0)+1; i++)
                    {
                        int cluster;
                        cluster = bpn.getClusterGenetic(inputs[i]);

                        if (outputs[i][0] == cluster)
                            truePositive += 1;
                        else
                            others += 1;
                    }

                    fitness = (truePositive) / (double)(truePositive + others);

                    return fitness;
                } );
        }

        public void Train(double[][] inputs, double[][] answers)
        {
            gaService.TrainGA();

            double[] weights;
            double fitness;

            gaService.GetBestWeights(out weights, out fitness);

         bpn.SetWeights(weights);
        }

        public int TestNetwork(double[] test)
        {
            int result;
            result = bpn.getClusterGenetic(test);

            // це для тестування
           /* Database = new EFUnitOfWork();
            answer = new KerogenAnswerService(Database);
            double[][] inputs = answer.GetInputs();
            double[] answers = new double[19];

            for (int i = 0; i < inputs.GetUpperBound(0) + 1; i++)
            {

                answers[i] = bpn.getClusterGenetic(inputs[i]);
            }*/

            return result;
        }

        public void Dispose()
        {
            Database.Dispose();
            answer.Dispose();
        }

        public string SaveNetworkXml()
        {
            return this.bpn.Save();
        }
    }
}