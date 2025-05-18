using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Utilities.Copier.CopierFileSystem
{
    public class CopierFileSystemUtil : ICopierFileSystemUtil
    {
        public void Copy(string source, string destination)
        {
            var dir = System.IO.Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Console.WriteLine($"Directory created: {dir}");
            }
            File.Copy(source, destination, true);
        }
    }
}
