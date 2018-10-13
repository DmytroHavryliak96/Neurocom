using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Data.Entity;

namespace Neurocom.DAO.Repositories
{
    public class AvailableNetworksRepository : IRepository<AvailableNetwork>
    {
        private ApplicationDbContext db;
        private IRepository<NeuralNetwork> neuralNetworkRepository;

        public AvailableNetworksRepository(ApplicationDbContext context, IRepository<NeuralNetwork> rep)
        {
            db = context;
            neuralNetworkRepository = rep;
        }

        public void Create(AvailableNetwork item)
        {
            db.AvailableNetworks.Add(item);
        }


        public void Delete(int id)
        {
            var aNetwork = db.AvailableNetworks.Find(id);

            if(aNetwork != null)
            {
                db.AvailableNetworks.Remove(aNetwork);
            }
        }

        public IEnumerable<AvailableNetwork> Find(Func<AvailableNetwork, bool> predicate)
        {
            return db.AvailableNetworks.Include(o => o.NeuralNetwork).Include(o => o.Task).Where(predicate);
        }

        public AvailableNetwork Get(int id)
        {
            AvailableNetwork aNet = db.AvailableNetworks.Find(id);
            aNet.NeuralNetwork = neuralNetworkRepository.Get(aNet.NeuralNetworkId);
            aNet.Task = db.Tasks.Find(aNet.TaskId);
            return aNet;
        }

        public IEnumerable<AvailableNetwork> GetAll()
        {
            return db.AvailableNetworks.Include(o => o.NeuralNetwork).Include(o => o.Task);
        }

        public void Update(AvailableNetwork entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}