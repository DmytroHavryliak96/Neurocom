using Neurocom.BL.NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;


namespace Neurocom.BL.Services.GeneticServices
{
    public class GeneticService : IGeneticService
    {
        private double crossover;
        private double mutationRate;
        private int populationSize;
        private int iterations;
        private int weights;

        private GA ga;

        public GeneticService()
        {
        }

        public void InitializeGA(NetworkInitializer initializer, Func<double[], double> fitnessFunction)
        {
            var input = (GeneticInitializer)initializer;
            this.crossover = input.Crossover;
            this.iterations = input.Iterations;
            this.mutationRate = input.MutationRate;
            this.populationSize = input.PopulationSize;

            InitializeWeights(initializer);

            ga = new GA(crossover, mutationRate, populationSize, iterations, weights);
            ga.Elitism = true;
            GAFunction func = new GAFunction(fitnessFunction);
            ga.FitnessFunction = func;
        }

        public void TrainGA()
        {
            ga.Go();
        }

        public void GetBestWeights(out double[] weights, out double fitness)
        {
            ga.GetBest(out weights, out fitness);
        }

        private void InitializeWeights(NetworkInitializer initializer)
        {
            var input = (GeneticInitializer)initializer;
            weights = input.hidden * (input.parameters + 3);
        }

    }
}