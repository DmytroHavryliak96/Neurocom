using Neurocom.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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
}