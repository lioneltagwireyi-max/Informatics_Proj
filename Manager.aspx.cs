using System;
using System.Linq;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Manager : System.Web.UI.Page
    {
        Service1Client client =
            new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin area: require Admin (or legacy Manager) session role.
            string roleName = Session["RoleName"] as string;
            if (!AppRoles.IsAdminRole(roleName))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                DisplayUsers();
            }
        }

        private void DisplayUsers()
        {
            try
            {
                var users = client.GetUsers();

                if (users == null || !users.Any())
                {
                    pnlNoUsers.Visible = true;
                    pnlUsers.Visible = false;

                    lblTotalUsers.Text = "0";
                    lblActiveUsers.Text = "0";
                    lblInactiveUsers.Text = "0";

                    return;
                }

                pnlNoUsers.Visible = false;
                pnlUsers.Visible = true;

                rptUsers.DataSource = users;
                rptUsers.DataBind();

                lblTotalUsers.Text =
                    users.Count().ToString();

                lblActiveUsers.Text =
                    users.Count(user =>
                        user.UserIsActive).ToString();

                lblInactiveUsers.Text =
                    users.Count(user =>
                        !user.UserIsActive).ToString();
            }
            catch (Exception)
            {
                pnlNoUsers.Visible = true;
                pnlUsers.Visible = false;

                lblMessage.Text =
                    "The user information could not be loaded.";
            }
        }

        protected string DisplayPhoneNumber(object phoneNumber)
        {
            if (phoneNumber == null ||
                string.IsNullOrWhiteSpace(
                    phoneNumber.ToString()))
            {
                return "Not provided";
            }

            return phoneNumber.ToString();
        }

        protected string GetStatusText(object isActive)
        {
            bool active;

            if (!bool.TryParse(
                isActive.ToString(),
                out active))
            {
                return "Unknown";
            }

            return active ? "Active" : "Inactive";
        }

        protected string GetStatusClass(object isActive)
        {
            bool active;

            if (!bool.TryParse(
                isActive.ToString(),
                out active))
            {
                return "";
            }

            return active
                ? "status-active"
                : "status-inactive";
        }
    }
}