using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.MobileBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace PhoneFileTransfer
{

    public partial class MobileFileDialog : Form
    {
        private List<string> selectedFilePaths;
        private List<string> previousPaths = new List<string>();

        private readonly IMobileBrowserService mobileBrowserService;
        public List<string> SelectedFilePaths { get => this.selectedFilePaths; }
        public string SelectedDevice { get; private set; }
        public string CurrentFolder { get; private set; }
        public bool RelativePaths { get; private set; }

        public MobileFileDialog(IMobileBrowserService mobileBrowserService)
        {
            InitializeComponent();
            this.selectedFilePaths = new List<string>();
            this.mobileBrowserService = mobileBrowserService;
            this.mobileBrowserService.DirectoriesUpdated += this.MobileBrowserService_DirectoriesUpdated;
            this.mobileBrowserService.FilesUpdated += this.MobileBrowserService_FilesUpdated;
            this.mobileBrowserService.BrowsingStatus += this.MobileBrowserService_BrowsingStatus;
            this.mobileBrowserService.RecursiveDirectoriesBrowseCompleted += this.MobileBrowserService_RecursiveDirectoriesBrowseCompleted;
            this.mobileBrowserService.RecursiveDirectoriesBrowseFoundFiles += this.MobileBrowserService_RecursiveDirectoriesBrowseFoundFiles;
        }



        private void btnSelectDevice_Click(object sender, EventArgs e)
        {
            var names = mobileBrowserService.GetDevicesNames();
            SelectionDialog selectionDialog = new SelectionDialog("Seleziona il dispositivo", names);
            if (selectionDialog.ShowDialog() == DialogResult.OK)
            {
                mobileBrowserService.SetCurrentDevice(selectionDialog.SelectedItem);
                this.SelectedDevice = selectionDialog.SelectedItem;
                mobileBrowserService.ExecuteBrowse();
            }
        }

        private void lstFolders_DoubleClick(object sender, EventArgs e)
        {
            if (lstFolders.SelectedItem != null)
            {
                CurrentFolder = (string)lstFolders.SelectedItem;
                mobileBrowserService.SetCurrentFolder(CurrentFolder);
                mobileBrowserService.ExecuteBrowse();
            }
        }


        private void buttonUpFolder_Click(object sender, EventArgs e)
        {
            mobileBrowserService.GoUpFolder();
        }


        private void btnCopyFiles_Click(object sender, EventArgs e)
        {
            var selectedFiles = new List<string>();
            foreach (var item in lstFiles.SelectedItems)
            {
                selectedFiles.Add((string)item);
            }
            SelectedFilePaths.Clear();
            SelectedFilePaths.AddRange(selectedFiles);
            RelativePaths = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // aggiungere copia ricorsiva delle directory, dovrebbe eseguire in background ed al termine chiudere il dialog


        private void MobileBrowserService_DirectoriesUpdated(object? sender, IEnumerable<string> currentFolders)
        {
            UpdateFoldersUiData data = new UpdateFoldersUiData();
            data.FolderNames = currentFolders.ToList();

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(data)));
            }
            else
            {
                UpdateUI(data);
            }
        }

        private void UpdateUI(UpdateFoldersUiData foldersData)
        {
            lstFolders.Items.Clear();

            foreach (var folder in foldersData.FolderNames)
            {
                lstFolders.Items.Add(folder);
            }
        }


        private void MobileBrowserService_FilesUpdated(object? sender, IEnumerable<string> fileNames)
        {
            UpdateFilesUiData data = new UpdateFilesUiData();
            data.FileNames = fileNames.ToList();

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(data)));
            }
            else
            {
                UpdateUI(data);
            }
        }

        private void UpdateUI(UpdateFilesUiData filesData)
        {
            lstFiles.Items.Clear();

            foreach (var file in filesData.FileNames)
            {
                lstFiles.Items.Add(file);
            }
        }

        private void MobileBrowserService_BrowsingStatus(object? sender, WorkerStatus e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(e)));
            }
            else
            {
                UpdateUI(e);
            }
        }


        private void UpdateUI(WorkerStatus status)
        {
            if (status == WorkerStatus.Running)
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mobileBrowserService.BrowseStatus == WorkerStatus.Idle)
            {
                List<string> selectedDirs = new List<string>();
                foreach (string selectedFolder in lstFolders.SelectedItems)
                {
                    selectedDirs.Add(selectedFolder);
                }
                mobileBrowserService.ExecuteRecursiveDirectoryBrowse(selectedDirs);

                buttonCopyFolders.Text = "Cancel";
            }
            else
            {
                mobileBrowserService.CancelRecursiveDirectoryBrowse();
            }

        }

        private void MobileBrowserService_RecursiveDirectoriesBrowseFoundFiles(object? sender, int e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateFoundFiles(e)));
            }
            else
            {
                UpdateFoundFiles(e);
            }
        }

        private void UpdateFoundFiles(int foundFiles)
        {
            labelStatus.Text = $"Found files: {foundFiles}";
        }

        private void MobileBrowserService_RecursiveDirectoriesBrowseCompleted(object? sender, IEnumerable<string> e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ResetCopyFolderButtonText()));

                if (e.Any())
                {
                    this.Invoke(new Action(() => ReturnFolderResult(e)));
                }
            }
            else
            {
                ResetCopyFolderButtonText();
                if (e.Any())
                {
                    ReturnFolderResult(e);
                }
            }
        }

        private void ReturnFolderResult(IEnumerable<string> e)
        {
            var selectedFiles = new List<string>();
            foreach (var item in e)
            {
                selectedFiles.Add((string)item);
            }
            SelectedFilePaths.Clear();
            SelectedFilePaths.AddRange(selectedFiles);

            this.RelativePaths = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ResetCopyFolderButtonText()
        {
            buttonCopyFolders.Text = "Copia folder selezionate";
        }

        private void MobileFileDialog_Load(object sender, EventArgs e)
        {

        }
    }

    public class UpdateFoldersUiData
    {
        public List<string> FolderNames;
    }

    public class UpdateFilesUiData
    {
        public List<string> FileNames;
    }


}