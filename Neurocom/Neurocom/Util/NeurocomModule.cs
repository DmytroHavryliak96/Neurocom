using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;
using Neurocom.BL.Interfaces;
using Neurocom.BL.Services;
using Neurocom.BL.Services.ControllerServices;
using Neurocom.BL.Services.ControllerServices.AdminControllerServices;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.CustomModels;
using Neurocom.Models;
using System.Reflection;


namespace Neurocom.Util
{
    public class NeurocomModule : NinjectModule
    {
        private string connectionString;

        public NeurocomModule(string connection)
        {
            connectionString = connection;
        }



        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IAdminService>().To<AdminService>();
            Bind<ITrainNetworkService>().To<TrainNetworkService>();
            Bind<ITestNetwork>().To<TestNetworkService>();
            Bind<IManageNetwork>().To<ManageNetworksService>();
            Bind<IInputConverter>().To<ConverterService>();
            Bind<IManageTasks>().To<ManageTasksService>();
            Bind<IManageTest>().To<ManageTestNetworksService>();

            Bind<Func<NetworkInitializer, INetworkService>>().ToMethod(
                context =>
                {
                    return (networkInitializer =>
                    {
                        switch(networkInitializer.networkName + networkInitializer.taskName)
                        {
                            case "BPNKerogen":
                                return new BackPropagationKerogenService();
                            case "BPNLayer":
                                return new BackPropagationLayerService();
                            case "LVQKerogen":
                                return new LVQKerogenService();
                            case "LVQLayer":
                                return new LVQLayerService();
                            default:
                                throw new ArgumentException("cannot find specified network");
                        }
                    }
                    );
                });


            Bind<Func<NetworkViewModel, string, INetworkService>>().ToMethod(
                context =>
                {
                    return ((model, xml) =>
                    {
                        switch (model.NetworkName + model.TaskName)
                        {
                            case "BPNKerogen":
                                return new BackPropagationKerogenService(xml);
                            case "BPNLayer":
                                return new BackPropagationLayerService(xml);
                            case "LVQKerogen":
                                return new LVQKerogenService(xml);
                            case "LVQLayer":
                                return new LVQLayerService(xml);
                            default:
                                throw new ArgumentException("cannot find specified network");
                        }
                    }
                    );
                });

            Bind<Func<NetworkInitializer, IUnitOfWork, IAnswerService>>().ToMethod(
                context =>
                {
                    return ((networkInitializer, db) =>
                    {
                        switch (networkInitializer.taskName)
                        {
                            case "Kerogen":
                                return new KerogenAnswerService(db);
                            case "Layer":
                                return new LayerAnswerService(db);
                            default:
                                throw new ArgumentException("cannot find specified task");
                        }
                    }
                    );
                });


            Bind<Func<InputDataModel, IUnitOfWork, IAnswerService>>().ToMethod(
                context =>
                {
                    return ((model, db) =>
                    {
                        switch (model.taskName)
                        {
                            case "Kerogen":
                                return new KerogenAnswerService(db);
                            case "Layer":
                                return new LayerAnswerService(db);
                            default:
                                throw new ArgumentException("cannot find specified task");
                        }
                    }
                    );
                });

            Bind<Func<NetworkTaskViewModel, IUnitOfWork, IAnswerService>>().ToMethod(
             context =>
             {
                 return ((model, db) =>
                 {
                     switch (model.TaskName)
                     {
                         case "Kerogen":
                             return new KerogenAnswerService(db);
                         case "Layer":
                             return new LayerAnswerService(db);
                         default:
                             throw new ArgumentException("cannot find specified task");
                     }
                 }
                 );
             });

    
            Bind<Func<NetworkTaskViewModel, IAnswerService, NetworkInitializer>>().ToMethod(
                context =>
                {
                    return ((network, answer) =>
                    {
                        switch (network.Name)
                        {
                            case "BPN":
                                return new BPNInitializer { taskName = network.TaskName, networkName = network.Name, parameters = answer.GetParameters()};
                            case "LVQ":
                                return new LVQInitializer { taskName = network.TaskName, networkName = network.Name, answers = answer.GetAnswers(),
                                    patterns = answer.GetInputs(), numOfClusters = answer.GetNumOfClusters()};
                            default:
                                throw new ArgumentException("cannot find specified network");
                        }
                    }
                    );
                });

            Bind<Func<NetworkInitializer, IAnswerService, NetworkInitializer>>().ToMethod(
              context =>
              {
                  return ((network, answer) =>
                  {
                      switch (network.networkName)
                      {
                          case "BPN":
                              return network;
                          case "LVQ":
                              {
                                  var net = (LVQInitializer)network;
                                  net.patterns = answer.GetInputs();
                                  net.answers = answer.GetAnswers();
                                  return net;
                              }
                          default:
                              throw new ArgumentException("cannot find specified network");
                      }
                  }
                  );
              });

            Bind<Func<TrainedNetwork, InputDataModel>>().ToMethod(
                context =>
                {
                    return ((network) =>
                    {
                        switch (network.AvailableNetwork.Task.Name)
                        {
                            case "Kerogen":
                                return new KerogenInput { taskName = network.AvailableNetwork.Task.Name, trainedNetworkId = network.Id, answer = ""};
                            case "Layer":
                                return new LayerInput { taskName = network.AvailableNetwork.Task.Name, trainedNetworkId = network.Id, answer = "" };
                            default:
                                throw new ArgumentException("cannot find specified task");
                        }
                    }
                    );
                });

            Bind<Func<InputDataModel, double[]>>().ToMethod(
                context =>
                {
                    return ((inputModel) =>
                    {
                        PropertyInfo[] propertyInfos;
                       
                        switch (inputModel.taskName)
                        {
                            case "Kerogen":
                                {
                                    propertyInfos = typeof(Kerogen).GetProperties();
                                    var model = (KerogenInput)inputModel;
                                    double[] testinput = new double[propertyInfos.Length - 2];
                                    testinput[0] = model.Oxygen;
                                    testinput[1] = model.Hydrogen;
                                    testinput[2] = model.Carbon;
                                    testinput[3] = model.Nitrogen;
                                    testinput[4] = model.Sulfur;
                                    return testinput;
                                }
                            case "Layer":
                                {
                                    propertyInfos = typeof(Layer).GetProperties();
                                    var model = (LayerInput)inputModel;
                                    double[] testinput = new double[propertyInfos.Length - 2];
                                    testinput[0] = model.Porosity;
                                    testinput[1] = model.Clayness;
                                    testinput[2] = model.Carbonate;
                                    testinput[3] = model.Amplitude;
                                    return testinput;
                                    
                                }
                            default:
                                throw new ArgumentException("cannot find specified task");
                        }
                    }
                    );
                });

            Bind<Func<NetworkTaskViewModel, NetworkInitializer>>().ToMethod(
           context =>
           {
               return ((network) =>
               {
                   switch (network.Name)
                   {
                       case "BPN":
                           return new BPNInitializer();
                       case "LVQ":
                           return new LVQInitializer();
                       default:
                           throw new ArgumentException("cannot find specified network");
                   }
               }
               );
           });



        }
    }
}