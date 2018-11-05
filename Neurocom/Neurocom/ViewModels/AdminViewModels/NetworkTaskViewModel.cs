using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neurocom.ViewModels.AdminViewModels
{
    public class NetworkTaskViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TaskName { get; set; }

        public string Description { get; set; }

        public string NetworkTypeProperty { get; set; }

        public int networkId { get; set; }

        public int taskId { get; set; }
    }
}