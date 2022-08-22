using Microsoft.VisualStudio.ComponentModelHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Rhino.VisualStudio.Windows
{
    static class Services
    {

        public static IOleServiceProvider ServiceProvider
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return (IOleServiceProvider)Package.GetGlobalService(typeof(IOleServiceProvider));
            }
        }

        static ServiceProvider vsServiceProvider;

        public static ServiceProvider VsServiceProvider
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return vsServiceProvider ?? (vsServiceProvider = new ServiceProvider(ServiceProvider));
            }
        }

        public static T GetService<T>()
          where T : class
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return VsServiceProvider.GetService(typeof(T)) as T;
        }
    }
}
