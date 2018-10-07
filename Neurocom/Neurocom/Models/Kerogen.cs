using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neurocom.Models
{
    public class Kerogen
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

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
}