using System;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class ProductAdd : System.Web.UI.Page
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
                BindBrands();
            }
        }

        private void BindBrands()
        {
            try
            {
                BrandInfo[] brands = client.GetBrands();
                ddlBrand.Items.Clear();
                ddlBrand.Items.Add(new ListItem("— Select brand —", "0"));
                if (brands != null)
                {
                    foreach (BrandInfo brand in brands)
                    {
                        ddlBrand.Items.Add(new ListItem(brand.BrandName, brand.BrandID.ToString()));
                    }
                }
            }
            catch
            {
                lblMessage.Text = "Brands could not be loaded.";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModelName.Text)
                || string.IsNullOrWhiteSpace(txtOS.Text)
                || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                lblMessage.Text = "Model name, OS and description are required.";
                return;
            }

            int brandId;
            int.TryParse(ddlBrand.SelectedValue, out brandId);
            if (brandId <= 0 && string.IsNullOrWhiteSpace(txtNewBrand.Text))
            {
                lblMessage.Text = "Select a brand or enter a new brand name.";
                return;
            }

            int year, ram, storage, stock;
            decimal price;
            int.TryParse(txtYear.Text, out year);
            int.TryParse(txtRam.Text, out ram);
            int.TryParse(txtStorage.Text, out storage);
            int.TryParse(txtStock.Text, out stock);
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                lblMessage.Text = "Enter a valid price.";
                return;
            }

            var product = new ProductSaveInfo
            {
                BrandID = brandId,
                BrandName = txtNewBrand.Text.Trim(),
                ModelName = txtModelName.Text.Trim(),
                OperatingSystem = txtOS.Text.Trim(),
                ReleaseYear = year,
                Description = txtDescription.Text.Trim(),
                ImagePath = txtImagePath.Text.Trim(),
                IsActive = chkActive.Checked,
                RAMGB = ram,
                StorageGB = storage,
                Colour = txtColour.Text.Trim(),
                Price = price,
                StockQuantity = stock
            };

            try
            {
                int newId = client.AddProduct(product);
                if (newId <= 0)
                {
                    lblMessage.Text = "Product could not be added. Check brand and required fields.";
                    return;
                }

                Response.Redirect("Products.aspx?added=" + newId);
            }
            catch
            {
                lblMessage.Text = "Save failed. Ensure PhoneFitService is running.";
            }
        }
    }
}
