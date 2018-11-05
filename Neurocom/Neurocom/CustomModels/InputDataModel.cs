using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neurocom.CustomModels
{
    public class InputDataModel
    {
        [HiddenInput(DisplayValue = false)]
        public string taskName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int trainedNetworkId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string answer { get; set; }
    }
}