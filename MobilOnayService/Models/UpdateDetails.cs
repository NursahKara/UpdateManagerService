using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilOnayService.Models
{
    public class UpdateDetails
    {
        public string PackageName { get; set; }

        public string Version { get; set; }

        public string Url { get; set; }

        public bool UpToDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
