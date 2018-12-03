using Neurocom.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.CustomModels;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;

namespace Neurocom.BL.Services.ControllerServices.UserControllerServices
{
    public class IUserService : IUserController
    {
        private IUnitOfWork database;

        private Func<TrainedNetwork, InputDataModel> _inputResover;
        private IInputConverter _converterResolver;
        private Func<InputDataModel, IUnitOfWork, IAnswerService> _answerResolver;
        private ITestNetwork _testResolver;
        private ITrainNetworkService _trainService;
        private Func<NetworkTaskViewModel, IUnitOfWork, IAnswerService> _answerResolverCreate;
        private Func<NetworkTaskViewModel, IAnswerService, NetworkInitializer> _resolver;
        private Func<NetworkInitializer, IAnswerService, NetworkInitializer> _initializerTypeResolver;
        private Func<NetworkInitializer, IUnitOfWork, IAnswerService> _answerForTraining;

        public IUserService(IUnitOfWork  db, Func<TrainedNetwork, InputDataModel> input, IInputConverter converter_, 
            Func<InputDataModel, IUnitOfWork, IAnswerService> answer, ITestNetwork test, Func<NetworkTaskViewModel, IUnitOfWork, IAnswerService> answerResolverCreate,
            Func<NetworkTaskViewModel, IAnswerService, NetworkInitializer> resolver, Func<NetworkInitializer, IAnswerService, NetworkInitializer> init,
            Func<NetworkInitializer, IUnitOfWork, IAnswerService> trainingAnswer, ITrainNetworkService trainService)
        {
            database = db;
            _inputResover = input;
            _converterResolver = converter_;
            _answerResolver = answer;
            _testResolver = test;
            _answerResolverCreate = answerResolverCreate;
            _resolver = resolver;
            _initializerTypeResolver = init;
            _answerForTraining = trainingAnswer;
            _trainService = trainService;
        }

        public InputDataModel CreateAnswerModel(InputDataModel _model)
        {
            var containerInput = _converterResolver.ConvertVector(_model);
            int answer = _testResolver.TestNetworkFromDataBase(_model.trainedNetworkId, containerInput);
            IAnswerService answerService = _answerResolver(_model, database);
            _model.answer = answerService.GetAnswer(answer);
            return _model;
        }

        public InputDataModel CreateDataInput(int _trainedNetworkId)
        {
            return _inputResover(database.TrainedNetworks.Get(_trainedNetworkId));
        }

        public void EditProfile(ApplicationUser _user, HttpPostedFileBase _image)
        {
            if (_image != null)
            {
                _user.ImageMimeType = _image.ContentType;
                _user.ImageData = new byte[_image.ContentLength];
                _image.InputStream.Read(_user.ImageData, 0, _image.ContentLength);
            }

            ApplicationUser appUser = new ApplicationUser
            {
                Id = _user.Id,
                Address = _user.Address,
                Email = _user.Email,
                UserName = _user.UserName,
                PhoneNumber = _user.PhoneNumber,
                ImageData = _user.ImageData,
                ImageMimeType = _user.ImageMimeType
            };

            database.Users.Update(appUser);
            database.Save();
        }

        public IEnumerable<TaskNetwork> GetAllTasks()
        {
            return database.TaskNetworks.GetAll();
        }

        public IEnumerable<NetworkViewModel> GetAllTestNetworks()
        {
            var value = database.TestNetworks.GetAll();
            List<NetworkViewModel> models = new List<NetworkViewModel>();
            foreach (var net in value)
            {
                models.Add(GetTestNetwork(net.Id));
            }
            return models;
        }

        public IEnumerable<NetworkViewModel> GetAllUserNetworks(string _userId)
        {
            var value = database.TrainedNetworks.Find(tr => tr.User.Id.Equals(_userId));
            List<NetworkViewModel> models = new List<NetworkViewModel>();
            foreach (var net in value)
            {
                models.Add(GetTrainedNetwork(net.Id));
            }
            return models;
        }

        public IEnumerable<NetworkTaskViewModel> GetNetworksForTask(int _taskId)
        {
            var value = database.AvailableNetworks.Find(val => val.TaskId == _taskId);
            var networkTaskViewModels = new List<NetworkTaskViewModel>();

            foreach (var item in value)
            {
                NetworkTaskViewModel model = new NetworkTaskViewModel
                {
                    Id = item.Id,
                    Name = item.NeuralNetwork.Name,
                    Description = item.NeuralNetwork.Description,
                    TaskName = item.Task.Name,
                    NetworkTypeProperty = item.NeuralNetwork.NetworkType.Name,
                };
                networkTaskViewModels.Add(model);
            }
            return networkTaskViewModels;
        }

        public ApplicationUser GetUser(string _userId)
        {
            var value = (ApplicationUserRepository)database.Users;
            return value.Get(_userId);
        }

        public NetworkViewModel GetTrainedNetwork(int trainedNetworkId)
        {
            var trainedNetwork = database.TrainedNetworks.Get(trainedNetworkId);
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

        public NetworkViewModel GetTestNetwork(int _testId)
        {
            var testNetwork = database.TestNetworks.Get(_testId);
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

        public IEnumerable<NetworkViewModel> GetAllNetworks()
        {
            List<NetworkViewModel> models = new List<NetworkViewModel>();
            foreach (var net in database.NeuralNetworks.GetAll())
            {
                models.Add(GetNetwork(net.Id));
            }
            return models;
        }

        public NetworkViewModel GetNetwork(int _netId)
        {
            var network = database.NeuralNetworks.Get(_netId);
            if (network != null)
            {
                NetworkViewModel model = new NetworkViewModel
                {
                    NetworkName = network.Name,
                    NetworkId = network.Id,
                    NetworkType = network.NetworkType.Name,
                    TypeDescription = network.NetworkType.Description,
                    NetworkDescription = network.Description
                };
                return model;
            }
            return null;
        }

        public NetworkInitializer GetNetworkInitializer(NetworkTaskViewModel model)
        {
            IAnswerService answer = _answerResolverCreate(model, database);
            NetworkInitializer item = _resolver(model, answer);
            return item;
        }

        public void TrainNetwork(NetworkInitializer data, string userId)
        {
            data = _initializerTypeResolver(data, _answerForTraining(data, database));

            var network = _trainService.TrainNetwork(data, userId);

            network.AvailableNetworkId = database.AvailableNetworks.Find(aNet => aNet.NeuralNetwork.Name.Equals(data.networkName) && aNet.Task.Name.Equals(data.taskName)).FirstOrDefault().Id;

            var rep = (TrainedNetworkRepository)database.TrainedNetworks;
            rep.Create(network, userId);

            database.Save();
        }

        public void DeleteUserNetwork(int _testId)
        {
            throw new NotImplementedException();
        }
    }
}