using Neurocom.BL.Services;
using Neurocom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Neurocom.BL.Interfaces
{
    public interface ITrainNetworkService
    {
        void TrainNetworkAsync(NetworkInitializer data, string userId);

        void Dispose();
    }
}