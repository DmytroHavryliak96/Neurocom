using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Data.Entity;

namespace Neurocom.DAO.Repositories
{
    public class TestNetworkRepository : IRepository<TestNetwork>
    {
        private ApplicationDbContext db;
        private IRepository<TrainedNetwork> trainedNetworkRepository;

        public TestNetworkRepository(ApplicationDbContext context, IRepository<TrainedNetwork> rep)
        {
            db = context;
            trainedNetworkRepository = rep;
        }

        public void Create(TestNetwork item)
        {
            db.TestNetworks.Add(item);
        }

        public void Create(TrainedNetwork entity, string userId)
        {
            entity.User = db.Users.Find(userId);
            entity.CreatedDate = DateTime.Now;
            db.TrainedNetworks.Add(entity);
            db.SaveChanges();

            TestNetwork testNetwork = new TestNetwork();
            testNetwork.TrainedNetwork = entity;
            testNetwork.TrainedNetworkId = entity.Id;
            db.TestNetworks.Add(testNetwork);

        }

        public void Delete(int id)
        {
            TestNetwork testNetwork = db.TestNetworks.Find(id);
            var trained = db.TrainedNetworks.Find(testNetwork.TrainedNetworkId);

            if (testNetwork != null)
            {
                db.TestNetworks.Remove(testNetwork);
                db.SaveChanges();

                db.TrainedNetworks.Remove(trained);
                db.SaveChanges();
            }
        }

        public IEnumerable<TestNetwork> Find(Func<TestNetwork, bool> predicate)
        {
            return this.GetAll().Where(predicate);
        }

        public TestNetwork Get(int id)
        {
            TestNetwork testNetwork = db.TestNetworks.Find(id);
            testNetwork.TrainedNetwork = trainedNetworkRepository.Get(testNetwork.TrainedNetworkId);
            return testNetwork;
        }

        public IEnumerable<TestNetwork> GetAll()
        {
            var testNetworks = db.TestNetworks.ToList();
            List<TestNetwork> result = new List<TestNetwork>();
            foreach (var item in testNetworks)
            {
                result.Add(this.Get(item.Id));
            }
            return result;
        }

        public void Update(TestNetwork item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}