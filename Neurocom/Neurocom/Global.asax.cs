using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Neurocom.Util;
using Ninject.Web.Mvc;
using Neurocom.CustomModels;
using Neurocom.BL.Services;

namespace Neurocom
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(InputDataModel), new CustomModelBinder()));
            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(NetworkInitializer), new NetworkBinder()));
            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(TaskViewModel), new TaskModelBinder()));

            NinjectModule neurocomModule = new NeurocomModule("DefaultConnection");
            var kernel = new StandardKernel(neurocomModule);
            kernel.Unbind<ModelValidatorProvider>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

        }
    }
}
