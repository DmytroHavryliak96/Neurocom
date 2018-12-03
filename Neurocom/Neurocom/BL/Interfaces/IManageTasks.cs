using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.Models;
using System.Web;
using Neurocom.CustomModels;

namespace Neurocom.BL.Interfaces
{
    public interface IManageTasks
    {
        IEnumerable<TaskNetwork> GetAllTasks();
        TaskNetwork GetTask(int taskId);
        void EditTask(TaskNetwork task, HttpPostedFileBase image);

        IEnumerable<TaskViewModel> GetAllData(string tableName);
        TaskViewModel GetData(TaskViewModel item);
        void UpdateData(TaskViewModel model);
        void DeleteData(TaskViewModel model);

        void Dispose();
    }
}
