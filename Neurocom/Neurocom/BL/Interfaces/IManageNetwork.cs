using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.CustomModels;

namespace Neurocom.BL.Interfaces
{
    public interface IManageNetwork
    {
        IEnumerable<NetworkViewModel> GetAllTrainedNetworks();
        NetworkViewModel GetTrainedNetwork(int trainedNetworkId);

        IEnumerable<NetworkType> GetAllTypes();
        NetworkType GetNetworkType(int networkTypeId);
        void UpdateNetworkType(NetworkType netType);

        IEnumerable<NeuralNetwork> GetAllNeuralNetworks();
        NeuralNetwork GetNetwork(int networkId);
        void UpdateNetwork(NeuralNetwork net);

        InputDataModel CreateInputModel(int trainedNetworkId);
        InputDataModel GetAnswerModel(InputDataModel model);

        void Dispose();
    }
}
