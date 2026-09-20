using System;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Invoice : System.Web.UI.Page
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
                int orderId;
                if (!int.TryParse(Request.QueryString["id"], out orderId))
                {
                    lblMessage.Text = "Invoice not found.";
                    return;
                }

                LoadInvoice(orderId);
            }
        }

        private void LoadInvoice(int orderId)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            OrderInvoice invoice = client.GetInvoice(userID, orderId);

            // Fresh checkout passes totals via PlaceOrder redirect session stash
            if (Session["LastInvoice"] is OrderInvoice fresh
                && fresh.OrderID == orderId)
            {
                invoice = fresh;
                Session.Remove("LastInvoice");
            }

            if (invoice == null)
            {
                lblMessage.Text = "Invoice not found for this account.";
                return;
            }

            pnlInvoice.Visible = true;
            litOrderId.Text = invoice.OrderID.ToString();
            litOrderDate.Text = invoice.OrderDate.ToString("dd MMM yyyy HH:mm");
            litStatus.Text = invoice.OrderStatus;
            litSubtotal.Text = invoice.Subtotal.ToString("N2");
            litDiscount.Text = invoice.DiscountAmount.ToString("N2");
            litShipping.Text = invoice.ShippingAmount.ToString("N2");
            litTax.Text = invoice.TaxAmount.ToString("N2");
            litTotal.Text = invoice.TotalAmount.ToString("N2");
            litNotes.Text = invoice.TransactionNotes;
            rptLines.DataSource = invoice.Lines;
            rptLines.DataBind();
        }
    }
}
