using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Data.Entity;

namespace Neurocom.DAO.Repositories
{
    public class NeuralNetworkRepository : IRepository<NeuralNetwork>
    {
        private ApplicationDbContext db;

        public NeuralNetworkRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(NeuralNetwork item)
        {
            db.NeuralNetworks.Add(item);
        }

        public void Delete(int id)
        {
            var neuralNetwork = db.NeuralNetworks.Find(id);

            if(neuralNetwork != null)
            {
                db.NeuralNetworks.Remove(neuralNetwork);
            }
        }

        public IEnumerable<NeuralNetwork> Find(Func<NeuralNetwork, bool> predicate)
        {
            return db.NeuralNetworks.Include(o => o.NetworkType).Where(predicate);
        }

        public NeuralNetwork Get(int id)
        {
    
            return db.NeuralNetworks.Include(o => o.NetworkType).FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<NeuralNetwork> GetAll()
        {
            return db.NeuralNetworks.Include(o => o.NetworkType).ToList();
        }

        public void Update(NeuralNetwork entity)
        {
            NeuralNetwork dbEntry = db.NeuralNetworks.Find(entity.Id);
            if (dbEntry != null)
            {
                dbEntry.Description = entity.Description;
            }
        }
    }
}