using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneFileTransfer.Factories;
using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopierService;
using PhoneFileTransfer.Services.FIleCopyAndRemoveService;
using PhoneFileTransfer.Services.FileRemoverService;
using PhoneFileTransfer.Services.JobStoreService;
using PhoneFileTransfer.Services.MobileBrowserService;
using PhoneFileTransfer.Utilities.AdbServerStarter;
using PhoneFileTransfer.Utilities.Copier;
using PhoneFileTransfer.Utilities.Copier.CopierFileSystem;
using PhoneFileTransfer.Utilities.Copier.CopierMobile;
using PhoneFileTransfer.Utilities.Remover.RemoverFileSystem;
using PhoneFileTransfer.Utilities.Remover.RemoverMtp;

namespace PhoneFileTransfer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {

                    services.AddSingleton<IFileRemover, FileRemover>();
                  
                    services.AddSingleton<IPersistenceStore, PersistenceStore>();

                    services.AddTransient<MtpBrowserService>();
                    services.AddTransient<AdbBrowserService>();
                  
                    services.AddSingleton<MainForm>();
                    services.AddTransient<IFactory, Factory>();
                    services.AddTransient<ICopierFileSystemUtil, CopierFileSystemUtil>();

                    services.AddTransient<CopierMtpUtil>();
                    services.AddTransient<CopierAdbUtil>();
                    
                    services.AddTransient<IRemoverFileSystemUtil, RemoverFileSystemUtil>();
                    services.AddTransient<IRemoverAdbUtil, RemoverAdbUtil>();
                    services.AddSingleton<IAdbServerStarter, AdbServerStarter>();
                
                })
                .Build();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Risolvi e avvia il form principale
            var mainForm = host.Services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
    }
}