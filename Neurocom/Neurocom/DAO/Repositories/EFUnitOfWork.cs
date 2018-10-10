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

        public EFUnitOfWork()
        {
            db = new ApplicationDbContext();
        }

        public IRepository<AvailableNetwork> AvailableNetworks
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

        }

        public IRepository<TaskNetwork> TaskNetworks
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public IRepository<TestNetwork> TestNetworks
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public IRepository<TrainedNetwork> TrainedNetworks
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public IRepository<NetworkType> Types
        {
            get
            {
                throw new NotImplementedException();
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