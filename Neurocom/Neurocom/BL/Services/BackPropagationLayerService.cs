﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.BL.NetworkLibrary;

namespace Neurocom.BL.Services
{
    public class BackPropagationLayerService : BackPropagationServices, INetworkService
    {
        public BackPropagationLayerService() : base()
        {

        }

        public BackPropagationLayerService(string xml) : base(xml)
        {
            this.bpn = new BackPropagationNetwork(xml);
        }

        public void CreateNetwork()
        {
            bpn = new BackPropagationNetwork(layerSizes, TFuncs);
        }

        public string SaveNetworkXml()
        {
            return this.bpn.Save();
        }

        public int TestNetwork(double[] test)
        {
            int result;
            double[] output = new double[1];
            result = bpn.getClusterPlast(test, output);
            return result;
        }

        public virtual void Train(double[][] inputs, double[][] answers)
        {
            bpn.TrainNetwork(inputs, Normalize.FormAnswersBackPropagationPlast(answers), minError, learningRate, Momentum);
        }

        public virtual void InitializeService(NetworkInitializer initializer)
        {
            BPNInitializer input = (BPNInitializer)initializer;
            this.parameters = input.parameters;
            this.hidden = input.hidden;
            this.learningRate = input.learningRate;
            this.Momentum = input.Momentum;
            this.minError = input.minError;
            layerSizes = new int[3] { this.parameters, this.hidden, 1 };
            TFuncs = new TransferFunction[3] {TransferFunction.None,
                                                               TransferFunction.Sigmoid,
                                                               TransferFunction.Sigmoid};
        }
    }
}