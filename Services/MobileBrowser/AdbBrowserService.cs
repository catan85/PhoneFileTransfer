using PhoneFileTransfer.Models;
using PhoneFileTransfer.Services.MobileBrowser;
using PhoneFileTransfer.Utilities.AdbServerStarter;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.MobileBrowser
{
    public class AdbBrowserService : IMobileBrowserService
    {
        private AdbClient _adbClient;
        private DeviceData _selectedDevice;

        private readonly List<string> _previousPaths = new();
        private CancellationTokenSource _cts;
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

        public AdbBrowserService(IAdbServerStarterUtil adbStarter)
        {
            _adbClient = new AdbClient();
            if (!adbStarter.IsAdbServerRunning())
                adbStarter.StartAdbServer();
        }

        public List<string> GetDevicesNames()
        {
            var devices = _adbClient.GetDevices();
            return devices.Select(d => d.Serial).ToList();
        }

        public void SetCurrentDevice(string deviceSerial)
        {
            var devices = _adbClient.GetDevices();
            _selectedDevice = devices.FirstOrDefault(d => d.Serial == deviceSerial);
            if (_selectedDevice == null)
                throw new Exception("Device not found");

            _currentDirectory = "/"; // root directory in Android file system
            _previousPaths.Clear();
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
                _previousPaths.RemoveAt(_previousPaths.Count - 1);
                _currentDirectory = _previousPaths.Last();
                ExecuteBrowse();
            }
        }

        public void ExecuteBrowse()
        {
            BrowseStatus = WorkerStatus.Running;
            BrowsingStatus?.Invoke(this, BrowseStatus);

            _cts = new CancellationTokenSource();
            _browseTask = Task.Run(() => BrowseInBackground(_cts.Token));
        }

        public void ExecuteRecursiveDirectoryBrowse(List<string> directories)
        {
            BrowseStatus = WorkerStatus.Running;
            BrowsingStatus?.Invoke(this, BrowseStatus);

            _cts = new CancellationTokenSource();
            _browseTask = Task.Run(() => BrowseRecursivelyDirectoriesInBackground(_cts.Token, directories));
        }

        public void CancelRecursiveDirectoryBrowse()
        {
            _cts?.Cancel();
        }

        private async Task BrowseInBackground(CancellationToken token)
        {
            try
            {
                var data = await NavigateToFolder(_currentDirectory);
                DirectoriesUpdated?.Invoke(this, data.directories);
                FilesUpdated?.Invoke(this, data.files);
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
        }

        private async Task<(List<string> directories, List<string> files)> NavigateToFolder(string path)
        {
            _currentDirectories.Clear();
            _currentFiles.Clear();

            // Forza output un elemento per riga
            string cmd = $"export LC_ALL=C.UTF-8 && ls -p -1 \"{path}\"";
            var output = await RunShellCommandAsync(cmd);

            var directories = new List<string>();
            var files = new List<string>();

            var lines = output
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();

            foreach (var line in lines)
            {
                bool isDirectory = line.EndsWith("/");
                var name = isDirectory ? line.TrimEnd('/') : line;

                name = UnescapeAdbPath(name); // <-- qui unescape del nome

                var fullPath = Path.Combine(path, name).Replace('\\', '/');

                if (isDirectory)
                    directories.Add(fullPath + "/");
                else
                    files.Add(fullPath);
            }

            _currentDirectories.AddRange(directories);
            _currentFiles.AddRange(files);

            return (directories, files);
        }

        private string UnescapeAdbPath(string path)
        {
            // Molto basilare. Personalizzalo se i dispositivi usano encoding più complessi
            return path.Replace("\\ ", " "); // gestisce solo spazi per ora
        }

        private async Task<string> RunShellCommandAsync(string command)
        {
            var receiver = new ConsoleOutputReceiver();
            await Task.Run(() =>
            {
                _adbClient.ExecuteRemoteCommand(command, _selectedDevice, receiver);
            });
            return receiver.ToString();
        }

        private async Task BrowseRecursivelyDirectoriesInBackground(CancellationToken token, List<string> directories)
        {
            try
            {
                var data = await GetFilesInFoldersRecursively(token, directories);
                RecursiveDirectoriesBrowseCompleted?.Invoke(this, data);
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
        }

        public async Task<List<string>> GetFilesInFoldersRecursively(CancellationToken token, List<string> folderNames)
        {
            List<string> foundFiles = new List<string>();
            try
            {
                Queue<string> directoryQueue = new Queue<string>(folderNames);

                while (directoryQueue.Count > 0)
                {
                    if (token.IsCancellationRequested)
                    {
                        foundFiles.Clear();
                        break;
                    }

                    RecursiveDirectoriesBrowseFoundFiles?.Invoke(this, foundFiles.Count);

                    var currentDirectory = directoryQueue.Dequeue();

                    var subDirsAndFiles = await NavigateToFolder(currentDirectory);
                    foreach (var subDir in subDirsAndFiles.directories)
                    {
                        directoryQueue.Enqueue(subDir);
                    }

                    foreach (var file in subDirsAndFiles.files)
                    {
                        foundFiles.Add(GetRelativePath(file, _currentDirectory));
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }

            return foundFiles;
        }

        private void HandleError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex.Message);
        }

        static string GetRelativePath(string fullPath, string rootPath)
        {
            if (fullPath.StartsWith(rootPath))
                return fullPath.Substring(rootPath.Length).TrimStart('/');
            return fullPath;
        }
    }

}
