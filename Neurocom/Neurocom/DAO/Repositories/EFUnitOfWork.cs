using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.DAO.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        private ApplicationUserRepository applicationUserRepository;
        private KerogenRepository kerogenRepository;
        private LayerRepository layerRepository;
        private NetworkTypeRepository networkTypeRepository;
        private NeuralNetworkRepository neuralNetworkRepository;
        private TaskNetworkRepository taskNetworkRepository;
        private AvailableNetworksRepository availableNetworkRepository;
        private TrainedNetworkRepository trainedNetworkRepository;
        private TestNetworkRepository testNetworkRepository;

        public EFUnitOfWork()
        {
            db = new ApplicationDbContext();
        }

        public IRepository<AvailableNetwork> AvailableNetworks
        {
            get
            {
                if (availableNetworkRepository == null)
                    availableNetworkRepository = new AvailableNetworksRepository(db);

                return availableNetworkRepository;
            }

        }

        public IRepository<Kerogen> Kerogens
        {
            get
            {
                if(kerogenRepository == null) 
                    kerogenRepository = new KerogenRepository(db);

                return kerogenRepository;
            }

        }

        public IRepository<Layer> Layers
        {
            get
            {
                if (layerRepository == null)
                    layerRepository = new LayerRepository(db);
                return layerRepository;
            }

        }

        public IRepository<NeuralNetwork> NeuralNetworks
        {
            get
            {
                if (neuralNetworkRepository == null)
                    neuralNetworkRepository = new NeuralNetworkRepository(db);

                return neuralNetworkRepository;
            }

        }

        public IRepository<TaskNetwork> TaskNetworks
        {
            get
            {
                if (taskNetworkRepository == null)
                    taskNetworkRepository = new TaskNetworkRepository(db);

                return taskNetworkRepository;
            }

        }

        public IRepository<TestNetwork> TestNetworks
        {
            get
            {
                if (testNetworkRepository == null)
                    testNetworkRepository = new TestNetworkRepository(db, TrainedNetworks);

                return TestNetworks;
            }

        }

        public IRepository<TrainedNetwork> TrainedNetworks
        {
            get
            {
                if (trainedNetworkRepository == null)
                    trainedNetworkRepository = new TrainedNetworkRepository(db, AvailableNetworks);

                return trainedNetworkRepository;
            }

        }

        public IRepository<NetworkType> Types
        {
            get
            {
                if (networkTypeRepository == null)
                    networkTypeRepository = new NetworkTypeRepository(db);

                return networkTypeRepository;
            }

        }

        public IRepository<ApplicationUser> Users
        {
            get
            {
                if (applicationUserRepository == null)
                    applicationUserRepository = new ApplicationUserRepository(db);
                return applicationUserRepository;
            }

        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
    }
}