using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using Neurocom.CustomModels;

namespace Neurocom.BL.Services.ControllerServices.AdminControllerServices
{
    public class ManageTasksService : IManageTasks
    {
        private IUnitOfWork Database { get; set; }
        private Func<string, IUnitOfWork, IAnswerService> answerService;

        public ManageTasksService(IUnitOfWork db, Func<string, IUnitOfWork, IAnswerService> answerService_)
        {
            Database = db;
            answerService = answerService_;
        }

        public IEnumerable<TaskNetwork> GetAllTasks()
        {
            return Database.TaskNetworks.GetAll();
        }

        public TaskNetwork GetTask(int taskId)
        {
            return Database.TaskNetworks.Get(taskId);
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

        public IEnumerable<TaskViewModel> GetAllData(string tablename)
        {
            IAnswerService service = answerService(tablename, Database);
            return service.GetAllData();
        }

        public TaskViewModel GetData(TaskViewModel item)
        {
            IAnswerService service = answerService(item.TaskName, Database);
            return service.GetData(item);
        }

        public void UpdateData(TaskViewModel model)
        {
            IAnswerService service = answerService(model.TaskName, Database);
            service.UpdateTask(model);
        }

        public void DeleteData(TaskViewModel model)
        {
            IAnswerService service = answerService(model.TaskName, Database);
            service.DeleteData(model);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}