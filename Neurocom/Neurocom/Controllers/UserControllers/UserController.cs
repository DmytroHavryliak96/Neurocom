using Neurocom.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Neurocom.Models;
using Neurocom.CustomModels;

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
        public ActionResult EditProfile(ApplicationUser _user, HttpPostedFileBase _image)
        {
            if (ModelState.IsValid)
            {
                _contrl.EditProfile(_user,_image);
               return RedirectToAction("Index");
            }
            else
            {
                return View(_user);
            }
        }
    }
}