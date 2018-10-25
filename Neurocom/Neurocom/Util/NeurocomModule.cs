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

            Bind<Func<NetworkInitializer, IUnitOfWork, IAnswerService>>().ToMethod(
                context =>
                {
                    return ((networkInitializer, db) =>
                    {
                        switch (networkInitializer.taskName)
                        {
                            case "Kerogen":
                                return new KerogenAnswerService(db);
                           
                            default:
                                throw new ArgumentException("cannot find specified network");
                        }
                    }
                    );
                });
        }
    }
}