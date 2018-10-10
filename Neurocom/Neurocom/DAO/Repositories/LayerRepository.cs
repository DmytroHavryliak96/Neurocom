using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.DAO.Repositories
{
    public class LayerRepository : IRepository<Layer>
    {
        private ApplicationDbContext db;

        public LayerRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(Layer item)
        {
            db.Layers.Add(item);
        }

        public void Create(Layer item, string pass)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            Layer layer = db.Layers.Find(id);

            if(layer != null)
            {
                db.Layers.Remove(layer);
            }
        }

        public IEnumerable<Layer> Find(Func<Layer, bool> predicate)
        {
            return db.Layers.Where(predicate);
        }

        public Layer Get(int id)
        {
            return db.Layers.Find(id);
        }

        public IEnumerable<Layer> GetAll()
        {
            return db.Layers;
        }

        public void Update(Layer entity)
        {
            if (entity.Id == 0)
            {
                db.Layers.Add(entity);
            }
            else
            {
                Layer dbEntry = db.Layers.Find(entity.Id);
                if (dbEntry != null)
                {
                    dbEntry.Amplitude = entity.Amplitude;
                    dbEntry.Carbonate = entity.Carbonate;
                    dbEntry.Clayness = entity.Clayness;
                    dbEntry.Porosity = entity.Porosity;
                    dbEntry.Type = entity.Type;
                }
            }
        }
    }
}