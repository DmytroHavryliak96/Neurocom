using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.CustomModels;

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

        [HttpGet]
        public ViewResult EditType(int typeId)
        {
            NetworkType type = manageNetworkService.GetNetworkType(typeId);
            return View(type);
        }


        [HttpPost]
        public ActionResult EditType(NetworkType type)
        {
            if (ModelState.IsValid)
            {

                manageNetworkService.UpdateNetworkType(type);
                return RedirectToAction("ManageTypes");
            }
            else
            {
                // there is something wrong with the data values
                return View(type);
            }
        }

        public ViewResult ManageNeuralNetworks()
        {
            return View(manageNetworkService.GetAllNeuralNetworks());

        }

        [HttpGet]
        public ViewResult EditNetwork(int networkId)
        {
            NeuralNetwork network = manageNetworkService.GetNetwork(networkId);
            return View(network);
        }


        [HttpPost]
        public ActionResult EditNetwork(NeuralNetwork network)
        {
            if (ModelState.IsValid)
            {
                manageNetworkService.UpdateNetwork(network);
                return RedirectToAction("ManageNeuralNetworks");
            }
            else
            {
                // there is something wrong with the data values
                return View(network);
            }
        }

        public ActionResult DataInput(int trainedNetworkId)
        {
            return View(manageNetworkService.CreateInputModel(trainedNetworkId));
        }


        public ActionResult CreateInput(InputDataModel model)
        {
            if (ModelState.IsValid)
            {
                return View("DataInput", manageNetworkService.GetAnswerModel(model));
            }
            else
            {
                // there is something wrong with the data values
                return View("DataInput", model);
            }
        }

    }
}