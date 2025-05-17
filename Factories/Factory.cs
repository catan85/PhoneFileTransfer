using Microsoft.Extensions.DependencyInjection;
using PhoneFileTransfer.Services.FileCopierService;
using PhoneFileTransfer.Services.JobStoreService;
using PhoneFileTransfer.Services.MobileBrowserService;
using PhoneFileTransfer.Utilities.Copier.CopierFileSystem;
using PhoneFileTransfer.Utilities.Copier.CopierMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneFileTransfer;
using PhoneFileTransfer.Services.FIleCopyAndRemoveService;
using PhoneFileTransfer.Services.FileRemoverService;

namespace PhoneFileTransfer.Factories
{
    public class Factory : IFactory
    {
        private readonly IPersistenceStore _persistenceStore;
        private readonly ICopierFileSystemUtil _copierFileSystem;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileRemover _fileRemover;

        public Factory(IPersistenceStore persistenceStore, ICopierFileSystemUtil copierFileSystem, IServiceProvider serviceProvider, IFileRemover fileRemover)
        {
            this._persistenceStore = persistenceStore;
            this._copierFileSystem = copierFileSystem;
            this._serviceProvider = serviceProvider;
            this._fileRemover = fileRemover;
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


        public FileCopier CreateFileCopier(bool useAdb)
        {
            if (useAdb)
            {
                return new FileCopier(_persistenceStore, _copierFileSystem, _serviceProvider.GetRequiredService<CopierAdbUtil>());
            }
            else
            {
                return new FileCopier(_persistenceStore, _copierFileSystem, _serviceProvider.GetRequiredService<CopierMtpUtil>());
            }
        }

        public FileCopyAndRemover CreateFileCopyAndRemover(bool useAdb)
        {
            return new FileCopyAndRemover(CreateFileCopier(useAdb), _fileRemover);
        }

    }
}
