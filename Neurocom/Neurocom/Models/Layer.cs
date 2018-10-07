using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neurocom.Models
{
    public class Layer
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

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