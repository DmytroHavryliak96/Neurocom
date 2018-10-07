using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neurocom.Models
{
    public class TrainedNetwork
    {
        public int Id { get; set; }

        public string XmlName { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int AvailableNetworkId { get; set; }
        public AvailableNetwork AvailableNetwork { get; set; }
       
        public DateTime CreatedDate { get; set; }
    }
}