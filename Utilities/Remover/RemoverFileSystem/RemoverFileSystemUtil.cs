using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Remover.RemoverFileSystem
{
    internal class RemoverFileSystemUtil : IRemoverFileSystemUtil
    {
        public void Remove(string path)
        {
            File.Delete(path);
        }
    }
}
