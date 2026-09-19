using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class shop : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActivityTracker.LogCustomerAction(
                    ActivityTracker.ActionPageView,
                    "shop.aspx",
                    "Browsed phone catalogue");
                DisplayPhones();
            }
        }

        private void DisplayPhones()
        {
            var phones = client.GetActivePhones();
            rptPhones.DataSource = phones;
            rptPhones.DataBind();
        }
    }
}