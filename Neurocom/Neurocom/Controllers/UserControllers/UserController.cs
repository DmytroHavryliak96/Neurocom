using Neurocom.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Neurocom.Models;
using Neurocom.CustomModels;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.BL.Services;

namespace Neurocom.Controllers.UserControllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserController _contrl;

        public UserController(IUserController controller)
        {
            _contrl = controller;
        }

        public ActionResult Index()
        {
            return View(_contrl.GetAllUserNetworks(User.Identity.GetUserId()));
        }

        public ActionResult DataInput(int trainedNetworkId)
        {
            return View(_contrl.CreateDataInput(trainedNetworkId));
        }

        public ActionResult CreateInput(InputDataModel model)
        {
            if (ModelState.IsValid)
            {
                return View("DataInput", _contrl.CreateAnswerModel(model));
            }
            else
            {
                // there is something wrong with the data values
                return View("DataInput", model);
            }
        }

        public ActionResult TaskList()
        {
            return View(_contrl.GetAllTasks());
        }

        public ActionResult NetworkList()
        {
            return View(_contrl.GetAllNetworks());
        }

        public ViewResult TestNetworks()
        {
            return View(_contrl.GetAllTestNetworks());
        }

        public ViewResult EditProfile()
        {
            return View(_contrl.GetUser(User.Identity.GetUserId()));
        }

        [HttpPost]
        public ActionResult EditProfile(ApplicationUser _user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                _contrl.EditProfile(_user, image);
               return RedirectToAction("Index");
            }
            else
            {
                return View(_user);
            }
        }

        public ActionResult AddNetwork()
        {
            return View(_contrl.GetAllTasks());
        }

        public ActionResult ChooseNetworkType(int taskId)
        {
            return View(_contrl.GetNetworksForTask(taskId));
        }

        public ActionResult NetworkDataInput(NetworkTaskViewModel model)
        {
            return View(_contrl.GetNetworkInitializer(model));
        }

        public ActionResult CreateNet(NetworkInitializer input)
        {
            if (ModelState.IsValid)
            {
                _contrl.TrainNetwork(input, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View("NetworkDataInput", input);
            }
        }
        [HttpPost]
        public ActionResult DeleteNetwork(int trainedNetworkId)
        {
            _contrl.DeleteUserNetwork(trainedNetworkId);
            return RedirectToAction("Index");
        }

    }
}