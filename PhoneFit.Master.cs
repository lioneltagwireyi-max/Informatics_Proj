using System;
using System.Web.UI.HtmlControls;
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
            bool isAdmin = AppRoles.IsAdminRole(roleName);
            bool isCustomer = AppRoles.IsCustomerRole(roleName);

            if (bodyTag != null)
            {
                string roleClass = !loggedIn ? "role-guest"
                    : isAdmin ? "role-admin"
                    : "role-customer";
                bodyTag.Attributes["class"] = roleClass;
            }

            pnlGuestLinks.Visible = !loggedIn;
            pnlAuthLinks.Visible = loggedIn;
            pnlAdminNav.Visible = isAdmin;
            pnlCustomerNav.Visible = loggedIn && isCustomer;
            pnlGuestShopNav.Visible = !loggedIn || (!isAdmin && !isCustomer);

            // Keep individual links consistent for older pages that expect them.
            lnkAdmin.Visible = isAdmin;
            lnkProducts.Visible = isAdmin;
            lnkReports.Visible = isAdmin;
            lnkInvoices.Visible = loggedIn && isCustomer;
            lnkCart.Visible = !isAdmin;
            lnkCartIcon.Visible = !isAdmin;
            lnkBrowse.Visible = !isAdmin;
            lnkBrowse.Text = isAdmin ? "Admin home" : "Browse Phones";
            if (isAdmin)
            {
                lnkBrowse.NavigateUrl = "~/Manager.aspx";
            }

            if (!loggedIn)
            {
                litUtilityTag.Text = "<span class=\"tag\">PHONEFIT</span>";
                litUtilityPromo.Text = "Free shipping on orders over R1 000 · 30-day returns";
                litRoleChip.Text = string.Empty;
                lnkAccountIcon.HRef = "Login.aspx";
            }
            else if (isAdmin)
            {
                litUtilityTag.Text = "<span class=\"tag\">ADMIN</span>";
                litUtilityPromo.Text = "Manage catalogue, users and reports";
                litRoleChip.Text = "<span class=\"role-chip\">Signed in as Admin</span>";
                litAccountLabel.Text = "Admin";
                lnkAccountIcon.HRef = "Manager.aspx";
            }
            else
            {
                litUtilityTag.Text = "<span class=\"tag\">MEMBER</span>";
                litUtilityPromo.Text = "Loyalty discount on return orders · Free shipping from R1 000";
                litRoleChip.Text = "<span class=\"role-chip\">Customer account</span>";
                litAccountLabel.Text = "Account";
                lnkAccountIcon.HRef = "Home.aspx";
            }

            lblCartCount.Text = "0";

            if (loggedIn && isCustomer)
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
        }
    }
}
