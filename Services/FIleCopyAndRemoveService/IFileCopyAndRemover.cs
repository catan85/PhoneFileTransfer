namespace PhoneFileTransfer.Services.FIleCopyAndRemoveService
{
    public interface IFileCopyAndRemover
    {
        void Execute(bool skipAlreadyDone);
    }
}