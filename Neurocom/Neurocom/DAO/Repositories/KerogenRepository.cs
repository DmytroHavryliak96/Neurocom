using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.DAO.Repositories
{
    public class KerogenRepository : IRepository<Kerogen>
    {
        private ApplicationDbContext db;

        public KerogenRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(Kerogen item)
        {
            db.Kerogens.Add(item);
        }

        public void Create(Kerogen item, string pass)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            Kerogen kerogen = db.Kerogens.Find(id);

            if(kerogen != null)
            {
                db.Kerogens.Remove(kerogen);
            }
        }

        public IEnumerable<Kerogen> Find(Func<Kerogen, bool> predicate)
        {
            return db.Kerogens.Where(predicate).ToList();
        }

        public Kerogen Get(int id)
        {
            return db.Kerogens.Find(id);
        }

        public IEnumerable<Kerogen> GetAll()
        {
            return db.Kerogens;
        }

        public void Update(Kerogen entity)
        {
            if (entity.Id == 0)
            {
                db.Kerogens.Add(entity);
            }
            else
            {
                Kerogen dbEntry = db.Kerogens.Find(entity.Id);
                if (dbEntry != null)
                {
                    dbEntry.Carbon = entity.Carbon;
                    dbEntry.Hydrogen = entity.Hydrogen;
                    dbEntry.Nitrogen = entity.Nitrogen;
                    dbEntry.Oxygen = entity.Oxygen;
                    dbEntry.Sulfur = entity.Sulfur;
                    dbEntry.Type = entity.Type;
                }
            }
        }
    }
}