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

        public KerogenAnswerService(IUnitOfWork dataBase)
        {
            db = dataBase;
        }

        public double[][] GetInputs()
        {
            var kerogenRepository = (KerogenRepository)db.Kerogens;
            return kerogenRepository.GetInputs();
        }
    }
}