using System;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class PhoneFitMaster : System.Web.UI.MasterPage
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            string roleName = Session["RoleName"] as string;
            bool loggedIn = Session["UserID"] != null;

            pnlGuestLinks.Visible = !loggedIn;
            pnlAuthLinks.Visible = loggedIn;
            lnkAdmin.Visible = AppRoles.IsAdminRole(roleName);
            lnkProducts.Visible = AppRoles.IsAdminRole(roleName);
            lnkInvoices.Visible = AppRoles.IsCustomerRole(roleName);
            lnkCart.Visible = !AppRoles.IsAdminRole(roleName);
            lnkReports.Visible = AppRoles.IsAdminRole(roleName);

            lblCartCount.Text = "0";

            if (loggedIn && AppRoles.IsCustomerRole(roleName))
            {
                try
                {
                    int userID = Convert.ToInt32(Session["UserID"]);
                    CartItemInfo[] cartItems = client.GetCartItems(userID);
                    int cartCount = 0;
                    if (cartItems != null)
                    {
                        foreach (CartItemInfo item in cartItems)
                        {
                            cartCount += item.Quantity;
                        }
                    }
                    lblCartCount.Text = cartCount.ToString();
                }
                catch
                {
                    lblCartCount.Text = "0";
                }
            }

            if (loggedIn)
            {
                litAccountLabel.Text = AppRoles.IsAdminRole(roleName) ? "Admin" : "Account";
            }
        }
    }
}
