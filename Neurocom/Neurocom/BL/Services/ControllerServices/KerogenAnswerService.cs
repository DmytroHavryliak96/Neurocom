using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.CustomModels;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;
using Neurocom.Models;

namespace Neurocom.BL.Services.ControllerServices
{
    public class KerogenAnswerService : IAnswerService
    {
        private IUnitOfWork db;
        private Dictionary<int, string> kerogenAnswers;
        private const int num = 3;

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

        public int GetParameters()
        {
            var rep = (KerogenRepository)db.Kerogens;
            return rep.GetKerogenParameters();
        }

        public int GetNumOfClusters()
        {
            var rep = (KerogenRepository)db.Kerogens;
            return rep.GetNumOfClusters();
        }

        public void UpdateTask(TaskViewModel model)
        {
            var model1 = (KerogenViewModel)model;

            Kerogen kerogen = new Kerogen
            {
                Carbon = model1.Carbon,
                Hydrogen = model1.Hydrogen,
                Nitrogen = model1.Nitrogen,
                Oxygen = model1.Oxygen,
                Sulfur = model1.Sulfur,
                Type = model1.Type,
                Id = model1.Id
            };

            db.Kerogens.Update(kerogen);
            db.Save();
        }

        public IEnumerable<TaskViewModel> GetAllData()
        {
            var kerogens = db.Kerogens.GetAll();
            List<KerogenViewModel> list = new List<KerogenViewModel>();
            foreach (var item in kerogens)
            {
                list.Add(new KerogenViewModel
                {
                    Carbon = item.Carbon,
                    Hydrogen = item.Hydrogen,
                    Nitrogen = item.Nitrogen,
                    Oxygen = item.Oxygen,
                    Sulfur = item.Sulfur,
                    Type = item.Type,
                    TaskName = "Kerogens",
                    Id = item.Id
                });

            }

            return list;
        }

        public TaskViewModel GetData(TaskViewModel item)
        {
            var kerogenModel = (KerogenViewModel)item;
            var kerogen = db.Kerogens.Get(item.Id);

            kerogenModel.Carbon = kerogen.Carbon;
            kerogenModel.Hydrogen = kerogen.Hydrogen;
            kerogenModel.Nitrogen = kerogen.Nitrogen;
            kerogenModel.Oxygen = kerogen.Oxygen;
            kerogenModel.Sulfur = kerogen.Sulfur;
            kerogenModel.Type = kerogen.Type;
            kerogenModel.TaskName = "Kerogens";

            return kerogenModel;
        }

        public void DeleteData(TaskViewModel model)
        {
            db.Kerogens.Delete(model.Id);
            db.Save();
        }
    }
}