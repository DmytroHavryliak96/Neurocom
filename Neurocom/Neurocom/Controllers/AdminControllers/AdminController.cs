using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;

namespace Neurocom.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAdminService adminService;

        public AdminController(IAdminService adminService_)
        {
            adminService = adminService_;
        }

        
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageUsers()
        {
            return View(adminService.GetAllUsers());
        }

        [HttpGet]
        public ViewResult EditUser(string userId)
        {
            var user = adminService.GetUser(userId);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(ApplicationUserViewModel user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                adminService.UpdateUser(user, image);
                return RedirectToAction("ManageUsers");
            }
            else
            {
                // there is something wrong with the data values
                return View(user);
            }
        }

        public ViewResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                adminService.CreateUser(model);
                return RedirectToAction("ManageUsers");
            }
            else
            {
                // there is something wrong with the data values
                return View(model);
            }

        }


        [HttpPost]
        public ActionResult DeleteUser(string userId)
        {
            adminService.DeleteUser(userId);
            return RedirectToAction("ManageUsers");
        }

        protected override void Dispose(bool disposing)
        {
            adminService.Dispose();
            base.Dispose(disposing);
        }





    }
}