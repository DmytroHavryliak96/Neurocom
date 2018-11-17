using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neurocom.BL.Interfaces;

namespace Neurocom.Controllers.AdminControllers
{
    public class AdminController : Controller
    {
        private IAdminService adminService;

        public AdminController(IAdminService adminService_)
        {
            adminService = adminService_;
        }

        
        public ActionResult Index()
        {
            return View(adminService.GetAllUsers());
        }
    }
}