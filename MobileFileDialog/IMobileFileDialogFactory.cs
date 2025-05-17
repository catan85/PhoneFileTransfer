namespace PhoneFileTransfer.MobileFileDialog
{
    public interface IMobileFileDialogFactory
    {
        MobileFileDialog Create(bool adbMod);
    }
}