using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class Login : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("LoginGroup");
            if (!Page.IsValid)
            {
                return;
            }

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            string hashedPassword = SecrecyHash.hashFunction(password);

            LoginInfo loginResult = client.LoginUser(email, hashedPassword);

            if (loginResult.LoginStatus == "Success")
            {
                Session["UserID"] = loginResult.UserID;
                Session["RoleName"] = loginResult.RoleName;

                // Admin (and legacy Manager) → Manager.aspx; Customer → Home.aspx.
                Response.Redirect(AppRoles.GetPostLoginRedirect(loginResult.RoleName));
                return;
            }

            if (loginResult.LoginStatus == "Inactive")
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "This account has been disabled.";
                return;
            }

            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "The email address or password is incorrect.";
        }
    }
}
