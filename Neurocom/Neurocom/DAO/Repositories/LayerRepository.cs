using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Reflection;

namespace Neurocom.DAO.Repositories
{
    public class LayerRepository : IRepository<Layer>, IDataInput
    {
        private ApplicationDbContext db;

        private int LayerParameters;

        public int GetLayerParameters()
        {
            return this.LayerParameters;
        }

        public int GetNumberOfLayerParameters()
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Layer).GetProperties();
            return propertyInfos.Length - 2;
        }

        public LayerRepository(ApplicationDbContext context)
        {
            this.db = context;
            this.LayerParameters = GetNumberOfLayerParameters();
        }

        public void Create(Layer item)
        {
            db.Layers.Add(item);
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

        public double[][] GetAnswers()
        {
            int amount = db.Layers.Count();

            double[][] answer = new double[amount][];

            for (int i = 0; i < amount; i++)
                answer[i] = new double[1];

            int k = 0;
            foreach (var layer in db.Layers)
            {
                answer[k][0] = layer.Type;
                k++;
            }

            return answer;
        }

        public double[][] GetInputs()
        {
            int amount = db.Layers.Count();

            double[][] result = new double[amount][];

            for (int i = 0; i < amount; i++)
                result[i] = new double[LayerParameters];

            int k = 0;
            foreach (var layer in db.Layers)
            {
                result[k][0] = layer.Porosity;
                result[k][1] = layer.Clayness;
                result[k][2] = layer.Carbonate;
                result[k][3] = layer.Amplitude;
                k++;
            }

            return result;
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