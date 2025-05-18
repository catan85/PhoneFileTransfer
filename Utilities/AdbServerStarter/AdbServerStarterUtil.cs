
namespace PhoneFileTransfer.Utilities.AdbServerStarter
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;

    class AdbServerStarterUtil : IAdbServerStarterUtil
    {
        private const string AdbPath = @"Tools\Adb\adb.exe"; // Cambia con il percorso corretto di adb.exe
        private const int AdbPort = 5037;


        public bool IsAdbServerRunning()
        {
            try
            {
                using (var client = new TcpClient("127.0.0.1", AdbPort))
                {
                    return true; // Connessione riuscita, server attivo
                }
            }
            catch (SocketException)
            {
                return false; // Connessione fallita, server non attivo
            }
        }

        public void StartAdbServer()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = AdbPath;
                process.StartInfo.Arguments = "start-server";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("Output adb start-server:\n" + output);
                Console.WriteLine("ADB server started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nell'avvio del server ADB: " + ex.Message);
            }
        }
    }

}
