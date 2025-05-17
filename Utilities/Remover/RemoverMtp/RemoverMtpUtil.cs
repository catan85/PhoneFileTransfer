using MediaDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Remover.RemoverMtp
{
    internal class RemoverMtpUtil : IRemoverMtpUtil
    {
        public void Remove(string deviceDescription, string path)
        {
            try
            {
                var device = MediaDevice.GetDevices().FirstOrDefault(d => d.Description == deviceDescription);
                device.Connect(MediaDeviceAccess.GenericWrite, MediaDeviceShare.Write, enableCache: false);
                device.DeleteFile(path);
                
                System.Threading.Thread.Sleep(20);
            } 
            catch (Exception ex) { 
                MessageBox.Show(ex.Message +"\n"+ ex.StackTrace);
            
            }

        }
    }
}