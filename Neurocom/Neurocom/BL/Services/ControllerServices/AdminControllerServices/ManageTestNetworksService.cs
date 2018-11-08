using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using Neurocom.CustomModels;
using Neurocom.ViewModels.AdminViewModels;

namespace Neurocom.BL.Services.ControllerServices.AdminControllerServices
{
    public class ManageTestNetworksService : IManageTest
    {
        private IUnitOfWork Database { get; set; }
        private Func<NetworkTaskViewModel, IAnswerService, NetworkInitializer> resolver;
        private Func<NetworkTaskViewModel, IUnitOfWork, IAnswerService> answerResolver;

        public ManageTestNetworksService(IUnitOfWork db, Func<NetworkTaskViewModel, IAnswerService, NetworkInitializer> _resolver, Func<NetworkTaskViewModel, IUnitOfWork, IAnswerService> _answerResolver)
        {
            Database = db;
            resolver = _resolver;
            answerResolver = _answerResolver;
        }

        public void DeleteTestNetwork(int _testId)
        {
            Database.TestNetworks.Delete(_testId);
            Database.Save();
        }

        public IEnumerable<TaskNetwork> GetAllTasks()
        {
            return Database.TaskNetworks.GetAll();
        }

        public IEnumerable<NetworkViewModel> GetAllTestNetworks()
        {
            List<NetworkViewModel> models = new List<NetworkViewModel>();

            foreach(var net in Database.TestNetworks.GetAll())
            {
                models.Add(GetTestNetwork(net.Id));
            }
            return models;
        }

        public NetworkInitializer GetNetworkInitializer(NetworkTaskViewModel model)
        {
            IAnswerService answer = answerResolver(model, Database);
            NetworkInitializer item = resolver(model, answer);
            return item;
        }

        public IEnumerable<NetworkTaskViewModel> GetNetworksForTask(int taskId)
        {
            var task = Database.TaskNetworks.Get(taskId);
            var networks = Database.AvailableNetworks.Find(aNet => aNet.Task.Name.Equals(task.Name));
            var networkTaskViewModels = new List<NetworkTaskViewModel>();

            foreach (var item in networks)
            {
                NetworkTaskViewModel model = new NetworkTaskViewModel
                {
                    Id = item.Id,
                    Name = item.NeuralNetwork.Name,
                    Description = item.NeuralNetwork.Description,
                    TaskName = task.Name,
                    NetworkTypeProperty = item.NeuralNetwork.NetworkType.Name,
                };
                networkTaskViewModels.Add(model);
            }
            return networkTaskViewModels;
        }

        public NetworkViewModel GetTestNetwork(int testNetworkId)
        {
            var testNetwork = Database.TestNetworks.Get(testNetworkId);
            if (testNetwork != null)
            {
                NetworkViewModel network = new NetworkViewModel
                {
                    TestNetworkId = testNetwork.Id,
                    TrainedNetworkID = testNetwork.TrainedNetworkId,
                    NetworkName = testNetwork.TrainedNetwork.AvailableNetwork.NeuralNetwork.Name,
                    NetworkId = testNetwork.TrainedNetwork.AvailableNetwork.NeuralNetwork.Id,
                    NetworkType = testNetwork.TrainedNetwork.AvailableNetwork.NeuralNetwork.NetworkType.Name,
                    NetworkTypeId = testNetwork.TrainedNetwork.AvailableNetwork.NeuralNetwork.NetworkType.Id,
                    UserName = testNetwork.TrainedNetwork.User.UserName,
                    UserId = testNetwork.TrainedNetwork.User.Id,
                    TaskId = testNetwork.TrainedNetwork.AvailableNetwork.Task.Id,
                    TaskName = testNetwork.TrainedNetwork.AvailableNetwork.Task.Name,
                    CreatedDate = testNetwork.TrainedNetwork.CreatedDate
                };
                return network;
            }
            return null;
        }
    }
}