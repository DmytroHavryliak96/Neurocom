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

        public AvailableNetworksRepository(ApplicationDbContext context)
        {
            db = context;
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
            return this.GetAll().Where(predicate);
        }

        public AvailableNetwork Get(int id)
        {
            AvailableNetwork aNet = db.AvailableNetworks.Find(id);
            aNet.NeuralNetwork = db.NeuralNetworks.Find(aNet.NeuralNetworkId);
            aNet.NeuralNetwork.NetworkType = db.NetworkTypes.Find(aNet.NeuralNetwork.NetworkTypeId);
            aNet.Task = db.Tasks.Find(aNet.TaskId);
            return aNet;
        }

        public IEnumerable<AvailableNetwork> GetAll()
        {
            IEnumerable<AvailableNetwork> networks = db.AvailableNetworks;
            var listNetworks = networks.ToList();
            for (int i = 0; i < listNetworks.Count(); i++)
            {
                listNetworks[i].NeuralNetwork = db.NeuralNetworks.Find(listNetworks[i].NeuralNetworkId);
                listNetworks[i].NeuralNetwork.NetworkType = db.NetworkTypes.Find(listNetworks[i].NeuralNetwork.NetworkTypeId);
                listNetworks[i].Task = db.Tasks.Find(listNetworks[i].TaskId);
            }
            return listNetworks;
        }

        public void Update(AvailableNetwork entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}