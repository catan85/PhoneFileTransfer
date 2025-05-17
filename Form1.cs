using PhoneFileTransfer.Models;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using PhoneFileTransfer.Services.JobStoreService;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PhoneFileTransfer.Services.FileCopierService;
using PhoneFileTransfer.Services.FileRemoverService;
using System.Net.NetworkInformation;
using System.Diagnostics;
using PhoneFileTransfer.Services.MtpBrowserService;
using PhoneFileTransfer.Services.FileCopierService.Models;
using PhoneFileTransfer.Services.FIleCopyAndRemoveService;


namespace PhoneFileTransfer
{
    public partial class MainForm : Form
    {

        private BindingList<Job> _jobList;
        private readonly IFileCopier fileCopier;
        private readonly IFileRemover fileRemover;
        private readonly IFileCopyAndRemover fileCopyAndRemover;

        private readonly IPersistenceStore persistenceStore;
        private readonly MtpFileDialog mtpFileDialog;

        public MainForm(IFileCopier fileCopier, IFileRemover fileRemover, IFileCopyAndRemover fileCopyAndRemover, IPersistenceStore persistenceStore, MtpFileDialog mtpFileDialog)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;

            persistenceStore.Load();
            _jobList = new BindingList<Job>(persistenceStore.Get().JobList);
            dataGridView1.DataSource = _jobList;

            this.fileCopier = fileCopier;
            this.fileRemover = fileRemover;
            this.fileCopyAndRemover = fileCopyAndRemover;
            this.persistenceStore = persistenceStore;
            this.mtpFileDialog = mtpFileDialog;
            fileCopier.FileCopying += this.FileCopier_FileCopying;
            fileRemover.FileRemoving += this.FileRemover_FileRemoving;
        }

        private void buttonAddJobs_Click(object sender, EventArgs e)
        {
            if (checkBoxMediaDevice.Checked)
            {
                BrowseSourcesWithMtpDialog();
            }
            else
            {
                BrowseSourcesFromFileSystem();
            }
        }

        private void BrowseSourcesWithMtpDialog()
        {
            var dialogResult = mtpFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK && mtpFileDialog.SelectedFilePaths.Count > 0)
            {
                var destinationFolder = ShowDestinationDialog();
                var sourceFiles = mtpFileDialog.SelectedFilePaths;
                if (mtpFileDialog.RelativePaths)
                {
                    persistenceStore.Get().LastSourcePath = mtpFileDialog.CurrentFolder;
                    AddJobs(true, mtpFileDialog.SelectedDevice, sourceFiles, destinationFolder, mtpFileDialog.CurrentFolder);
                }
                else
                {
                    persistenceStore.Get().LastSourcePath = Path.GetDirectoryName(sourceFiles.First());
                    AddJobs(true, mtpFileDialog.SelectedDevice, mtpFileDialog.SelectedFilePaths, destinationFolder, null);
                }
            }
        }

        private void BrowseSourcesFromFileSystem()
        {
            FileSystemFileDialog sourceFileDialog = new FileSystemFileDialog(persistenceStore.Get().LastSourcePath);

            var sourceResult = sourceFileDialog.ShowDialog();

            var sourceFiles = sourceFileDialog.SelectedFilePaths.ToList();


            if (sourceResult == DialogResult.OK && sourceFiles.Count() > 0)
            {


                var destinationFolder = ShowDestinationDialog();

                if (sourceFileDialog.RelativePaths)
                {
                    persistenceStore.Get().LastSourcePath = sourceFileDialog.CurrentFolder;
                    AddJobs(false, "", sourceFiles, destinationFolder, sourceFileDialog.CurrentFolder);
                }
                else
                {
                    persistenceStore.Get().LastSourcePath = Path.GetDirectoryName(sourceFiles.First());
                    AddJobs(false, "", sourceFiles, destinationFolder, null);
                }


            }
        }

        private void AddJobs(bool isMediaDevice, string deviceDescription, List<string> sourceFiles, string destinationFolder, string sourceFolder)
        {
            if (destinationFolder != null)
            {
                foreach (var file in sourceFiles)
                {
                    int id = GenerateId();
                    if (sourceFolder == null)
                    {
                        AddJob(id, isMediaDevice, deviceDescription, file, Path.Combine(destinationFolder, Path.GetFileName(file)));
                    }
                    else
                    {
                        AddJob(id, isMediaDevice, deviceDescription, sourceFolder + file, destinationFolder + file);
                    }

                }
            }
        }

        private string ShowDestinationDialog()
        {
            var destinationFolderDialog = new FolderBrowserDialog();
            destinationFolderDialog.InitialDirectory = persistenceStore.Get().LastDestinationPath;
            destinationFolderDialog.UseDescriptionForTitle = true;
            destinationFolderDialog.Description = "Select destination folder";
            var result = destinationFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var destinationFolder = destinationFolderDialog.SelectedPath;
                persistenceStore.Get().LastDestinationPath = destinationFolder;

                return destinationFolder;
            }
            else { return null; }
        }

        private int GenerateId()
        {
            int id = 1;
            if (_jobList.Count > 0)
            {
                id = _jobList.Last().Id + 1;
            }
            return id;
        }

        private void AddJob(int id, bool isMediaDevice, string deviceDescription, string sourceFile, string destinationFile)
        {
            _jobList.Add(new Job
            {
                Id = id,
                SourceFile = sourceFile,
                DestinationFile = destinationFile,
                IsMediaDevice = isMediaDevice,
                DeviceDescription = deviceDescription,
                CopyDone = false,
                RemoveDone = false
            });
            persistenceStore.Save();
        }

        // Metodo per rimuovere un Job esistente
        private void RemoveJob(List<Job> jobs)
        {
            foreach (Job job in jobs)
            {
                _jobList.Remove(job);
            }
            persistenceStore.Save();
        }


        private void buttonRemoveJobs_Click(object sender, EventArgs e)
        {
            var selectedJobs = new List<Job>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                selectedJobs.Add((Job)row.DataBoundItem);
            }

            RemoveJob(selectedJobs);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {


            if (fileCopier.CopyStatus == WorkerStatus.Idle)
            {
                bool skipDone = false;
                if (_jobList.Any(j => j.CopyDone))
                {
                    var resumeResponse = MessageBox.Show("Sono già stati elaborati dei job, riprendere l'esecuzione precedente?", "Confermare", MessageBoxButtons.OKCancel);
                    if (resumeResponse == DialogResult.OK)
                    {
                        skipDone = true;
                    }
                }
                fileCopier.ExecuteCopy(skipDone);
            }
            else if (fileCopier.CopyStatus == WorkerStatus.Running)
            {
                fileCopier.PauseCopy();
            }
            else if (fileCopier.CopyStatus == WorkerStatus.Paused)
            {
                fileCopier.ResumeCopy();
            }
        }

        private void FileCopier_FileCopying(object? sender, CopierStatusEventArgs e)
        {

            UiStatusCopy status = new UiStatusCopy();
            status.StatusText = $"Copying file: {e.Status}";
            status.CopierStatus = fileCopier.CopyStatus;
            status.CopiedFiles = e.CopiedFiles;
            status.FilesToCopy = e.FilesToCopy;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(status)));
            }
            else
            {
                UpdateUI(status);
            }
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (fileRemover.RemoveStatus == WorkerStatus.Idle)
            {
                bool skipDone = false;
                if (_jobList.Any(j => j.CopyDone))
                {
                    var resumeResponse = MessageBox.Show("Sono già stati elaborati dei job, riprendere l'esecuzione precedente?", "Confermare", MessageBoxButtons.OKCancel);
                    if (resumeResponse == DialogResult.OK)
                    {
                        skipDone = true;
                    }
                }
                fileRemover.ExecuteRemove(skipDone);
            }
            else if (fileRemover.RemoveStatus == WorkerStatus.Running)
            {
                fileRemover.PauseRemove();
            }
            else if (fileRemover.RemoveStatus == WorkerStatus.Paused)
            {
                fileRemover.ResumeRemove();
            }
        }

        private void FileRemover_FileRemoving(object? sender, string fileName)
        {
            UiStatusRemove status = new UiStatusRemove();
            status.StatusText = $"Removing file: {fileName}";
            status.RemoverStatus = fileRemover.RemoveStatus;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(status)));
            }
            else
            {
                UpdateUI(status);
            }
        }


        private void UpdateUI(UiStatusCopy status)
        {
            labelStatus.Text = status.StatusText;

            if (status.CopierStatus == WorkerStatus.Idle)
            {
                ResetButtonAppearance(buttonCopy);
                progressBar1.Visible = false;
                progressBar1.Value = 0;
            }
            else
            {
                SetButtonDisabledAppearance(buttonCopy);
                progressBar1.Visible = true;
                progressBar1.Value = status.CopiedFiles;
                progressBar1.Maximum = status.FilesToCopy;
            }
        }

        private void UpdateUI(UiStatusRemove status)
        {
            labelStatus.Text = status.StatusText;

            if (status.RemoverStatus == WorkerStatus.Idle)
            {
                ResetButtonAppearance(buttonRemove);
                progressBar1.Visible = true;
            }
            else
            {
                SetButtonDisabledAppearance(buttonRemove);
                progressBar1.Visible = false;
            }
        }

        private void SetButtonDisabledAppearance(System.Windows.Forms.Button button)
        {
            button.BackColor = Color.LightGray; // Cambia colore di sfondo
            button.ForeColor = Color.DarkGray;  // Cambia colore del testo
            button.FlatStyle = FlatStyle.Flat;  // Cambia lo stile del pulsante
            button.FlatAppearance.BorderColor = Color.DarkGray; // Cambia il colore del bordo
        }

        private void ResetButtonAppearance(System.Windows.Forms.Button button)
        {
            button.BackColor = SystemColors.Control; // Colore predefinito dello sfondo
            button.ForeColor = SystemColors.ControlText; // Colore predefinito del testo
            button.FlatStyle = FlatStyle.Standard; // Ripristina lo stile del pulsante
        }

        private void checkBoxMediaDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (this._jobList != null && this._jobList.Count > 0)
            {
                var mb = MessageBox.Show("Are you sure? Job list will be cleaned", "", MessageBoxButtons.YesNo);

                if (mb == DialogResult.Yes)
                {
                    this._jobList.Clear();
                }
            }
        }

        private void buttonCopyAndRemove_Click(object sender, EventArgs e)
        {
            if (fileRemover.RemoveStatus == WorkerStatus.Idle && fileCopier.CopyStatus == WorkerStatus.Idle)
            {
                bool skipDone = false;
                if (_jobList.Any(j => j.CopyDone) || _jobList.Any(j => j.RemoveDone))
                {
                    var resumeResponse = MessageBox.Show("Sono già stati elaborati dei job, riprendere l'esecuzione precedente?", "Confermare", MessageBoxButtons.OKCancel);
                    if (resumeResponse == DialogResult.OK)
                    {
                        skipDone = true;
                    }
                }
                fileCopyAndRemover.Execute(skipDone);
            }
        }
    }

    public class UiStatusCopy
    {
        public string StatusText { get; set; }
        public WorkerStatus CopierStatus { get; set; }
        public int FilesToCopy { get; set; }
        public int CopiedFiles { get; set; }
    }

    public class UiStatusRemove
    {
        public string StatusText { get; set; }
        public WorkerStatus RemoverStatus { get; set; }
    }
}
