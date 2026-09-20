using System;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Reports : System.Web.UI.Page
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
                DateTime to = DateTime.Today;
                DateTime from = to.AddDays(-30);
                txtFrom.Text = from.ToString("yyyy-MM-dd");
                txtTo.Text = to.ToString("yyyy-MM-dd");
                LoadReport(from, to);
            }
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParse(txtFrom.Text, out from)
                || !DateTime.TryParse(txtTo.Text, out to))
            {
                lblMessage.Text = "Enter valid From and To dates.";
                return;
            }

            if (to < from)
            {
                lblMessage.Text = "To date must be on or after From date.";
                return;
            }

            lblMessage.Text = string.Empty;
            LoadReport(from, to);
        }

        protected void btnLast30_Click(object sender, EventArgs e)
        {
            DateTime to = DateTime.Today;
            DateTime from = to.AddDays(-30);
            txtFrom.Text = from.ToString("yyyy-MM-dd");
            txtTo.Text = to.ToString("yyyy-MM-dd");
            lblMessage.Text = string.Empty;
            LoadReport(from, to);
        }

        private void LoadReport(DateTime from, DateTime to)
        {
            try
            {
                ReportSummary report = client.GetReportSummary(from, to);
                if (report == null)
                {
                    lblMessage.Text = "No report data returned.";
                    return;
                }

                lblProductsSold.Text = report.DistinctProductsSold.ToString();
                lblUsersInRange.Text = report.RegisteredUsersInRange.ToString();
                lblOrders.Text = report.OrdersInRange.ToString();
                lblRevenue.Text = report.RevenueInRange.ToString("N2");
                lblActiveCustomers.Text = report.ActiveCustomerAccounts.ToString();
                lblTotalUsers.Text = report.TotalRegisteredUsers.ToString();

                StockOnHandInfo[] stock = report.StockOnHandForSoldProducts;
                bool hasStock = stock != null && stock.Length > 0;
                pnlNoStock.Visible = !hasStock;
                rptStock.Visible = hasStock;
                if (hasStock)
                {
                    rptStock.DataSource = stock;
                    rptStock.DataBind();
                }

                UsersPerDayInfo[] perDay = report.UsersRegisteredPerDay;
                bool hasDays = perDay != null && perDay.Length > 0;
                pnlNoUsersDay.Visible = !hasDays;
                rptUsersPerDay.Visible = hasDays;
                if (hasDays)
                {
                    rptUsersPerDay.DataSource = perDay;
                    rptUsersPerDay.DataBind();
                }
            }
            catch
            {
                lblMessage.Text = "Reports could not be loaded. Ensure PhoneFitService is running.";
            }
        }
    }
}
