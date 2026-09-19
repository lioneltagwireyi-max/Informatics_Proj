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

            string loginResult = client.LoginUser(email, hashedPassword);

            if (loginResult == "Customer")
            {
                Response.Redirect("Home.aspx");
            }
            else if (loginResult == "Manager")
            {
                lblMessage.Text = "Manager login successful. Dashboard will be added soon.";
            }
            else if (loginResult == "Inactive")
            {
                lblMessage.Text = "This account has been disabled.";
            }
            else
            {
                lblMessage.Text = "The email address or password is incorrect.";
            }
        }

    }
}