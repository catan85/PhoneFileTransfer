﻿using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.FileCopier.Models;
using PhoneFileTransfer.Services.FileRemover;
using PhoneFileTransfer.Services.Persistence;
using PhoneFileTransfer.Utilities.Copier;
using PhoneFileTransfer.Utilities.Copier.CopierFileSystem;
using PhoneFileTransfer.Utilities.Copier.CopierMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.FileCopier
{
    public class FileCopierService : IFileCopierService
    {

        private readonly IPersistenceStoreService _persistenceStore;
        private readonly ICopierFileSystemUtil copierFileSystem;
        private readonly ICopierMobileUtil copierMobile;
        private CancellationTokenSource _CancellationTokenSource;
        private ManualResetEventSlim _PauseEvent;
        private Task _Task;
        public event EventHandler<CopierStatusEventArgs> FileCopying;
        public event EventHandler CopyCompleted;

        public WorkerStatus CopyStatus { get; private set; }

        private int copiedFiles;
        private int filesToCopy;

        public FileCopierService(IPersistenceStoreService persistenceStore, ICopierFileSystemUtil copierFileSystem, ICopierMobileUtil copierMobileUtil)
        {
            CopyStatus = WorkerStatus.Idle;
            _persistenceStore = persistenceStore;
            this.copierFileSystem = copierFileSystem;
            this.copierMobile = copierMobileUtil;
            _PauseEvent = new ManualResetEventSlim(true); // Inizialmente non in pausa
        }

        public void ExecuteCopy(bool skipJobsDone)
        {
            CopyStatus = WorkerStatus.Running;
            _CancellationTokenSource = new CancellationTokenSource();
            _Task = Task.Run(() => CopyInBackGround(skipJobsDone, _CancellationTokenSource.Token));
        }

        public void PauseCopy()
        {
            CopyStatus = WorkerStatus.Paused;
            _PauseEvent.Reset();
            FileCopying?.Invoke(this, CreateCopierStatusEventArgs("Copy paused"));
        }

        public void ResumeCopy()
        {
            CopyStatus = WorkerStatus.Running;
            _PauseEvent.Set();
            FileCopying?.Invoke(this, CreateCopierStatusEventArgs("Copy Resumed"));
        }

        public void CancelCopy()
        {
            if (_CancellationTokenSource != null)
            {
                _CancellationTokenSource.Cancel();
            }
        }

        private void CopyInBackGround(bool skipJobsDone, CancellationToken token)
        {
            var jobs = _persistenceStore.Get().JobList;

            if (skipJobsDone)
            {
                //filter done jobs
                jobs = jobs.Where(j => j.CopyDone == false).ToList();
            }
            else
            {
                // reset done flags
                jobs.ForEach(j => j.CopyDone = false);
            }

           
            filesToCopy = jobs.Count();
            copiedFiles = 0;

            foreach (var job in jobs)
            {
                
                // Verifica se il processo deve essere interrotto
                if (token.IsCancellationRequested)
                {
                    FileCopying?.Invoke(this, CreateCopierStatusEventArgs("Copy cancelled"));
                    CopyStatus = WorkerStatus.Idle;
                    return;
                }

                // Attende se in pausa
                _PauseEvent.Wait();

                FileCopying?.Invoke(this, CreateCopierStatusEventArgs(job.SourceFile));


                if (job.IsMediaDevice)
                    this.copierMobile.Copy(job.DeviceDescription, job.SourceFile, job.DestinationFile);
                else
                    this.copierFileSystem.Copy(job.SourceFile, job.DestinationFile);
 
                job.CopyDone = true;
                _persistenceStore.Save();

                copiedFiles++;

            }
            CopyStatus = WorkerStatus.Idle;
            FileCopying?.Invoke(this, CreateCopierStatusEventArgs("Copy complete."));
            CopyCompleted?.Invoke(this, null);
        }

        private CopierStatusEventArgs CreateCopierStatusEventArgs(string status)
        {
            return new CopierStatusEventArgs()
            {
                Status = status,
                FilesToCopy = filesToCopy,
                CopiedFiles = copiedFiles
            };
        }


    }
}
