using Microsoft.Extensions.DependencyInjection;
using PhoneFileTransfer.Services.MobileBrowserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.Services.MobileBrowserService
{
    public class MobileBrowserServiceFactory : IMobileBrowserServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MobileBrowserServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMobileBrowserService Create(bool useAdb)
        {
            if (useAdb)
                return _serviceProvider.GetRequiredService<AdbBrowserService>();
            else
                return _serviceProvider.GetRequiredService<MtpBrowserService>();
        }
    }
}
