using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.CustomModels
{
    public abstract class TaskViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string TaskName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

    }

    public class KerogenViewModel : TaskViewModel
    {
        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Carbon { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Hydrogen { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Oxygen { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Nitrogen { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Sulfur { get; set; }

        [Range(1, 3,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Type { get; set; }
    }

    public class LayerViewModel : TaskViewModel
    {
        [Range(0.0, 1.0,
     ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Porosity { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Clayness { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Carbonate { get; set; }

        [Range(0.0, 1.0,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Amplitude { get; set; }

        [Range(1, 2,
    ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Type { get; set; }
    }
}