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

            var values = (ValueProviderCollection)bindingContext.ValueProvider;
            var networkName_ = (string)values.GetValue("networkName").ConvertTo(typeof(string));

            ModelBindingContext newbindingContext = new ModelBindingContext()
            {

            };

            switch (networkName_)
            {
                case "BPN":
                    {
                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                             () => new BPNInitializer(), typeof(BPNInitializer));

                        break;
                    }
                case "LVQ":
                    {

                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                           () => new LVQInitializer(), typeof(LVQInitializer));
                        break;
                    };
                default:
                    break;

                   

            }
            newbindingContext.ModelState = bindingContext.ModelState;
            newbindingContext.ValueProvider = bindingContext.ValueProvider;

            return base.BindModel(controllerContext, newbindingContext);

        }

                
    }
    
}