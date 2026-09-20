using System;
using System.Web;

namespace PhoneFit
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Startup hook. App_Start BundleConfig/RouteConfig were removed —
            // PhoneFit.Master uses assets/ and explicit .aspx URLs.
        }
    }
}
