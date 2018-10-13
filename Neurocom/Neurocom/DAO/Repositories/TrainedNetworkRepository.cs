using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Data.Entity;

namespace Neurocom.DAO.Repositories
{
    public class TrainedNetworkRepository : IRepository<TrainedNetwork>
    {
        private ApplicationDbContext db;
        private IRepository<AvailableNetwork> availableNetworkRepository;

        public TrainedNetworkRepository(ApplicationDbContext context, IRepository<AvailableNetwork> rep)
        {
            db = context;
            availableNetworkRepository = rep;
        }

        public void Create(TrainedNetwork item)
        {
            db.TrainedNetworks.Add(item);
        }


        public void Delete(int id)
        {
            var trainedNetwork = db.TrainedNetworks.Find(id);

            if(trainedNetwork != null)
            {
                db.TrainedNetworks.Remove(trainedNetwork);
            }
        }

        public IEnumerable<TrainedNetwork> Find(Func<TrainedNetwork, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public TrainedNetwork Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainedNetwork> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(TrainedNetwork item)
        {
            throw new NotImplementedException();
        }
    }
}