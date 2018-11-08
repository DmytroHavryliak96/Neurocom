using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.Models;
using Neurocom.CustomModels;
using Neurocom.BL.Services;

namespace Neurocom.BL.Interfaces
{
    public interface IManageTest
    {
        NetworkViewModel GetTestNetwork(int testNetworkId);
        IEnumerable<NetworkViewModel> GetAllTestNetworks();

        IEnumerable<TaskNetwork> GetAllTasks();

        IEnumerable<NetworkTaskViewModel> GetNetworksForTask(int taskId);

        NetworkInitializer GetNetworkInitializer(NetworkTaskViewModel model);

        void DeleteTestNetwork(int _testId);
    }
}
