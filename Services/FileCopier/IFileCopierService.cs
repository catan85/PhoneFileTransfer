using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopier.Models;

namespace PhoneFileTransfer.Services.FileCopier
{
    public interface IFileCopierService
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