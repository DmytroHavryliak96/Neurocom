using Neurocom.BL.NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neurocom.BL.Services
{
    public abstract class LVQService
    {
        protected double minError;
        protected double learningRate;
        protected double decayRate;
        protected int numOfClusters;
        protected double[][] inputs;
        protected int[] clusters;
        protected LVQ lvq;

        public LVQService()
        {
        }

        public LVQService(string xml)
        {
            this.lvq = new LVQ(xml);
        }

        public LVQ GetLvq()
        {
            return this.lvq;
        }

    }
}