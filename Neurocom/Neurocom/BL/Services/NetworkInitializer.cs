using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.BL.Services
{
    public abstract class NetworkInitializer
    {
        [HiddenInput(DisplayValue = false)]
        public string taskName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string networkName { get; set; }

        [Range(0.001, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double minError { get; set; }

        [Range(0.1, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double learningRate { get; set; }

        public NetworkInitializer(double minError, double learningRate)
        {
            this.minError = minError;
            this.learningRate = learningRate;
        }

        public NetworkInitializer()
        {

        }
    }


    public class BPNInitializer : NetworkInitializer
    {
        [Range(0.1, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Momentum { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int parameters { get; set; }

        [Range(5, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int hidden { get; set; }

        public BPNInitializer(double minError, double learningRate, double Momentum, int parameters, int hidden) : base(minError, learningRate)
        {
            this.Momentum = Momentum;
            this.parameters = parameters;
            this.hidden = hidden;
        }

        public BPNInitializer()
        {

        }


    }

    public class LVQInitializer : NetworkInitializer
    {
        [Range(0.5, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double decayRate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int numOfClusters { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double[][] patterns { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double[][] answers { get; set; }

        public LVQInitializer(double minError, double learningRate, double decayRate, int numOfClusters, double[][] patternsInput, double[][] answersInput) : base(minError, learningRate)
        {
            this.decayRate = decayRate;
            this.numOfClusters = numOfClusters;

            int TRAINING_PATTERNS = patternsInput.GetUpperBound(0) + 1;
            int VEC_LEN = patternsInput[0].Length;

            patterns = new double[TRAINING_PATTERNS][];
            for (int i = 0; i < TRAINING_PATTERNS; i++)
                patterns[i] = new double[VEC_LEN];

            for (int i = 0; i < TRAINING_PATTERNS; i++)
                for (int j = 0; j < VEC_LEN; j++)
                    patterns[i][j] = patternsInput[i][j];

            int rows = answersInput.GetUpperBound(0) + 1;
            int columns = answersInput[0].Length;

            answers = new double[rows][];
            for (int i = 0; i < rows; i++)
                answers[i] = new double[columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    answers[i][j] = answersInput[i][j];


        }

        public LVQInitializer()
        {

        }


    }
}