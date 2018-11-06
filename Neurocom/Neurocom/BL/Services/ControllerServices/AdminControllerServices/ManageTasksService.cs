using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;

namespace Neurocom.BL.Services.ControllerServices.AdminControllerServices
{
    public class ManageTasksService : IManageTasks
    {
        private IUnitOfWork Database { get; set; }

        public ManageTasksService(IUnitOfWork db)
        {
            Database = db;
        }

        public void EditKerogen(Kerogen kerogen)
        {
            Database.Kerogens.Update(kerogen);
            Database.Save();
        }

        public void EditLayer(Layer layer)
        {
            Database.Layers.Update(layer);
            Database.Save();
        }

        public void EditTask(TaskNetwork task, HttpPostedFileBase image)
        {
            if (image != null)
            {
                task.ImageMimeType = image.ContentType;
                task.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(task.ImageData, 0, image.ContentLength);
            }
            Database.TaskNetworks.Update(task);
            Database.Save();
        }

        public IEnumerable<Kerogen> GetAllKerogens()
        {
            return Database.Kerogens.GetAll();
        }

        public IEnumerable<Layer> GetAllLayers()
        {
            return Database.Layers.GetAll();
        }

        public IEnumerable<TaskNetwork> GetAllTasks()
        {
            return Database.TaskNetworks.GetAll();
        }

        public Kerogen GetKerogen(int kerogenId)
        {
            return Database.Kerogens.Get(kerogenId);
        }

        public Layer GetLayer(int layerId)
        {
            return Database.Layers.Get(layerId);
        }

        public TaskNetwork GetTask(int taskId)
        {
            return Database.TaskNetworks.Get(taskId);
        }

        public void DeleteKerogen(int kerogenId)
        {
            Database.Kerogens.Delete(kerogenId);
            Database.Save();
        }

        public Kerogen CreateKerogen()
        {
            return new Kerogen();
        }

        public void DeleteLayer(int layerId)
        {
            Database.Layers.Delete(layerId);
            Database.Save();
        }

        public Layer CreateLayer()
        {
            return new Layer();
        }
    }
}