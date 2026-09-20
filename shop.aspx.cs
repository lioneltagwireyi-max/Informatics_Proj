using System;
using System.Collections.Generic;
using System.Linq;
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

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayPhones();
        }

        private void DisplayPhones()
        {
            PhoneCatalogue[] phones = client.GetActivePhones();
            if (phones == null)
            {
                phones = new PhoneCatalogue[0];
            }

            IEnumerable<PhoneCatalogue> sorted = phones;
            string sortKey = ddlSort.SelectedValue;

            switch (sortKey)
            {
                case "name_desc":
                    sorted = phones.OrderByDescending(p => p.ModelName);
                    break;
                case "price_asc":
                    sorted = phones.OrderBy(p => p.StartingPrice).ThenBy(p => p.ModelName);
                    break;
                case "price_desc":
                    sorted = phones.OrderByDescending(p => p.StartingPrice).ThenBy(p => p.ModelName);
                    break;
                default:
                    sorted = phones.OrderBy(p => p.ModelName);
                    break;
            }

            PhoneCatalogue[] list = sorted.ToArray();
            rptPhones.DataSource = list;
            rptPhones.DataBind();

            int count = list.Length;
            lblPhoneCount.Text = count == 1
                ? "Showing 1 smartphone"
                : "Showing " + count + " smartphones";
        }
    }
}
