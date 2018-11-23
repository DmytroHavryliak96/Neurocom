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

        public void Create(TrainedNetwork entity, string currentUserId)
        {
            if (currentUserId == null)
            {
                entity.User = null;
            }
            else
            {

                entity.User = db.Users.Find(currentUserId);
            }
            entity.CreatedDate = DateTime.Now;
            db.TrainedNetworks.Add(entity);
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
            return this.GetAll().Where(predicate);
        }

        public TrainedNetwork Get(int id)
        {
            TrainedNetwork network = db.TrainedNetworks.Find(id);         
            network.AvailableNetwork = availableNetworkRepository.Get(network.AvailableNetworkId);
            network.User = db.Users.Find(network.User.Id);
            
            return network;
        }

        public IEnumerable<TrainedNetwork> GetAll()
        {
            IEnumerable<TrainedNetwork> networks = db.TrainedNetworks;
            var listNetworks = networks.ToList();
            for (int i = 0; i < listNetworks.Count(); i++)
            {
                listNetworks[i].AvailableNetwork = availableNetworkRepository.Get(listNetworks[i].AvailableNetworkId);
                listNetworks[i].User = db.Users.Find(listNetworks[i].User.Id);
            }
            return listNetworks;
        }

        public void Update(TrainedNetwork item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}