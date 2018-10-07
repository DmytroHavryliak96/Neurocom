using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neurocom.Models
{
    public class TestNetwork
    {
        public int Id { get; set; }

        public int TrainedNetworkId { get; set; }
        public TrainedNetwork TrainedNetwork { get; set; }
    }
}