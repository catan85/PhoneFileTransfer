using PhoneFileTransfer.Services.FileCopierService;
using PhoneFileTransfer.Services.FIleCopyAndRemoveService;

namespace PhoneFileTransfer.Factories
{
    public interface IFactory
    {
        FileCopier CreateFileCopier(bool useAdb);
        MobileFileDialog CreateMobileFileDialog(bool useAdb);
        FileCopyAndRemover CreateFileCopyAndRemover(bool useAdb);
    }
}