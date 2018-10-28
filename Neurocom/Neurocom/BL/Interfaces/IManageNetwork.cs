using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;

namespace Neurocom.BL.Interfaces
{
    interface IManageNetwork
    {
        IEnumerable<NetworkViewModel> GetAllNetworks();
        NetworkViewModel GetNetwork(int trainedNetworkId);

        int TestNetworkFromDataBase
    }
}
