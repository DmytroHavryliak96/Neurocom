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
            var values = (ValueProviderCollection)bindingContext.ValueProvider;
            var taskName_ = (string)values.GetValue("taskName").ConvertTo(typeof(string));

            ModelBindingContext newbindingContext = new ModelBindingContext()
            {

            };

            switch (taskName_)
            {
                case "Kerogen":
                    {
                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                             () => new KerogenInput(), typeof(KerogenInput));

                        break;
                    }
                case "Layer":
                    {
                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                             () => new LayerInput(), typeof(LayerInput));

                        break;
                    }
                default:
                    break;
            }

            newbindingContext.ModelState = bindingContext.ModelState;
            newbindingContext.ValueProvider = bindingContext.ValueProvider;

            return base.BindModel(controllerContext, newbindingContext);
        }
    }
}