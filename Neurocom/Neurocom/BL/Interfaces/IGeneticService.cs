using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.BL.Services;


namespace Neurocom.BL.Interfaces
{
    interface IGeneticService
    {
        void InitializeGA(NetworkInitializer initializer, Func<double[], double> fitnessFunction);
        void TrainGA();
        void GetBestWeights(out double[] weights, out double fitness);
    }
}
