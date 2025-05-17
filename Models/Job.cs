using System.ComponentModel;

public class Job : INotifyPropertyChanged
{
    private bool _copyDone;
    private bool _removeDone;

    public int Id { get; set; }
    public string SourceFile { get; set; }
    public string DestinationFile { get; set; }
    public bool IsMediaDevice { get; set; }
    public string DeviceDescription { get; set; }

    public bool CopyDone
    {
        get { return _copyDone; }
        set
        {
            if (_copyDone != value)
            {
                _copyDone = value;
                OnPropertyChanged("CopyDone");
            }
        }
    }

    public bool RemoveDone
    {
        get { return _removeDone; }
        set
        {
            if (_removeDone != value)
            {
                _removeDone = value;
                OnPropertyChanged("RemoveDone");
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
