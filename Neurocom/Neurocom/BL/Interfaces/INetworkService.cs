using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.BL.Services;

namespace Neurocom.BL.Interfaces
{
    public interface INetworkService
    {
        void CreateNetwork();
        void Train(double[][] inputs, double[][] answers);
        void InitializeService(NetworkInitializer initializer);
        int TestNetwork(double[] test);
        string SaveNetworkXml();
    }
}
