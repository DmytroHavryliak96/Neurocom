using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Neurocom.BL.Services;
using Neurocom.Models;
using System.Threading.Tasks;

namespace Neurocom.BL.Services.ControllerServices
{
    public class TrainNetworkService : ITrainNetworkService
    {
        private IUnitOfWork Database { get; set; }

        private Func<NetworkInitializer, INetworkService> networkBuilder;
        private Func<NetworkInitializer, IUnitOfWork, IAnswerService> answerBuilder;

        // визначення сервісів
        public TrainNetworkService(IUnitOfWork uow, Func<NetworkInitializer, INetworkService> networkResolver, Func<NetworkInitializer, IUnitOfWork, IAnswerService> answerResolver)
        {
            Database = uow;
            networkBuilder = networkResolver;
            answerBuilder = answerResolver;
        }
    
        // асинхронний метод для навчання нейронної мережі для певної задачі на основі даних користувача
        public async Task<TrainedNetwork> TrainNetworkAsync(NetworkInitializer data)
        {
            INetworkService service = networkBuilder(data); // виклик іос-контейнера для побудови необхідного сервіса-нейромережі
            IAnswerService answerservice = answerBuilder(data, Database); // виклик іос-контейнера для побудови необхідного сервіса для отримання відповідей для задач

            service.InitializeService(data); // ініціалізація сервіса
            service.CreateNetwork(); // створення мережі

            // асинхронне навчання нейронної мережі
            return await Task.Run(() => {
                service.Train(answerservice.GetInputs(), answerservice.GetAnswers());
                string xml = service.SaveNetworkXml(); // збереження навченої мережі

                TrainedNetwork network = new TrainedNetwork();
                network.AvailableNetworkId = Database.AvailableNetworks.Find(aNet => aNet.NeuralNetwork.Name.Equals(data.networkName) && aNet.Task.Name.Equals(data.taskName)).FirstOrDefault().Id;
                network.XmlName = xml;
                return network;

            });  
        }
    }
}