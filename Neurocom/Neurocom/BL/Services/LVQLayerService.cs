using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.BL.NetworkLibrary;


namespace Neurocom.BL.Services
{
    public class LVQLayerService : LVQService, INetworkService
    {
        public LVQLayerService() : base()
        {

        }

        public LVQLayerService(string xml) : base(xml)
        {

        }

        public void CreateNetwork()
        {
            lvq = new LVQ(inputs, clusters, minError, learningRate, decayRate, numOfClusters);
        }

        public void InitializeService(NetworkInitializer initializer)
        {
            LVQInitializer input = (LVQInitializer) initializer;
            int VEC_LEN = input.patterns[0].Length;
            int TRAINING_PATTERNS = input.patterns.GetUpperBound(0) + 1;

            inputs = new double[TRAINING_PATTERNS][];
            for (int i = 0; i < TRAINING_PATTERNS; i++)
                inputs[i] = new double[VEC_LEN];

            for (int i = 0; i < TRAINING_PATTERNS; i++)
                for (int j = 0; j < VEC_LEN; j++)
                    inputs[i][j] = input.patterns[i][j];

            this.clusters = new int[TRAINING_PATTERNS];
            int[] formedAnswers = Normalize.FormAnswersLVQ(input.answers);
            for (int i = 0; i < TRAINING_PATTERNS; i++)
                this.clusters[i] = formedAnswers[i];

            this.minError = input.minError;
            this.learningRate = input.learningRate;
            this.decayRate = input.decayRate;
            this.numOfClusters = input.numOfClusters;
        }

        public string SaveNetworkXml()
        {
            return this.lvq.Save();
        }

        public int TestNetwork(double[] test)
        {
            return lvq.getClusterPlast(test);
        }

        public void Train(double[][] inputs, double[][] answers)
        {
            lvq.TrainNetwork();
        }
    }
}