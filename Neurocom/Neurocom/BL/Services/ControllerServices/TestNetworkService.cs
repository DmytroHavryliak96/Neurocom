using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Neurocom.ViewModels.AdminViewModels;

namespace Neurocom.BL.Services.ControllerServices
{
    public class TestNetworkService : ITestNetwork
    {
        private IUnitOfWork Database { get; set; }
        private Func<NetworkViewModel, string, INetworkService> networkBuilder;

        public TestNetworkService(IUnitOfWork uow, Func<NetworkViewModel, string, INetworkService> networkResolver)
        {
            Database = uow;
            networkBuilder = networkResolver;
        }

        public int TestNetworkFromDataBase(int trainedNetworkId, double[] testVector)
        {
            var trainedNetwok = Database.TrainedNetworks.Get(trainedNetworkId);
            NetworkViewModel model = new NetworkViewModel { TaskName = trainedNetwok.AvailableNetwork.Task.Name, NetworkName = trainedNetwok.AvailableNetwork.NeuralNetwork.Name};
            INetworkService service = networkBuilder(model, trainedNetwok.XmlName);
            return service.TestNetwork(testVector);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}