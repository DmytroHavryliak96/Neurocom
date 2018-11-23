using Neurocom.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.CustomModels
{
    public class NetworkBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        
            NetworkInitializer  item = null;
            var values = (ValueProviderCollection)bindingContext.ValueProvider;
            var taskName_ = (string)values.GetValue("taskName").ConvertTo(typeof(string));
            var networkName_ = (string)values.GetValue("networkName").ConvertTo(typeof(string));
            var minError_ = (double)values.GetValue("minError").ConvertTo(typeof(double));
            var learningRate_ = (double)values.GetValue("learningRate").ConvertTo(typeof(double));

           // var trainedNetId = (int)values.GetValue("trainedNetworkId").ConvertTo(typeof(int));
                switch (networkName_)
                {
                    case "BPN":
                        {
                            var momentum_ = (double)values.GetValue("Momentum").ConvertTo(typeof(double));
                            var parameters_ = (int)values.GetValue("parameters").ConvertTo(typeof(int));
                            var hidden_ = (int)values.GetValue("hidden").ConvertTo(typeof(int));

                        item = new BPNInitializer
                        {
                            taskName = taskName_,
                            networkName = networkName_,
                            minError = minError_,
                            learningRate = learningRate_,
                            Momentum = momentum_,
                            parameters = parameters_,
                            hidden = hidden_
                        };
                        break;
                    };

                default:
                    break;
                }

            return item;
        }
           
    }
    
}