using PhoneFileTransfer.Services.FileCopier;
using PhoneFileTransfer.Services.FileCopyAndRemove;
using PhoneFileTransfer.Services.FileRemover;
using PhoneFileTransfer.Utilities.Remover.RemoverMobile;

namespace PhoneFileTransfer.Factories
{
    public interface IFactory
    {
        IFileCopierService CreateFileCopierService(bool useAdb);
        MobileFileDialog CreateMobileFileDialog(bool useAdb);
        FileCopyAndRemoverService CreateFileCopyAndRemover(IFileCopierService fileCopier, IFileRemoverService fileRemover);
        IRemoverMobileUtil CreateRemoverMobileUtil(bool useAdb);
        IFileRemoverService CreateFileRemoverService(bool useAdb);
    }
}