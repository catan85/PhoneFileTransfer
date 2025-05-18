using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.FileCopier.Models
{
    public class CopierStatusEventArgs
    {
        public string Status { get; set; }
        public int CopiedFiles { get; set; }   
        public int FilesToCopy { get; set; }
    }
}
