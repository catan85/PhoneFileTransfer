namespace PhoneFileTransfer.Utilities.AdbServerStarter
{
    public interface IAdbServerStarterUtil
    {
        bool IsAdbServerRunning();
        void StartAdbServer();
    }
}