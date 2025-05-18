using MediaDevices;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Remover.RemoverMobile
{
    internal class RemoverAdbUtil : IRemoverMobileUtil
    {
        public void Remove(string deviceSerial, string path)
        {
            var _client = new AdbClient();

            var device = _client.GetDevices().FirstOrDefault(d => d.Serial == deviceSerial);
            if (device == null)
                throw new Exception($"Device with serial '{deviceSerial}' not found.");

            try
            {
                string command = $"rm \"{path}\"";

                var receiver = new ConsoleOutputReceiver();
                _client.ExecuteRemoteCommand(command, device, receiver);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }


        }
    }
}