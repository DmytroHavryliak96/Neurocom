using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.Models;


namespace Neurocom.BL.Interfaces
{
    public interface IManageTest
    {
        NetworkViewModel GetTestNetwork(int testNetwork);
        IEnumerable<NetworkViewModel> GetAllTestNetworks();

        IEnumerable<TaskNetwork> GetAllTasks();

        IEnumerable<NetworkTaskViewModel> GetNetworksForTask(int taskId);

       
    }
}
