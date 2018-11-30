using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.CustomModels
{
    public class TaskModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            var values = (ValueProviderCollection)bindingContext.ValueProvider;
            var taskName = (string)values.GetValue("TaskName").ConvertTo(typeof(string));
          //  string taskName = "Kerogens";

            ModelBindingContext newbindingContext = new ModelBindingContext()
            {

            };

            switch (taskName)
            {
                case "Kerogens":
                    {
                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                             () => new KerogenViewModel(), typeof(KerogenViewModel));

                        break;
                    }
                case "Layers":
                    {

                        newbindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                           () => new LayerViewModel(), typeof(LayerViewModel));
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