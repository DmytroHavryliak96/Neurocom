using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;

namespace Neurocom.BL.Services.ControllerServices
{
    public class LayerAnswerService : IAnswerService
    {
        private IUnitOfWork db;

        public LayerAnswerService(IUnitOfWork dataBase)
        {
            db = dataBase;
        }

        public double[][] GetAnswers()
        {
            var layerRepository = (LayerRepository)db.Layers;
            return layerRepository.GetAnswers();
        }

        public double[][] GetInputs()
        {
            var layerRepository = (LayerRepository)db.Layers;
            return layerRepository.GetInputs();
        }


    }
}