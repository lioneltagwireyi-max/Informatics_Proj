using System;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Invoices : System.Web.UI.Page
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
                BindOrders();
            }
        }

        private void BindOrders()
        {
            try
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                OrderSummary[] orders = client.GetOrdersForUser(userID);
                if (orders == null || orders.Length == 0)
                {
                    pnlEmpty.Visible = true;
                    return;
                }

                rptOrders.DataSource = orders;
                rptOrders.DataBind();
            }
            catch
            {
                lblMessage.Text = "Invoices could not be loaded.";
                pnlEmpty.Visible = true;
            }
        }
    }
}
