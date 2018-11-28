using Neurocom.BL.Interfaces;
using Neurocom.CustomModels;
using Neurocom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.Controllers.AdminControllers
{
    public class ManageTasksController : Controller
    {
        private IManageTasks taskService;

        public ManageTasksController (IManageTasks service)
        {
            taskService = service;
        }
        // GET: ManageTask
        public ActionResult Index()
        {
            return View(taskService.GetAllTasks());
        }

        public ViewResult EditTask(int taskId)
        {
            return View(taskService.GetTask(taskId));
        }

        [HttpPost]
        public ActionResult EditTask(TaskNetwork task, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                taskService.EditTask(task, image);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(task);
            }
        }

        public ViewResult ManageData(string tablename)
        {
            ViewBag.tableName = tablename;
            return View(taskService.GetAllData(tablename));
        }

        public ViewResult GetData(TaskViewModel model)
        {
            return View(taskService.GetData(model));
        }

        [HttpPost]
        public ActionResult EditData(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                taskService.UpdateData(model);
                return RedirectToAction("ManageData", model.TaskName);
            }
            else
            {
                return View("GetData", model);
            }
        }

        public ViewResult CreateData(TaskViewModel model)
        {
            return View("GetData", model);
        }

        [HttpPost]
        public ActionResult DeleteData(TaskViewModel model)
        {
            taskService.DeleteData(model);
            return RedirectToAction("ManageData", model.TaskName);
        }




    }
}