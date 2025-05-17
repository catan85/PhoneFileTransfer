using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.JobStoreService;
using PhoneFileTransfer.Utilities.Remover.RemoverFileSystem;
using PhoneFileTransfer.Utilities.Remover.RemoverMtp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.FileRemoverService
{
    internal class FileRemover : IFileRemover
    {
        private readonly IPersistenceStore _persistenceStore;
        private readonly IRemoverMtpUtil removerMtp;
        private readonly IRemoverFileSystemUtil removerFileSystem;
        private CancellationTokenSource _CancellationTokenSource;
        private ManualResetEventSlim _PauseEvent;
        private Task _Task;
        public event EventHandler<string> FileRemoving;
        public event EventHandler RemoveCompleted;

        public WorkerStatus RemoveStatus { get; private set; }

        public FileRemover(IPersistenceStore persistenceStore, IRemoverMtpUtil removerMtp, IRemoverFileSystemUtil removerFileSystem)
        {
            RemoveStatus = WorkerStatus.Idle;
            _persistenceStore = persistenceStore;
            this.removerMtp = removerMtp;
            this.removerFileSystem = removerFileSystem;
            _PauseEvent = new ManualResetEventSlim(true); // Inizialmente non in pausa
        }

        // Avvia la rimozione dei file in background
        public void ExecuteRemove(bool skipAlreadyRemoved)
        {
            RemoveStatus = WorkerStatus.Running;
            _CancellationTokenSource = new CancellationTokenSource();
            _Task = Task.Run(() => RemoveInBackGround(skipAlreadyRemoved, _CancellationTokenSource.Token));
        }

        // Pausa il processo di rimozione
        public void PauseRemove()
        {
            RemoveStatus = WorkerStatus.Paused;
            _PauseEvent.Reset();
            FileRemoving?.Invoke(this, "Remove Paused");
        }

        // Riprende il processo di rimozione
        public void ResumeRemove()
        {
            RemoveStatus = WorkerStatus.Running;
            _PauseEvent.Set();
            FileRemoving?.Invoke(this, "Remove Resumed");
        }

        // Cancella il processo di rimozione
        public void CancelRemove()
        {
            if (_CancellationTokenSource != null)
            {
                _CancellationTokenSource.Cancel();
            }
        }

        // Funzione privata che esegue la rimozione in background
        private void RemoveInBackGround(bool skipAlreadyRemoved, CancellationToken token)
        {
            var jobs = _persistenceStore.Get().JobList;

            if (skipAlreadyRemoved)
            {
                //filter done jobs
                jobs = jobs.Where(j => j.RemoveDone == false).ToList();
            }
            else
            {
                // reset done flags
                jobs.ForEach(j => j.RemoveDone = false);
            }


            foreach (var job in jobs)
            {
                // Verifica se il processo deve essere interrotto
                if (token.IsCancellationRequested)
                {
                    FileRemoving?.Invoke(this, "Remove cancelled.");
                    RemoveStatus = WorkerStatus.Idle;
                    return;
                }

                // Attende se in pausa
                _PauseEvent.Wait();

                // Rimozione del file
                FileRemoving?.Invoke(this, job.SourceFile);

                if (job.IsMediaDevice)
                    this.removerMtp.Remove(job.DeviceDescription, job.SourceFile);
                else
                    this.removerFileSystem.Remove(job.SourceFile);

            
                job.RemoveDone = true;
                _persistenceStore.Save();

            }

            RemoveStatus = WorkerStatus.Idle;
            FileRemoving?.Invoke(this, "Remove complete.");
            RemoveCompleted?.Invoke(this, null);
        }


    }
}
