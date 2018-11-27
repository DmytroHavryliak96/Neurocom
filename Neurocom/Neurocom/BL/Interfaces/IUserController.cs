using Neurocom.BL.Services;
using Neurocom.CustomModels;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Neurocom.BL.Interfaces
{
    public interface IUserController
    {
        IEnumerable<NetworkViewModel> GetAllUserNetworks(string _userId);

        IEnumerable<TaskNetwork> GetAllTasks();

        IEnumerable<NetworkTaskViewModel> GetNetworksForTask(int _taskId);

        IEnumerable<NetworkViewModel> GetAllTestNetworks();

        ApplicationUser GetUser(string _userId);

        void EditProfile(ApplicationUser _user, HttpPostedFileBase _image);

        InputDataModel CreateDataInput(int _trainedNetworkId);

        InputDataModel CreateAnswerModel(InputDataModel _model);

        NetworkViewModel GetNetwork(int _netId);

        IEnumerable<NetworkViewModel> GetAllNetworks();

        NetworkInitializer GetNetworkInitializer(NetworkTaskViewModel model);

        void TrainNetwork(NetworkInitializer data, string userId);

        void DeleteUserNetwork(int _testId);

    }
}
