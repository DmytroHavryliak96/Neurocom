using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.CustomModels;

namespace Neurocom.BL.Services.ControllerServices
{
    public class ConverterService : IInputConverter
    {
        private const int kerogenParameters = 5;
        private const int layerParameters = 4;
        private Func<InputDataModel, double[]> inputResolver;

        public ConverterService(Func<InputDataModel, double[]> res)
        {
            inputResolver = res;
        }

        public double[] ConvertVector(InputDataModel model)
        {
            return inputResolver(model);
        }
    }
}