using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopierService.Models;

namespace PhoneFileTransfer.Services.FileCopierService
{
    public interface IFileCopier
    {
        public void ExecuteCopy(bool skipJobsDone);
        public void PauseCopy();
        public void ResumeCopy();
        public void CancelCopy();
        public event EventHandler<CopierStatusEventArgs> FileCopying;
        public event EventHandler CopyCompleted;
        public WorkerStatus CopyStatus { get; }
    }
}