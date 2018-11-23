using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.CustomModels
{
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            InputDataModel item = null;
            var values = (ValueProviderCollection)bindingContext.ValueProvider;
            var taskName_ = (string)values.GetValue("taskName").ConvertTo(typeof(string));
            var trainedNetId = (int)values.GetValue("trainedNetworkId").ConvertTo(typeof(int));
            switch (taskName_)
            {
                case "Kerogen":
                    {
                        var carbon = (double)values.GetValue("Carbon").ConvertTo(typeof(double));
                        var hydrogen = (double)values.GetValue("Hydrogen").ConvertTo(typeof(double));
                        var oxygen = (double)values.GetValue("Oxygen").ConvertTo(typeof(double));
                        var nitrogen = (double)values.GetValue("Nitrogen").ConvertTo(typeof(double));
                        var sulfur = (double)values.GetValue("Sulfur").ConvertTo(typeof(double));
                        item = new KerogenInput
                        {
                            Carbon = carbon,
                            Hydrogen = hydrogen,
                            Oxygen = oxygen,
                            Nitrogen = nitrogen,
                            Sulfur = sulfur,
                            trainedNetworkId = trainedNetId,
                            taskName = taskName_
                        };

                        break;
                    }
                case "Layer":
                    {
                        var porosity = (double)values.GetValue("Porosity").ConvertTo(typeof(double));
                        var clayness = (double)values.GetValue("Clayness").ConvertTo(typeof(double));
                        var carbonate = (double)values.GetValue("Carbonate").ConvertTo(typeof(double));
                        var amplitude = (double)values.GetValue("Amplitude").ConvertTo(typeof(double));
                        
                        item = new LayerInput
                        {
                            Porosity = porosity,
                            Clayness = clayness,
                            Carbonate =carbonate,
                            Amplitude = amplitude,
                        
                            trainedNetworkId = trainedNetId,
                            taskName = taskName_
                        };

                        break;
                    }
                default:
                    break;
            }
            return item;
        }
    }
}