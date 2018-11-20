using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.CustomModels;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.DAO.Interfaces;

namespace Neurocom.BL.Services.ControllerServices.AdminControllerServices
{
    public class ManageNetworksService : IManageNetwork
    {
        private IUnitOfWork Database { get; set; }
        private IInputConverter converter;
        private ITestNetwork testService;

        private Func<TrainedNetwork, InputDataModel> inputResolver;
        private Func<InputDataModel, IUnitOfWork, IAnswerService> answerBuilder;

        public ManageNetworksService(IUnitOfWork db, Func<TrainedNetwork, InputDataModel> input, IInputConverter converter_, Func<InputDataModel, IUnitOfWork, IAnswerService> answer, ITestNetwork test)
        {
            Database = db;
            inputResolver = input;
            converter = converter_;
            testService = test;
            answerBuilder = answer;
        }

        public InputDataModel CreateInputModel(int trainedNetworkId)
        {
            return inputResolver(Database.TrainedNetworks.Get(trainedNetworkId));
        }



        public IEnumerable<NeuralNetwork> GetAllNeuralNetworks()
        {
            return Database.NeuralNetworks.GetAll();
        }

        public IEnumerable<NetworkViewModel> GetAllTrainedNetworks()
        {
            List<NetworkViewModel> models = new List<NetworkViewModel>();
            foreach (var net in Database.TrainedNetworks.GetAll().OrderBy(net => net.Id)
            {
                models.Add(GetTrainedNetwork(net.Id));
            }
            return models;
        }

        public IEnumerable<NetworkType> GetAllTypes()
        {
            return Database.Types.GetAll();
        }

        public NeuralNetwork GetNetwork(int networkId)
        {
            return Database.NeuralNetworks.Get(networkId);
        }

        public NetworkType GetNetworkType(int networkTypeId)
        {
            return Database.Types.Get(networkTypeId);
        }

        public NetworkViewModel GetTrainedNetwork(int trainedNetworkId)
        {
            var trainedNetwork = Database.TrainedNetworks.Get(trainedNetworkId);
            if (trainedNetwork != null)
            {
                NetworkViewModel network = new NetworkViewModel
                {
                    TrainedNetworkID = trainedNetwork.Id,
                    NetworkName = trainedNetwork.AvailableNetwork.NeuralNetwork.Name,
                    NetworkId = trainedNetwork.AvailableNetwork.NeuralNetwork.Id,
                    NetworkType = trainedNetwork.AvailableNetwork.NeuralNetwork.NetworkType.Name,
                    NetworkTypeId = trainedNetwork.AvailableNetwork.NeuralNetwork.NetworkType.Id,
                    UserName = trainedNetwork.User.UserName,
                    UserId = trainedNetwork.User.Id,
                    TaskId = trainedNetwork.AvailableNetwork.Task.Id,
                    TaskName = trainedNetwork.AvailableNetwork.Task.Name,
                    CreatedDate = trainedNetwork.CreatedDate
                };
                return network;
            }
            return null;
        }

        public void UpdateNetwork(NeuralNetwork net)
        {
            Database.NeuralNetworks.Update(net);
            Database.Save();
        }

        public void UpdateNetworkType(NetworkType netType)
        {
            Database.Types.Update(netType);
            Database.Save();
        }

        public InputDataModel GetAnswerModel(InputDataModel model)
        {
            var containerInput = converter.ConvertVector(model);
            int answer = testService.TestNetworkFromDataBase(model.trainedNetworkId, containerInput);
            IAnswerService answerService = answerBuilder(model, Database);
            model.answer = answerService.GetAnswer(answer);
            return model;

        }

        public void Dispose()
        {
            Database.Dispose();
            testService.Dispose();
        }
    }
}