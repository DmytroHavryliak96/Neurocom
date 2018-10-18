using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using System.Reflection;

namespace Neurocom.DAO.Repositories
{
    public class KerogenRepository : IRepository<Kerogen>, IDataInput
    {
        private ApplicationDbContext db; // контекст бази даних
        private int kerogenParameters; // кількість параметрів керогену

        public int GetKerogenParameters() // отримати кількість параметрів керогену
        {
            return this.kerogenParameters;
        }

        public int GetNumberOfKerogenParameters() // отримання к-сті параметрів через рефлексію
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Kerogen).GetProperties();
            return propertyInfos.Length - 2;
        }

        public KerogenRepository(ApplicationDbContext context) // конструктор репозиторію
        {
            this.db = context;
            this.kerogenParameters = GetNumberOfKerogenParameters();
        }

        public void Create(Kerogen item) // додавання елементу 
        {
            db.Kerogens.Add(item);
        }

        public void Delete(int id) // видалення елементу
        {
            Kerogen kerogen = db.Kerogens.Find(id);

            if(kerogen != null)
            {
                db.Kerogens.Remove(kerogen);
            }
        }

        public IEnumerable<Kerogen> Find(Func<Kerogen, bool> predicate) // пошук елементів по предикату
        {
            return db.Kerogens.Where(predicate).ToList();
        }

        public Kerogen Get(int id) // отримати кероген по id
        {
            return db.Kerogens.Find(id);
        }

        public IEnumerable<Kerogen> GetAll() // отримати всі керогени
        {
            return db.Kerogens;
        }

        public void Update(Kerogen entity) // оновити дані про Кероген
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

        public double[][] GetAnswers()
        {
            int amount = db.Kerogens.Count();

            double[][] answer = new double[amount][];

            for (int i = 0; i < amount; i++)
                answer[i] = new double[1];

            int k = 0;
            foreach (var kerogen in db.Kerogens)
            {
                answer[k][0] = kerogen.Type;
                k++;
            }

            return answer;
        }

        public double[][] GetInputs()
        {
            int amount = db.Kerogens.Count();

            double[][] result = new double[amount][];

            for (int i = 0; i < amount; i++)
                result[i] = new double[kerogenParameters];

            int k = 0;
            foreach (var kerogen in db.Kerogens)
            {
                result[k][0] = kerogen.Carbon;
                result[k][1] = kerogen.Hydrogen;
                result[k][2] = kerogen.Oxygen;
                result[k][3] = kerogen.Nitrogen;
                result[k][4] = kerogen.Sulfur;
                k++;
            }

            return result;
        }

       
    }
}