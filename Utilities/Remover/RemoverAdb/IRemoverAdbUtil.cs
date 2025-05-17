namespace PhoneFileTransfer.Utilities.Remover.RemoverMtp
{
    internal interface IRemoverAdbUtil
    {
        void Remove(string deviceDescription, string path);
    }
}