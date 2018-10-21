using Neurocom.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neurocom.BL.Interfaces
{
    public interface ITrainNetworkService
    {
        void TrainNetwork(NetworkInitializer data);
    }
}