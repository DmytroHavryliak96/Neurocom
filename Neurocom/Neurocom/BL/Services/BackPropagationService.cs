using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.BL.NetworkLibrary;

namespace Neurocom.BL.Services
{
    public abstract class BackPropagationServices
    {
        protected TransferFunction[] TFuncs;
        protected int parameters;
        protected int hidden;
        protected double learningRate;
        protected double Momentum;
        protected double minError;
        protected int[] layerSizes;

        protected BackPropagationNetwork bpn;

        public BackPropagationServices()
        {
        }

        public BackPropagationServices(string xml)
        {
            this.bpn = new BackPropagationNetwork(xml);
        }

        public BackPropagationNetwork GetBPN()
        {
            return this.bpn;
        }

    }

}