using System;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Products : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppRoles.IsAdminRole(Session["RoleName"] as string))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["added"]))
                {
                    lblMessage.ForeColor = System.Drawing.Color.DarkGreen;
                    lblMessage.Text = "Product added successfully.";
                }
                else if (Request.QueryString["deleted"] == "1")
                {
                    lblMessage.ForeColor = System.Drawing.Color.DarkGreen;
                    lblMessage.Text = "Product deactivated.";
                }

                BindProducts();
            }
        }

        private void BindProducts()
        {
            try
            {
                ProductAdminInfo[] products = client.GetAllProductsForAdmin();
                if (products == null || products.Length == 0)
                {
                    pnlEmpty.Visible = true;
                    rptProducts.Visible = false;
                    lblCount.Text = "0 products";
                    return;
                }

                pnlEmpty.Visible = false;
                rptProducts.Visible = true;
                rptProducts.DataSource = products;
                rptProducts.DataBind();
                lblCount.Text = products.Length + " products in catalogue";
            }
            catch
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Products could not be loaded. Ensure PhoneFitService is running.";
                pnlEmpty.Visible = true;
            }
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "DeleteProduct")
            {
                return;
            }

            int phoneModelId;
            if (!int.TryParse(e.CommandArgument.ToString(), out phoneModelId))
            {
                return;
            }

            try
            {
                bool deleted = client.DeleteProduct(phoneModelId);
                lblMessage.ForeColor = deleted
                    ? System.Drawing.Color.DarkGreen
                    : System.Drawing.Color.Red;
                lblMessage.Text = deleted
                    ? "Product deactivated."
                    : "Product could not be deleted.";
                BindProducts();
            }
            catch
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Delete failed.";
            }
        }

        protected void btnSeed_Click(object sender, EventArgs e)
        {
            try
            {
                int count = client.EnsureMinimumCatalogue(20);
                lblMessage.ForeColor = System.Drawing.Color.DarkGreen;
                lblMessage.Text = "Catalogue now has " + count + " products.";
                BindProducts();
            }
            catch
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Seed failed. Ensure PhoneFitService is running.";
            }
        }
    }
}
