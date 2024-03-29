﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.DAO.Interfaces;
using Neurocom.CustomModels;

namespace Neurocom.BL.Interfaces
{
    public interface IAnswerService
    {
        double[][] GetInputs();
        double[][] GetAnswers();

        string GetAnswer(int answer);

        int GetParameters();

        int GetNumOfClusters();

        void UpdateTask(TaskViewModel model);

        IEnumerable<TaskViewModel> GetAllData();

        TaskViewModel GetData(TaskViewModel item);

        void DeleteData(TaskViewModel model);

        void Dispose();


    }
}
