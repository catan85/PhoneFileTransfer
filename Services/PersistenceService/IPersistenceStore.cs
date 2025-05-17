using PhoneFileTransfer.Models;

namespace PhoneFileTransfer.Services.JobStoreService
{
    public interface IPersistenceStore
    {
        public Persistence Get();
        public void Load();
        public void Save();
    }
}