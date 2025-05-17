namespace PhoneFileTransfer.Utilities.Copier.CopierMtp
{
    public interface ICopierMtpUtil
    {
        void Copy(string deviceDescription, string source, string destination);
    }
}