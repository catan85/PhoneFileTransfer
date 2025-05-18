using Newtonsoft.Json;
using PhoneFileTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneFileTransfer.Services.Persistence
{
    internal class PersistenceStoreService : IPersistenceStoreService
    {
        private readonly string _persistenceFileName = "persistence.json";
        private Models.Persistence persistence = new Models.Persistence();

        public PersistenceStoreService() { }

        public void Save()
        {
            var data = JsonConvert.SerializeObject(persistence);
            File.WriteAllText(_persistenceFileName, data);
        }

        public void Load()
        {

            if (File.Exists(_persistenceFileName))
            {
                var data = File.ReadAllText(_persistenceFileName);
                this.persistence = JsonConvert.DeserializeObject<Models.Persistence>(data);
            }
            else
            {
                this.persistence = new Models.Persistence()
                { JobList = new List<Job>(), LastDestinationPath = "", LastSourcePath = "" };
            }
        }

        public Models.Persistence Get() { return persistence; }
    }
}
