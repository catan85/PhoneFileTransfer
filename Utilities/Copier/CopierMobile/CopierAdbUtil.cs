using PhoneFileTransfer.Utilities.Copier.CopierMtp;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Copier.CopierMobile
{
    public class CopierAdbUtil : ICopierMobileUtil
    {
        private AdbClient _adbClient;

        public CopierAdbUtil()
        {
            _adbClient = new AdbClient();
        }

        public void Copy(string deviceSerial, string source, string destination)
        {
            var dir = Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Console.WriteLine($"Directory created: {dir}");
            }

            var device = _adbClient.GetDevices().FirstOrDefault(d => d.Serial == deviceSerial);
            if (device == null)
                throw new Exception($"Device with serial '{deviceSerial}' not found.");

            using var localStream = File.OpenWrite(destination);

            // Uso di SyncService per copiare il file
            using var sync = new SyncService(_adbClient, device);
            sync.Pull(source, localStream, null, CancellationToken.None);

            Console.WriteLine($"File copied from '{source}' on device '{deviceSerial}' to '{destination}'.");
        }
    }
}
