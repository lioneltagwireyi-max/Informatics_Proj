using System;
using System.Linq;

namespace PhoneFit
{
    public partial class CustomerActivity : System.Web.UI.Page
    {
        private const int DayWindow = 7;

        protected void Page_Load(object sender, EventArgs e)
        {
            string roleName = Session["RoleName"] as string;
            if (!AppRoles.IsAdminRole(roleName))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindStats();
            }
        }

        private void BindStats()
        {
            try
            {
                ActivityStats stats = ActivityTracker.GetStats(DayWindow, 25);

                litDayWindow.Text = stats.RecentDayWindow.ToString();
                lblActiveCustomers.Text = stats.ActiveCustomersLastDays.ToString();
                lblLogins.Text = stats.LoginCountLastDays.ToString();
                lblEventsWindow.Text = stats.EventsLastDays.ToString();
                lblPageViews.Text = stats.PageViewCountLastDays.ToString();
                lblAddToCart.Text = stats.AddToCartCountLastDays.ToString();
                lblCheckouts.Text = stats.CheckoutCountLastDays.ToString();

                pnlEmpty.Visible = stats.TotalEvents == 0;

                if (stats.TopPages != null && stats.TopPages.Any())
                {
                    pnlNoPages.Visible = false;
                    rptTopPages.DataSource = stats.TopPages;
                    rptTopPages.DataBind();
                }
                else
                {
                    pnlNoPages.Visible = true;
                }

                if (stats.TopActions != null && stats.TopActions.Any())
                {
                    pnlNoActions.Visible = false;
                    rptTopActions.DataSource = stats.TopActions;
                    rptTopActions.DataBind();
                }
                else
                {
                    pnlNoActions.Visible = true;
                }

                if (stats.RecentCustomers != null && stats.RecentCustomers.Any())
                {
                    pnlNoCustomers.Visible = false;
                    rptRecentCustomers.DataSource = stats.RecentCustomers;
                    rptRecentCustomers.DataBind();
                }
                else
                {
                    pnlNoCustomers.Visible = true;
                }

                rptRecentEvents.DataSource = stats.RecentEvents;
                rptRecentEvents.DataBind();
            }
            catch (Exception)
            {
                lblMessage.Text = "Customer activity statistics could not be loaded.";
                pnlEmpty.Visible = true;
            }
        }

        protected string FormatUserId(object userId)
        {
            if (userId == null || userId == DBNull.Value)
            {
                return "—";
            }

            return userId.ToString();
        }
    }
}
