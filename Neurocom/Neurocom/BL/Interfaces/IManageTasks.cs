using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.Models;
using System.Web;

namespace Neurocom.BL.Interfaces
{
    public interface IManageTasks
    {
        IEnumerable<TaskNetwork> GetAllTasks();
        TaskNetwork GetTask(int taskId);
        void EditTask(TaskNetwork task, HttpPostedFileBase image);

        IEnumerable<Kerogen> GetAllKerogens();
        Kerogen GetKerogen(int kerogenId);
        void EditKerogen(Kerogen kerogen);
        void DeleteKerogen(int kerogenId);
        Kerogen CreateKerogen();


        IEnumerable<Layer> GetAllLayers();
        Layer GetLayer(int layerId);
        void EditLayer(Layer layer);
        void DeleteLayer(int layerId);
        Layer CreateLayer();



    }
}
