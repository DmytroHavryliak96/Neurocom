using Neurocom.BL.Interfaces;
using Neurocom.BL.Services;
using Neurocom.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Neurocom.Controllers.AdminControllers
{
    public class ManageTestNetworksController : Controller
    {
        private IManageTest testService;

        public ManageTestNetworksController (IManageTest test_)
        {
            testService = test_;
        }

        // GET: ManageTestNetworks
        public ActionResult Index()
        {

            return View(testService.GetAllTestNetworks());
        }

        public ActionResult AddTestNetwork()
        {
            return View(testService.GetAllTasks());
        }

        public ActionResult ChooseNetworkType(int taskId)
        {
           
            return View(testService.GetNetworksForTask(taskId));
        }

        public ActionResult NetworkDataInput(NetworkTaskViewModel model)
        {
            return View(testService.GetNetworkInitializer(model));
        }

        public ActionResult CreateInput(NetworkInitializer input)
        {
            if (ModelState.IsValid)
            {
                testService.TrainNetwork(input, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View("NetworkDataInput", input);
            }
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }

    }
}