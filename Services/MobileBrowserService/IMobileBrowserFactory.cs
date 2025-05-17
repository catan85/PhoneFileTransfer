using PhoneFileTransfer.Services.MobileBrowserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.MobileBrowserService
{
    public interface IMobileBrowserServiceFactory
    {
        IMobileBrowserService Create(bool useAdb);
    }
}
