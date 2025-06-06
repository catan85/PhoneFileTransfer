﻿using MediaDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Copier.CopierMobile
{
    public class CopierMtpUtil : ICopierMobileUtil
    {

        public void Copy(string deviceDescription, string source, string destination)
        {
            var dir = System.IO.Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Console.WriteLine($"Directory created: {dir}");
            }

            var device = MediaDevice.GetDevices().FirstOrDefault(d => d.Description == deviceDescription);
            device.Connect(enableCache: false);
            device.DownloadFile(source, destination);
        }


    }
}
