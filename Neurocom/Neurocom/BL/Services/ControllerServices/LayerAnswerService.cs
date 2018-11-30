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

        public void DeleteData(TaskViewModel model)
        {
            db.Layers.Delete(model.Id);
            db.Save();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<TaskViewModel> GetAllData()
        {
            var layers = db.Layers.GetAll();
            List<LayerViewModel> list = new List<LayerViewModel>();
            foreach (var item in layers)
            {
                list.Add(new LayerViewModel
                {
                    Amplitude = item.Amplitude,
                    Carbonate = item.Carbonate,
                    Clayness = item.Clayness,
                    Porosity = item.Porosity,
                    Type = item.Type,
                    Id = item.Id,
                    TaskName = "Layers"
            });

            }

            return list;
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

        public TaskViewModel GetData(TaskViewModel item)
        {
            var layerModel = (LayerViewModel)item;
            var layer = db.Layers.Get(item.Id);

            layerModel.Amplitude = layer.Amplitude;
            layerModel.Carbonate = layer.Amplitude;
            layerModel.Clayness = layer.Clayness;
            layerModel.Porosity = layer.Porosity;
            layerModel.Type = layer.Type;
            layerModel.TaskName = "Layers";
        
            return layerModel;
        }

        public double[][] GetInputs()
        {
            var layerRepository = (LayerRepository)db.Layers;
            return layerRepository.GetInputs();
        }

        public int GetNumOfClusters()
        {
            var rep = (LayerRepository)db.Layers;
            return rep.GetNumOfClusters();
        }

        public int GetParameters()
        {
            var rep = (LayerRepository)db.Layers;
            return rep.GetLayerParameters();
        }

        public void UpdateTask(TaskViewModel model)
        {
            var model1 = (LayerViewModel)model;

            Layer layer = new Layer
            {
                Amplitude = model1.Amplitude,
                Carbonate = model1.Carbonate,
                Clayness = model1.Clayness,
                Porosity = model1.Porosity,
                Type = model1.Type,
                Id = model1.Id
            };

            db.Layers.Update(layer);
            db.Save();
        }
    }
}