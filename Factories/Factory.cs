using Microsoft.Extensions.DependencyInjection;
using PhoneFileTransfer.Services.FileCopier;
using PhoneFileTransfer.Services.Persistence;
using PhoneFileTransfer.Services.MobileBrowser;
using PhoneFileTransfer.Utilities.Copier.CopierFileSystem;
using PhoneFileTransfer.Utilities.Copier.CopierMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneFileTransfer;
using PhoneFileTransfer.Services.FileCopyAndRemove;
using PhoneFileTransfer.Services.FileRemover;
using PhoneFileTransfer.Utilities.Remover.RemoverMobile;
using PhoneFileTransfer.Utilities.Remover.RemoverFileSystem;

namespace PhoneFileTransfer.Factories
{
    public class Factory : IFactory
    {
        private readonly IPersistenceStoreService _persistenceStore;
        private readonly ICopierFileSystemUtil _copierFileSystem;
        private readonly IRemoverFileSystemUtil _removerFileSystem;
        private readonly IServiceProvider _serviceProvider;
 

        public Factory(IPersistenceStoreService persistenceStore, ICopierFileSystemUtil copierFileSystem, IServiceProvider serviceProvider, IRemoverFileSystemUtil removerFileSystem)
        {
            this._persistenceStore = persistenceStore;
            this._copierFileSystem = copierFileSystem;
            this._serviceProvider = serviceProvider;
            this._removerFileSystem = removerFileSystem;
        }

        public MobileFileDialog CreateMobileFileDialog(bool useAdb)
        {
            if (useAdb)
            {
                return new MobileFileDialog(_serviceProvider.GetRequiredService<AdbBrowserService>());
            }
            else
            {
                return new MobileFileDialog(_serviceProvider.GetRequiredService<MtpBrowserService>());
            }
        }


        public IFileCopierService CreateFileCopierService(bool useAdb)
        {
            if (useAdb)
            {
                return new FileCopierService(_persistenceStore, _copierFileSystem, _serviceProvider.GetRequiredService<CopierAdbUtil>());
            }
            else
            {
                return new FileCopierService(_persistenceStore, _copierFileSystem, _serviceProvider.GetRequiredService<CopierMtpUtil>());
            }
        }

        public FileCopyAndRemoverService CreateFileCopyAndRemover(IFileCopierService fileCopier, IFileRemoverService fileRemover)
        {
            return new FileCopyAndRemoverService(fileCopier,fileRemover);
        }

        public IFileRemoverService CreateFileRemoverService(bool useAdb)
        {
            return new FileRemoverService(_persistenceStore, CreateRemoverMobileUtil(useAdb), _removerFileSystem);
        }

        public IRemoverMobileUtil CreateRemoverMobileUtil(bool useAdb)
        {
            if (useAdb)
                return _serviceProvider.GetRequiredService<RemoverAdbUtil>();
            else
                return _serviceProvider.GetRequiredService<RemoverMtpUtil>();
        }
    }
}
