using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.DAO.Repositories
{
    public class TaskNetworkRepository : IRepository<TaskNetwork>
    {
        private ApplicationDbContext db;

        public TaskNetworkRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(TaskNetwork item)
        {
            db.Tasks.Add(item);
        }


        public void Delete(int id)
        {
            var task = db.Tasks.Find(id);

            if (task != null)
            {
                db.Tasks.Remove(task);
            }
        }

        public IEnumerable<TaskNetwork> Find(Func<TaskNetwork, bool> predicate)
        {
            return db.Tasks.Where(predicate);
        }

        public TaskNetwork Get(int id)
        {
            return db.Tasks.Find(id);
        }

        public IEnumerable<TaskNetwork> GetAll()
        {
            return db.Tasks;
        }

        public void Update(TaskNetwork item)
        {
            TaskNetwork dbEntry = db.Tasks.Find(item.Id);
            if (dbEntry != null)
            {
                dbEntry.Description = item.Description;
                dbEntry.ImageData = item.ImageData;
                dbEntry.ImageMimeType = item.ImageMimeType;
            }
        }
    }
}