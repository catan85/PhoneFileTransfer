using MediaDevices;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Remover.RemoverMtp
{
    internal class RemoverAdbUtil : IRemoverAdbUtil
    {
        public void Remove(string deviceDescription, string path)
        {
            var _client = new AdbClient();

            var devices = _client.GetDevices();
            if (devices.Count == 0)
                throw new InvalidOperationException("Nessun dispositivo connesso via adb.");
 
            var _device = devices.Where(d=> d.Model == deviceDescription.Replace(" ", "_")).FirstOrDefault();

            try
            {
                string command = $"rm \"{path}\"";

                var receiver = new ConsoleOutputReceiver();
                _client.ExecuteRemoteCommand(command, _device, receiver);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }


        }
    }
}