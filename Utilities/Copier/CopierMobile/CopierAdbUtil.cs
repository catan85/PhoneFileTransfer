
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Creo la directory della destinazione (utile sia per SyncService che per fallback)
            var dir = System.IO.Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var device = _adbClient.GetDevices().FirstOrDefault(d => d.Serial == deviceSerial);
            if (device == null)
                throw new Exception($"Device with serial '{deviceSerial}' not found.");

            // Normalizza il path per gestire accenti e caratteri speciali nella sorgente sul device
            source = source.Normalize(NormalizationForm.FormC);

            try
            {
                using var localStream = File.Open(destination, FileMode.Create, FileAccess.Write);
                using var sync = new SyncService(_adbClient, device);

                sync.Pull(source, localStream, null, CancellationToken.None);
                Console.WriteLine($"File copied from '{source}' on device '{deviceSerial}' to '{destination}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SyncService pull failed: {ex.Message}");
                // fallback al comando adb esterno
                Console.WriteLine("Falling back to adb.exe pull...");

                // Uso un file temporaneo senza caratteri speciali nel nome
                var tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileName(destination));

                // Assicuro che la directory temp esista (di solito esiste già)
                var tempDir = System.IO.Path.GetDirectoryName(tempFile);
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                var startInfo = new ProcessStartInfo
                {
                    FileName = "./tools/adb/adb.exe",
                    Arguments = $"pull \"{source}\" \"{tempFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                };

                using var process = Process.Start(startInfo);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"adb pull failed: {error}");
                }

                // Ora sposto il file temporaneo nella destinazione finale
                // Assicuro che la directory di destinazione esista (già fatta prima, ma in caso)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.Move(tempFile, destination, overwrite: true);

                Console.WriteLine($"File copied from '{source}' on device '{deviceSerial}' to '{destination}' (via adb.exe fallback).");
            }
        }

    }
}
