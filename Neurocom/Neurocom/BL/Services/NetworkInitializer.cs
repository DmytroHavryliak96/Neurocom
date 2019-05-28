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
        public double minError { get; set; } = 0.1;

        [Range(0.1, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double learningRate { get; set; } = 0.1;

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
        public double Momentum { get; set; } = 0.1;

        [HiddenInput(DisplayValue = false)]
        public int parameters { get; set; }

        [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
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

        public BPNInitializer(double Momentum, int parameters, int hidden)
        {
            this.Momentum = Momentum;
            this.parameters = parameters;
            this.hidden = hidden;
            this.learningRate = 0.1;
            this.minError = 0.001;
        }


    }

    public class GeneticInitializer : BPNInitializer
    {
        [HiddenInput(DisplayValue = false)]
        new public double minError { get; set; } = 0.1;

        [HiddenInput(DisplayValue = false)]
        new public double learningRate { get; set; } = 0.1;

        [HiddenInput(DisplayValue = false)]
        new public double Momentum { get; set; } = 0.1;

        [Range(0.1, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Crossover { get; set; }

        [Range(0.01, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double MutationRate { get; set; }

        [Range(1, 200, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int PopulationSize { get; set; }

        [Range(1, 10000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Iterations { get; set; }

        public GeneticInitializer(double Momentum, int parameters, int hidden, double crossover, double mutationRate, int populationSize, int iterations)
            : base(Momentum, parameters, hidden)
        {
            Crossover = crossover;
            MutationRate = mutationRate;
            PopulationSize = populationSize;
            Iterations = iterations;
        }

        public GeneticInitializer()
        {

        }
    }

    public class LVQInitializer : NetworkInitializer
    {
        [Range(0.001, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double minError { get; set; }

        [Range(0.1, 0.9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double learningRate { get; set; }

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
            this.minError = minError;
            this.learningRate = learningRate;

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