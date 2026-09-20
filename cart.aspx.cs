using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class cart : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActivityTracker.LogCustomerAction(
                    ActivityTracker.ActionPageView,
                    "cart.aspx",
                    "Opened shopping cart");
                DisplayCart();
            }
        }

        private void DisplayCart()
        {
            // Check whether a customer is logged in
            if(Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                Response.Redirect(Session["UserID"] == null ? "Login.aspx" : "Home.aspx");
                return;
            }

            // Get the user id of the logged in user
            int userID = Convert.ToInt32(Session["UserID"]);

            // Get the cart items that belong to the logged in user
            CartItemInfo[] cartItems = client.GetCartItems(userID);

            // If the customer has no items inside the cart display the empty cart section
            if (cartItems == null || cartItems.Length == 0)
            {
                pnlCartEmpty.Visible = true;
                pnlCartContent.Visible = false;
                litCartHeading.Text = "<p>Your cart currently has no products.</p>";
                return;
            }

            litCartHeading.Text = "<p>Session UserID: " + userID + " | Cart items returned: " + cartItems.Length + "</p>";

            // The customer has items inside the cart - display the cart and hide the empty cart section
            pnlCartEmpty.Visible = false;
            pnlCartContent.Visible = true;

            // Bind the customers cart items to the repeater
            rptCartItems.DataSource = cartItems;
            rptCartItems.DataBind();

            int itemCount = 0;
            decimal subTotal = 0;

            // Calculate the total number of products and the subtotal
            foreach(CartItemInfo item in cartItems)
            {
                itemCount += item.Quantity;
                subTotal += item.LineTotal;
            }

            // Display the number of items in the cart heading
            if(itemCount == 1)
            {
                litCartHeading.Text = "<p style='color: var(--emerald); font-size: 18px;'>You have 1 item in your cart.</p>";
                lblItemCount.Text = "1 item";
            }
            else
            {
                litCartHeading.Text = "<p style='color: var(--emerald); font-size: 18px;'>You have " + itemCount + " items in your cart.</p>";
                lblItemCount.Text = itemCount + " items";
            }

            // Display the totals
            lblSubtotal.Text = "R" + subTotal.ToString("N2");
            lblTotal.Text = "R" + subTotal.ToString("N2");
        }


        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           // ensure a customer is logged in
           if(Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                Response.Redirect(Session["UserID"] == null ? "Login.aspx" : "Home.aspx");
                return;
            }

            // get the logged in customers id
            int userID = Convert.ToInt32(Session["UserID"]);

            // get the cartItemID from the button that was clicked
            int cartItemID;
            if (!int.TryParse(e.CommandArgument.ToString(), out cartItemID))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "The selected cart item is invalid.";
                return;
            }

            // increase quantity by 1
            if(e.CommandName == "Increase")
            {
                bool wasChanged = client.ChangeCartItemQuantity(userID, cartItemID, 1);

                if (!wasChanged)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "The quantity could not be increased. Check the available stock.";
                    return;
                }
            }

            // decrease the item quantity by 1
            else if(e.CommandName == "Decrease")
            {
                bool wasChanged = client.ChangeCartItemQuantity(userID, cartItemID, -1);

                if (!wasChanged)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "The quantity could not be decreased.";
                    return;
                }
            }

            else if(e.CommandName == "Remove")
            {
                bool wasRemoved = client.RemoveCartItem(userID, cartItemID);

                if (!wasRemoved)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "The product could not be removed from your cart.";
                    return;
                }
            }

            // reload the cart so the new quantities and totals are displayed
            DisplayCart();
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            // ensure a customer is logged in
            if (Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                Response.Redirect(Session["UserID"] == null ? "Login.aspx" : "Home.aspx");
                return;
            }

            // get the logged in customers id
            int userID = Convert.ToInt32(Session["UserID"]);

            // clear the users cart
            bool wasCleared = client.ClearCart(userID);

            if (wasCleared)
            {
                // reload the cart so the empty cart section displays
                DisplayCart();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "The cart could not be cleared.";
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                Response.Redirect(Session["UserID"] == null ? "Login.aspx" : "Home.aspx");
                return;
            }

            ActivityTracker.LogCustomerAction(
                ActivityTracker.ActionCheckout,
                "cart.aspx",
                "Checkout started");

            Response.Redirect("Checkout.aspx");
        }
    }
}