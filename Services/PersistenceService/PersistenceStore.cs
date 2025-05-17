using Newtonsoft.Json;
using PhoneFileTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneFileTransfer.Services.JobStoreService
{
    internal class PersistenceStore : IPersistenceStore
    {
        private readonly string _persistenceFileName = "persistence.json";
        private Persistence persistence = new Persistence();

        public PersistenceStore() { }

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
                persistence = JsonConvert.DeserializeObject<Persistence>(data);
            }
            else
            {
                persistence = new Persistence()
                { JobList = new List<Job>(), LastDestinationPath = "", LastSourcePath = "" };
            }
        }

        public Persistence Get() { return persistence; }
    }
}
