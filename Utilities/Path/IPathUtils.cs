namespace PhoneFileTransfer.Utilities.Path
{
    public interface IPathUtils
    {
        string CombineSafe(params string[] segments);
    }
}