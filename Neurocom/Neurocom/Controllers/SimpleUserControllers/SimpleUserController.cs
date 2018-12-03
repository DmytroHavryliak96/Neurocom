using Neurocom.BL.Interfaces;
using Neurocom.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.Controllers.SimpleUserControllers
{
    public class SimpleUserController : Controller
    {
        private IUserController controller;

        public SimpleUserController(IUserController _controller)
        {
            controller = _controller;
        }

        public ActionResult Index()
        {
            return View(controller.GetAllTestNetworks());
        }

        public ActionResult SimpleDataInput(int trainedNetworkId)
        {
            return View(controller.CreateDataInput(trainedNetworkId));
        }

        public ActionResult CreateInput(InputDataModel model)
        {
            if (ModelState.IsValid)
            {
                return View("DataInput", controller.CreateAnswerModel(model));
            }
            else
            {
                // there is something wrong with the data values
                return View("DataInput", model);
            }
        }


        public ActionResult TaskList()
        {
            return View(controller.GetAllTasks());
        }
    }
}