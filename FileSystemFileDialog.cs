using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.MobileBrowserService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneFileTransfer
{
    public partial class FileSystemFileDialog : Form
    {
        private List<string> selectedFilePaths;

        public List<string> SelectedFilePaths { get => this.selectedFilePaths; }
        public string SelectedDevice { get; private set; }
        public bool RelativePaths { get; private set; }
        public string CurrentFolder { get; private set; }

        public FileSystemFileDialog(string initialFolder)
        {
            InitializeComponent();
            this.selectedFilePaths = new List<string>();
            this.CurrentFolder = initialFolder;
            ExecuteBrowse();
        }

        private void lstFolders_DoubleClick(object sender, EventArgs e)
        {
            if (lstFolders.SelectedItem != null)
            {
                CurrentFolder = (string)lstFolders.SelectedItem;

                ExecuteBrowse();
            }
        }

        private void ExecuteBrowse()
        {
            try
            {
                labelCurrentDirectory.Text = CurrentFolder;

                var currentDirs = Directory.GetDirectories(CurrentFolder);
                this.lstFolders.Items.Clear();
                this.lstFolders.Items.AddRange(currentDirs);

                var currentFiles = Directory.GetFiles(CurrentFolder);
                this.lstFiles.Items.Clear();
                this.lstFiles.Items.AddRange(currentFiles);
            }
            catch (Exception ex)
            {
                CurrentFolder = "C:\\";
                ExecuteBrowse();
            }

        }


        private void buttonUpFolder_Click(object sender, EventArgs e)
        {
            string parentDirectory = Path.GetFullPath(Path.Combine(CurrentFolder, ".."));
            CurrentFolder = parentDirectory;
            ExecuteBrowse();
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
            this.RelativePaths = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCopyDir_Click(object sender, EventArgs e)
        {
            var directoryQueue = new Queue<string>();
            foreach (var item in lstFolders.SelectedItems)
            {
                directoryQueue.Enqueue((string)item);
            }

            SelectedFilePaths.Clear();
            
            while (directoryQueue.Any())
            {
                var currentDirectory = directoryQueue.Dequeue();
                var currentSubDirectories = Directory.GetDirectories(currentDirectory);
                foreach (var subDir in currentSubDirectories)
                {
                    directoryQueue.Enqueue(subDir);
                }

                var currentFiles = Directory.GetFiles(currentDirectory);
                foreach (var file in currentFiles)
                {
                    SelectedFilePaths.Add(GetRelativePath(file, currentDirectory));
                }
            }
            this.RelativePaths = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        static string GetRelativePath(string fullPath, string rootPath)
        {
            Uri fullPathUri = new Uri(fullPath);
            Uri rootPathUri = new Uri(rootPath);

            Uri relativeUri = rootPathUri.MakeRelativeUri(fullPathUri);

            // Converti da URI a percorso file normale (sostituisce / con \ se necessario)
            return Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', '\\');
        }
    }

}
