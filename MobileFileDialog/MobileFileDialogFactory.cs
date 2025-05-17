using PhoneFileTransfer.Services.MobileBrowserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFileTransfer.MobileFileDialog
{
    public class MobileFileDialogFactory : IMobileFileDialogFactory
    {
        private readonly IMobileBrowserServiceFactory browserFactory;

        public MobileFileDialogFactory(IServiceProvider serviceProvider,IMobileBrowserServiceFactory browserFactory)
        {
            this.browserFactory = browserFactory;
        }

        public MobileFileDialog Create(bool useAdb)
        {
            var browserService = browserFactory.Create(useAdb);
            return new MobileFileDialog(browserService);
        }
    }
}
