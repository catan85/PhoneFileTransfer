using PhoneFileTransfer.Models;

namespace PhoneFileTransfer.Services.Persistence
{
    public interface IPersistenceStoreService
    {
        public Models.Persistence Get();
        public void Load();
        public void Save();
    }
}