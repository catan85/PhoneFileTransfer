using PhoneFileTransfer.Models;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using PhoneFileTransfer.Services.Persistence;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PhoneFileTransfer.Services.FileCopier;
using PhoneFileTransfer.Services.FileRemover;
using System.Net.NetworkInformation;
using System.Diagnostics;
using PhoneFileTransfer.Services.FileCopier.Models;
using PhoneFileTransfer.Services.FileCopyAndRemove;
using MediaDevices;
using System.IO;
using PhoneFileTransfer.Factories;
using PhoneFileTransfer.Utilities.Path;



namespace PhoneFileTransfer
{
    public partial class MainForm : Form
    {

        private BindingList<Job> _jobList;
        private IFileCopierService fileCopier;
        private IFileCopyAndRemoverService fileCopyAndRemover;
        private IFileRemoverService fileRemover;

        private readonly IPathUtils pathUtils;
        private readonly IPersistenceStoreService persistenceStore;
        private readonly IFactory factory;

        public MainForm(IPersistenceStoreService persistenceStore, IPathUtils pathUtils, IFactory factory)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;

            persistenceStore.Load();
            _jobList = new BindingList<Job>(persistenceStore.Get().JobList);
            dataGridView1.DataSource = _jobList;
 
  
            this.pathUtils = pathUtils;
            this.persistenceStore = persistenceStore;
            this.factory = factory;


            checkBoxMediaDevice.Checked = persistenceStore.Get().MobileMode;
            checkBoxAdbDriver.Checked = persistenceStore.Get().AdbMode;


            UpdateAdbSetting();
        }

        private void UpdateAdbSetting()
        {
            if (this.fileCopier != null)
            {
                this.fileCopier.FileCopying -= this.FileCopier_FileCopying;
            }

            this.fileCopier = this.factory.CreateFileCopierService(this.checkBoxAdbDriver.Checked);
            fileCopier.FileCopying += this.FileCopier_FileCopying;
            
            
            this.fileRemover = this.factory.CreateFileRemoverService(this.checkBoxAdbDriver.Checked);
            fileRemover.FileRemoving += this.FileRemover_FileRemoving;

            this.fileCopyAndRemover = this.factory.CreateFileCopyAndRemover(fileCopier,fileRemover);
        }

        private void buttonAddJobs_Click(object sender, EventArgs e)
        {
            if (checkBoxMediaDevice.Checked)
            {
                BrowseSourcesWithMobileDialog();
            }
            else
            {
                BrowseSourcesFromFileSystem();
            }
        }

        private void BrowseSourcesWithMobileDialog()
        {
            var mobileFileDialog = this.factory.CreateMobileFileDialog(checkBoxAdbDriver.Checked);

            mobileFileDialog.Init(persistenceStore.Get().DeviceName, persistenceStore.Get().LastSourcePath);

            var dialogResult = mobileFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK && mobileFileDialog.SelectedFilePaths.Count > 0)
            {
                var destinationFolder = ShowDestinationDialog();
                var sourceFiles = mobileFileDialog.SelectedFilePaths;
                if (mobileFileDialog.RelativePaths)
                {
                    persistenceStore.Get().LastSourcePath = mobileFileDialog.CurrentFolder;
                    AddJobs(true, mobileFileDialog.SelectedDevice, sourceFiles, destinationFolder, mobileFileDialog.CurrentFolder);
                }
                else
                {
                    persistenceStore.Get().LastSourcePath = Path.GetDirectoryName(sourceFiles.First());
                    AddJobs(true, mobileFileDialog.SelectedDevice, mobileFileDialog.SelectedFilePaths, destinationFolder, null);
                }
                persistenceStore.Get().DeviceName = mobileFileDialog.SelectedDevice;
            }
            persistenceStore.Save();
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
                        AddJob(id, isMediaDevice, deviceDescription, file, pathUtils.CombineSafe(destinationFolder, Path.GetFileName(file)));
                    }
                    else
                    {
                        AddJob(id, isMediaDevice, deviceDescription, pathUtils.CombineSafe(sourceFolder,file), pathUtils.CombineSafe(destinationFolder, file));
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
            persistenceStore.Get().MobileMode = checkBoxMediaDevice.Checked;
            persistenceStore.Save();

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAdbDriver_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAdbSetting();
            persistenceStore.Get().AdbMode = checkBoxAdbDriver.Checked;
            persistenceStore.Save();
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
