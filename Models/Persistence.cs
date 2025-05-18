using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Models
{
    public class Persistence
    {
        public List<Job> JobList { get; set; }
        public string LastSourcePath { get; set; }
        public string LastDestinationPath { get; set; }
        public bool MobileMode { get; set; }
        public bool AdbMode { get; set; }
        public string DeviceName { get; set; }

    }
}
