using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.ViewModels.AdminViewModels
{
    public class NetworkViewModel
    {
        public int TestNetworkId { get; set; }

        public int TrainedNetworkID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int AvailableNetworkId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int NetworkId { get; set; }

        public string NetworkName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int NetworkTypeId { get; set; }

        public string NetworkType { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string TypeDescription { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string NetworkDescription { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}