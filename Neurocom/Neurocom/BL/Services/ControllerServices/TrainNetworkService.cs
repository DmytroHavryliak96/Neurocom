using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Neurocom.BL.Services;
using Neurocom.Models;

namespace Neurocom.BL.Services.ControllerServices
{
    public class TrainNetworkService : ITrainNetworkService
    {
        private IUnitOfWork Database { get; set; }
        private Func<NetworkInitializer, INetworkService> networkBuilder;

        // необхідно додати сервіс для визначення відповіді мережі
        public TrainNetworkService(IUnitOfWork uow, Func<NetworkInitializer, INetworkService> networkResolver)
        {
            Database = uow;
            networkBuilder = networkResolver;
        }

        // необхідно додати асинхронність...
        public void TrainNetwork(NetworkInitializer data)
        {
            // заглушка
            double[][] inputs = new double[1][];
            inputs[0] = new double[1];
            inputs[0][0] = 0.0;

            double[][] answers = new double[1][];
            answers[0] = new double[1];
            answers[0][0] = 0.0;
            // 

            INetworkService service = networkBuilder(data); // виклик іос-контейнера для побудови необхідного сервіса-нейромережі
            service.InitializeService(data); // ініціалізація сервіса
            service.CreateNetwork(); // створення мережі
            service.Train(inputs, answers); // навчання мережі

            string xml = service.SaveNetworkXml(); // збереження навченої мережі

            TrainedNetwork network = new TrainedNetwork();
            network.AvailableNetworkId = Database.AvailableNetworks.Find(aNet => aNet.NeuralNetwork.Equals(data.networkName) && aNet.Task.Equals(data.taskName)).FirstOrDefault().Id;
            network.XmlName = xml;
            //....
        }
    }
}