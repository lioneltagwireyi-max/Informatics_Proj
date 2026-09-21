using System;
using System.Linq;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class product : System.Web.UI.Page
    {
        Service1Client client = new Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            int selectedID;

            if (!int.TryParse(Request.QueryString["id"], out selectedID))
            {
                Response.Redirect("shop.aspx");
                return;
            }

            try
            {
                // Load the catalogue model first so a bad id redirects cleanly
                // before variants / specs bind.
                if (!IsPostBack)
                {
                    PhoneCatalogue selectedPhone = client.GetPhoneByID(selectedID);

                    if (selectedPhone == null)
                    {
                        Response.Redirect("shop.aspx");
                        return;
                    }

                    imgMainPhone.ImageUrl = selectedPhone.ImagePath;
                    lblModelName.Text = selectedPhone.ModelName;
                    lblDescription.Text = selectedPhone.Description;
                    lblBrandName.Text = selectedPhone.BrandName;
                    lblStartingPrice.Text = selectedPhone.StartingPrice.ToString();
                    lblStockQuantity.Text = selectedPhone.StockQuantity.ToString();

                    ActivityTracker.LogCustomerAction(
                        ActivityTracker.ActionPageView,
                        "product.aspx",
                        "Viewed phone model " + selectedID);
                }

                VariantInfo[] variants = client.GetVariantsByPhoneID(selectedID);
                if (variants == null)
                {
                    variants = new VariantInfo[0];
                }

                rptVariants.DataSource = variants;
                rptVariants.DataBind();

                SpecificationInfo specification = client.GetSpecificationByPhoneID(selectedID);
                BindSpecification(specification);
            }
            catch (EndpointNotFoundException)
            {
                ShowServiceError(
                    "PhoneFitService is not running. Start both PhoneFit and PhoneFitService "
                    + "(endpoint http://localhost:51194/Service1.svc), then reopen this page.");
            }
            catch (CommunicationException ex)
            {
                ShowServiceError(
                    "Could not load phone details from the service. "
                    + "Confirm PhoneFitService is running and rebuild after the latest pull. "
                    + "(" + ex.GetType().Name + ")");
            }
            catch (TimeoutException)
            {
                ShowServiceError("The phone details request timed out. Retry after PhoneFitService is responding.");
            }
        }

        private void BindSpecification(SpecificationInfo specification)
        {
            if (specification == null)
            {
                return;
            }

            lblProcessor.Text = specification.Processor ?? string.Empty;

            string screenSize = specification.ScreenSize.HasValue
                ? specification.ScreenSize.Value.ToString().Replace(",", ".")
                : "—";
            lblDisplay.Text = screenSize + " inch " + (specification.ScreenType ?? string.Empty);

            lblRefreshRate.Text = specification.RefreshRate.HasValue
                ? specification.RefreshRate.Value.ToString() + " Hz"
                : "—";
            lblBattery.Text = specification.BatteryCapacity.HasValue
                ? specification.BatteryCapacity.Value.ToString() + " mAh"
                : "—";
            lblRearCamera.Text = specification.RearCameraMP.HasValue
                ? specification.RearCameraMP.Value.ToString().Replace(",", ".") + " MP"
                : "—";
            lblFrontCamera.Text = specification.FrontCameraMP.HasValue
                ? specification.FrontCameraMP.Value.ToString().Replace(",", ".") + " MP"
                : "—";

            lblSupports5G.Text = specification.Supports5G ? "Yes" : "No";
            lblDualSIM.Text = specification.DualSIM ? "Yes" : "No";
            lblExpandableStorage.Text = specification.ExpandableStorage ? "Yes" : "No";
            lblWaterResistance.Text = specification.WaterResistance ?? string.Empty;
        }

        private void ShowServiceError(string message)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = message;
        }

        protected void rptVariants_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectVariant")
            {
                int variantID = Convert.ToInt32(e.CommandArgument);

                int phoneModelID;

                if (!int.TryParse(
                    Request.QueryString["id"],
                    out phoneModelID))
                {
                    Response.Redirect("shop.aspx");
                    return;
                }

                VariantInfo[] variants = client.GetVariantsByPhoneID(phoneModelID);
                if (variants == null)
                {
                    return;
                }

                VariantInfo selectedVariant =
                    variants.FirstOrDefault(
                        variant => variant.VariantID == variantID
                    );

                if (selectedVariant != null)
                {
                    hfSelectedVariantID.Value = selectedVariant.VariantID.ToString();

                    lblStartingPrice.Text = selectedVariant.Price.ToString();

                    lblStockQuantity.Text = selectedVariant.StockQuantity.ToString();
                }
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || !AppRoles.IsCustomerRole(Session["RoleName"] as string))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = Session["UserID"] == null
                    ? "Please log in before adding a phone to your cart."
                    : "Only valid customer accounts can add products to the cart.";
                return;
            }

            int userID = Convert.ToInt32(Session["UserID"]);

            Page.Validate("CartGroup");
            if (!Page.IsValid)
            {
                return;
            }

            int variantID;
            if (!int.TryParse(hfSelectedVariantID.Value, out variantID))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please select a phone variant first.";
                return;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid quantity.";
                return;
            }

            bool phoneAdded = client.AddToCart(userID, variantID, quantity);

            if (phoneAdded)
            {
                ActivityTracker.LogCustomerAction(
                    ActivityTracker.ActionAddToCart,
                    "product.aspx",
                    "Variant " + variantID + " qty " + quantity);

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "The selected phone was added to your cart.";
                txtQuantity.Text = "1";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "The item could not be added. Check the available stock.";
            }
        }
    }
}
