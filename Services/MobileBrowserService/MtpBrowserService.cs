using MediaDevices;
using PhoneFileTransfer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PhoneFileTransfer.Services.MobileBrowserService
{
    public class MtpBrowserService : IMobileBrowserService
    {
        private MediaDevice _selectedDevice;
        
        private readonly List<string> _previousPaths = new();
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _pauseEvent;
        private Task _browseTask;
        private List<string> _currentDirectories = new();
        private List<string> _currentFiles = new();

        public event EventHandler<WorkerStatus> BrowsingStatus;
        public event EventHandler<string> ErrorOccurred;
        public event EventHandler<IEnumerable<string>> DirectoriesUpdated;
        public event EventHandler<IEnumerable<string>> FilesUpdated;
        public event EventHandler<int> RecursiveDirectoriesBrowseFoundFiles;
        public event EventHandler<IEnumerable<string>> RecursiveDirectoriesBrowseCompleted;


        public WorkerStatus BrowseStatus { get; private set; } = WorkerStatus.Idle;
        public IEnumerable<string> CurrentDirectories => _currentDirectories;
        public IEnumerable<string> CurrentFiles => _currentFiles;


        private string _currentDirectory;

        public MtpBrowserService()
        {
            _pauseEvent = new ManualResetEventSlim(true);
        }

        public List<string> GetDevicesNames()
        {
            return MediaDevice.GetDevices().ToList().Select(d => d.Description).ToList();
        }

        public void SetCurrentDevice(string deviceName)
        {
            _selectedDevice = MediaDevice.GetDevices().FirstOrDefault(d => d.Description == deviceName);
            _selectedDevice.Connect(enableCache:false);
            _currentDirectory = _selectedDevice.GetRootDirectory().FullName;
            _previousPaths.Add(_currentDirectory);
        }

        public void SetCurrentFolder(string folderName)
        {
            if (folderName != null) 
                _previousPaths.Add(folderName);

            _currentDirectory = folderName;
        }

        public void GoUpFolder()
        {
            if (_previousPaths.Count > 1)
            {
                _previousPaths.Remove(_previousPaths.Last());
                _currentDirectory = _previousPaths.LastOrDefault();
                ExecuteBrowse();
            }
        }

        public void ExecuteBrowse()
        {
            BrowseStatus = WorkerStatus.Running;
            BrowsingStatus?.Invoke(this, BrowseStatus);

            _cts = new CancellationTokenSource();
            _browseTask = Task.Run(() => BrowseInBackGround(_cts.Token));
        }

        public void ExecuteRecursiveDirectoryBrowse(List<string> directories)
        {
            BrowseStatus = WorkerStatus.Running;
            BrowsingStatus?.Invoke(this, BrowseStatus);
            _cts = new CancellationTokenSource() { };
            _browseTask = Task.Run(() => BrowseRecursivelyDirectoriesInBackground(_cts.Token, directories));

        }

        public void CancelRecursiveDirectoryBrowse()
        {
            _cts.Cancel();
        }

        private async Task BrowseInBackGround(CancellationToken token)
        {
            try
            {
                var data = NavigateToFolder(_currentDirectory);
                
                DirectoriesUpdated?.Invoke(this, data.Result.directories);
                FilesUpdated?.Invoke(this, data.Result.files);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        // Modifica nel servizio MtpBrowserService
        private async Task<(List<string> directories, List<string> files)> NavigateToFolder(string path)
        {
            try
            {
                _currentDirectory = path;

                _currentDirectories.Clear();
                _currentFiles.Clear();

                var directories = await Task.Run(() =>
                    _selectedDevice.GetDirectories(path).ToList());

                var files = await Task.Run(() =>
                    _selectedDevice.GetFiles(path).ToList());

                _currentDirectories.AddRange(directories);
                _currentFiles.AddRange(files);
                return (directories, files);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
            finally
            {
                BrowseStatus = WorkerStatus.Idle;
                BrowsingStatus?.Invoke(this, BrowseStatus);
            }
            return (null, null);
        }

        private async Task BrowseRecursivelyDirectoriesInBackground(CancellationToken token, List<string> directories)
        {
            try
            {
                var data = GetFilesInFoldersRecursively(token, directories);
                RecursiveDirectoriesBrowseCompleted?.Invoke(this, data.Result);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void HandleError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex.Message);
        }

        public async Task<List<string>> GetFilesInFoldersRecursively(CancellationToken token, List<string> folderNames)
        {
            List<string> foundFiles = new List<string>();
            try
            {
                Queue<string> directoryQueue = new Queue<string>();
                foreach (string selectedFolder in folderNames)
                {
                    directoryQueue.Enqueue(selectedFolder);
                }

                while (directoryQueue.Any())
                {
                    if (token.IsCancellationRequested)
                    {
                        foundFiles = new List<string>();
                        break;
                    }

                    RecursiveDirectoriesBrowseFoundFiles?.Invoke(this, foundFiles.Count);

                    var currentDirectory = directoryQueue.Dequeue();
                    var currentSubDirectories = await Task.Run(() =>
                        _selectedDevice.GetDirectories(currentDirectory).ToList());

                    foreach (var subDir in currentSubDirectories)
                    {
                        directoryQueue.Enqueue(subDir);
                    }

                    var currentFiles = await Task.Run(() => _selectedDevice.GetFiles(currentDirectory).ToList());
                    foreach (var file in currentFiles)
                    {
                        foundFiles.Add(GetRelativePath(file,this._currentDirectory));
                    }
                }
            }
            catch (Exception ex) { HandleError(ex); }
            finally
            {
                BrowseStatus = WorkerStatus.Idle;
                BrowsingStatus?.Invoke(this, BrowseStatus);
            }
            
            return foundFiles;
        }
        static string GetRelativePath(string fullPath, string rootPath)
        {
            return fullPath.Replace(rootPath, "");
        }
    }

}
