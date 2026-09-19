using System;

namespace PhoneFit
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActivityTracker.LogCustomerAction(
                    ActivityTracker.ActionPageView,
                    "Home.aspx",
                    "Viewed store home");
            }
        }
    }
}
