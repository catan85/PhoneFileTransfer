using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopier;
using PhoneFileTransfer.Services.FileRemover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.FileCopyAndRemove
{
    public class FileCopyAndRemoverService : IFileCopyAndRemoverService
    {
        private readonly IFileCopierService fileCopier;
        private readonly IFileRemoverService fileRemover;
        private int operation = 0;
        private bool _skipAlreadyDone = false;

        public FileCopyAndRemoverService(IFileCopierService fileCopier, IFileRemoverService fileRemover)
        {
            this.fileCopier = fileCopier;
            this.fileRemover = fileRemover;
            this.fileCopier.CopyCompleted += this.FileCopier_CopyCompleted;
            this.fileRemover.RemoveCompleted += this.FileRemover_RemoveCompleted;
        }

        public void Execute(bool skipAlreadyDone)
        {
            if (operation == 0)
            {
                _skipAlreadyDone = skipAlreadyDone;
                fileCopier.ExecuteCopy(_skipAlreadyDone);
                operation++;
            }
        }

        private void FileCopier_CopyCompleted(object? sender, EventArgs e)
        {
            if (operation == 1)
            {
                fileRemover.ExecuteRemove(_skipAlreadyDone);
                operation++;
            }
        }

        private void FileRemover_RemoveCompleted(object? sender, EventArgs e)
        {
            if (operation == 2)
            {
                // reset automa
                _skipAlreadyDone = false;
                operation = 0;
            }
        }

    }
}
