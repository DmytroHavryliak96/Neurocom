using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neurocom.BL.Interfaces;
using Neurocom.Models;

namespace Neurocom.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    public class ManageNetworksController : Controller
    {
        private IManageNetwork manageNetworkService;

        public ManageNetworksController(IManageNetwork service)
        {
            manageNetworkService = service;
        }

        // GET: ManageNetworks
        public ActionResult Index()
        {
            return View(manageNetworkService.GetAllTrainedNetworks());
        }

        public ViewResult Filter()
        {
            return View(manageNetworkService.GetAllTrainedNetworks());
        }

        public ViewResult ManageTypes()
        {
            return View(manageNetworkService.GetAllTypes());
        }



    }
}