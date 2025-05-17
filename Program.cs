using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopierService;
using PhoneFileTransfer.Services.FIleCopyAndRemoveService;
using PhoneFileTransfer.Services.FileRemoverService;
using PhoneFileTransfer.Services.JobStoreService;
using PhoneFileTransfer.Services.MediaDeviceWorker;
using PhoneFileTransfer.Services.MtpBrowserService;
using PhoneFileTransfer.Utilities.Copier;
using PhoneFileTransfer.Utilities.Copier.CopierFileSystem;
using PhoneFileTransfer.Utilities.Copier.CopierMtp;
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
                    services.AddSingleton<IMediaDeviceWorker,MediaDeviceWorker>();

                    services.AddSingleton<IFileCopier, FileCopier>();
                    services.AddSingleton<IFileRemover, FileRemover>();
                    services.AddSingleton<IFileCopyAndRemover, FileCopyAndRemover>();
                    services.AddSingleton<IPersistenceStore, PersistenceStore>();
                    services.AddSingleton<IMtpBrowserService, MtpBrowserService>();
                 
                    services.AddSingleton<MtpFileDialog>();
                    services.AddSingleton<MainForm>();
                    services.AddTransient<ICopierFileSystemUtil, CopierFileSystemUtil>();
                    services.AddTransient<ICopierMtpUtil, CopierMtpUtil>();
                    services.AddTransient<IRemoverFileSystemUtil, RemoverFileSystemUtil>();
                    services.AddTransient<IRemoverMtpUtil, RemoverMtpUtil>();
                
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