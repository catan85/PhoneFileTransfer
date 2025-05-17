using PhoneFileTransfer.Models;

namespace PhoneFileTransfer.Services.MobileBrowserService
{
    public interface IMobileBrowserService
    {
        WorkerStatus BrowseStatus { get; }
        IEnumerable<string> CurrentDirectories { get; }
        IEnumerable<string> CurrentFiles { get; }

        event EventHandler<WorkerStatus> BrowsingStatus;
        event EventHandler<IEnumerable<string>> DirectoriesUpdated;
        event EventHandler<string> ErrorOccurred;
        event EventHandler<IEnumerable<string>> FilesUpdated;
        event EventHandler<IEnumerable<string>> RecursiveDirectoriesBrowseCompleted;
        event EventHandler<int> RecursiveDirectoriesBrowseFoundFiles;

        void ExecuteBrowse();
        void ExecuteRecursiveDirectoryBrowse(List<string> directories);
        void CancelRecursiveDirectoryBrowse();
        List<string> GetDevicesNames();
        void GoUpFolder();
        void SetCurrentDevice(string deviceName);
        void SetCurrentFolder(string folderName);
    }
}