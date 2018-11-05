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
        private Dictionary<int, string> layerAnswers;

        public LayerAnswerService(IUnitOfWork dataBase)
        {
            db = dataBase;
            layerAnswers = new Dictionary<int, string>()
            {
                {1, "Колектор" },
                {2, "Покришка" }
            };
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public string GetAnswer(int answer)
        {
            return "Даний об'єкт розпізнаний мережею як " + layerAnswers[answer];
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