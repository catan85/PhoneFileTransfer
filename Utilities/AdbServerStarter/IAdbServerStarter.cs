namespace PhoneFileTransfer.Utilities.AdbServerStarter
{
    public interface IAdbServerStarter
    {
        bool IsAdbServerRunning();
        void StartAdbServer();
    }
}