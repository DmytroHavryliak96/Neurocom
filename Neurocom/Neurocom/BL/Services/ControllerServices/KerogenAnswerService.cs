using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;

namespace Neurocom.BL.Services.ControllerServices
{
    public class KerogenAnswerService : IAnswerService
    {
        private IUnitOfWork db;
        private Dictionary<int, string> kerogenAnswers;

        public KerogenAnswerService(IUnitOfWork dataBase)
        {
            db = dataBase;
            kerogenAnswers = new Dictionary<int, string>() {
                {1, "Ліптиніт" },
                {2, "Екзиніт" },
                {3, "Вітриніт" }
            };
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public string GetAnswer(int answer)
        {
            return "Кероген належить до типу " + kerogenAnswers[answer];
        }

        public double[][] GetAnswers()
        {
            var kerogenRepository = (KerogenRepository)db.Kerogens;
            return kerogenRepository.GetAnswers();
        }

        public double[][] GetInputs()
        {
            var kerogenRepository = (KerogenRepository)db.Kerogens;
            return kerogenRepository.GetInputs();
        }
    }
}