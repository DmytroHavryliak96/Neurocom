using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.Models
{
    public class NeuralNetwork
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int NetworkTypeId { get; set; }

        public NetworkType NetworkType { get; set; }
    }
}