namespace PhoneFileTransfer.Services.FileCopyAndRemove
{
    public interface IFileCopyAndRemoverService
    {
        void Execute(bool skipAlreadyDone);
    }
}