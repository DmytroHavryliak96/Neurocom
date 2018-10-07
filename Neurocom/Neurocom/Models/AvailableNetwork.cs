using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neurocom.Models
{
    public class AvailableNetwork
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int NeuralNetworkId { get; set; }

        public NeuralNetwork NeuralNetwork { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TaskId { get; set; }

        public TaskNetwork Task { get; set; }
    }
}