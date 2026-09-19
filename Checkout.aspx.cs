using System;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Checkout : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                Response.Redirect(Session["UserID"] == null ? "Login.aspx" : "Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PreviewCart();
            }
        }

        private void PreviewCart()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            CartItemInfo[] items = client.GetCartItems(userID);

            if (items == null || items.Length == 0)
            {
                pnlPreview.Visible = false;
                lblMessage.Text = "Your cart is empty. Add phones before checkout.";
                return;
            }

            int count = 0;
            decimal subtotal = 0;
            foreach (CartItemInfo item in items)
            {
                count += item.Quantity;
                subtotal += item.LineTotal;
            }

            litItemCount.Text = count.ToString();
            litSubtotal.Text = subtotal.ToString("N2");
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            try
            {
                OrderInvoice invoice = client.PlaceOrder(userID);
                if (invoice == null || invoice.OrderID <= 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "The order could not be placed. Check stock and try again.";
                    PreviewCart();
                    return;
                }

                ActivityTracker.LogCustomerAction(
                    ActivityTracker.ActionCheckout,
                    "Checkout.aspx",
                    "Order " + invoice.OrderID + " placed");

                Session["LastInvoice"] = invoice;
                Response.Redirect("Invoice.aspx?id=" + invoice.OrderID);
            }
            catch (Exception)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Checkout failed. Ensure the PhoneFit service is running.";
            }
        }
    }
}
