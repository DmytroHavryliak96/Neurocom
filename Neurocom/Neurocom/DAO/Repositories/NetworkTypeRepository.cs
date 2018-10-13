using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.DAO.Repositories
{
    public class NetworkTypeRepository : IRepository<NetworkType>
    {
        private ApplicationDbContext db;

        public NetworkTypeRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(NetworkType item)
        {
            db.NetworkTypes.Add(item);
        }


        public void Delete(int id)
        {
            var netType = db.NetworkTypes.Find(id);

            if(netType != null)
            {
                db.NetworkTypes.Remove(netType);
            }
        }

        public IEnumerable<NetworkType> Find(Func<NetworkType, bool> predicate)
        {
            return db.NetworkTypes.Where(predicate);
        }

        public NetworkType Get(int id)
        {
            return db.NetworkTypes.Find(id);
        }

        public IEnumerable<NetworkType> GetAll()
        {
            return db.NetworkTypes;
        }

        public void Update(NetworkType entity)
        {
            NetworkType dbEntry = db.NetworkTypes.Find(entity.Id);
            if (dbEntry != null)
            {
                dbEntry.Description = entity.Description;
                dbEntry.Name = entity.Name;
            }

            db.SaveChanges();
        }
    }
}