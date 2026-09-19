using System;
using System.Web.UI.WebControls;
using PhoneFit.BackendServiceReference;

namespace PhoneFit
{
    public partial class ProductEdit : System.Web.UI.Page
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
                int id;
                if (!int.TryParse(Request.QueryString["id"], out id))
                {
                    lblMessage.Text = "Product not found.";
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    return;
                }

                BindBrands();
                LoadProduct(id);
            }
        }

        private void BindBrands()
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

        private void LoadProduct(int phoneModelId)
        {
            try
            {
                ProductAdminInfo product = client.GetProductForAdmin(phoneModelId);
                if (product == null)
                {
                    lblMessage.Text = "Product not found.";
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    return;
                }

                hfPhoneModelID.Value = product.PhoneModelID.ToString();
                hfVariantID.Value = product.VariantID.ToString();
                ListItem brandItem = ddlBrand.Items.FindByValue(product.BrandID.ToString());
                if (brandItem != null)
                {
                    ddlBrand.SelectedValue = product.BrandID.ToString();
                }
                txtModelName.Text = product.ModelName;
                txtOS.Text = product.OperatingSystem;
                txtYear.Text = product.ReleaseYear > 0 ? product.ReleaseYear.ToString() : string.Empty;
                txtDescription.Text = product.Description;
                txtImagePath.Text = product.ImagePath;
                txtRam.Text = product.RAMGB.ToString();
                txtStorage.Text = product.StorageGB.ToString();
                txtColour.Text = product.Colour;
                txtPrice.Text = product.StartingPrice.ToString("0.00");
                txtStock.Text = product.StockQuantity.ToString();
                chkActive.Checked = product.IsActive;
            }
            catch
            {
                lblMessage.Text = "Product could not be loaded.";
                btnSave.Enabled = false;
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

            int phoneModelId, variantId, brandId, year, ram, storage, stock;
            decimal price;
            int.TryParse(hfPhoneModelID.Value, out phoneModelId);
            int.TryParse(hfVariantID.Value, out variantId);
            int.TryParse(ddlBrand.SelectedValue, out brandId);
            int.TryParse(txtYear.Text, out year);
            int.TryParse(txtRam.Text, out ram);
            int.TryParse(txtStorage.Text, out storage);
            int.TryParse(txtStock.Text, out stock);
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                lblMessage.Text = "Enter a valid price.";
                return;
            }

            if (brandId <= 0 && string.IsNullOrWhiteSpace(txtNewBrand.Text))
            {
                lblMessage.Text = "Select a brand or enter a new brand name.";
                return;
            }

            var product = new ProductSaveInfo
            {
                PhoneModelID = phoneModelId,
                VariantID = variantId,
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
                bool ok = client.UpdateProduct(product);
                if (!ok)
                {
                    lblMessage.Text = "Update failed.";
                    return;
                }

                lblMessage.ForeColor = System.Drawing.Color.DarkGreen;
                lblMessage.Text = "Product updated.";
                LoadProduct(phoneModelId);
            }
            catch
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Update failed. Ensure PhoneFitService is running.";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int phoneModelId;
            int.TryParse(hfPhoneModelID.Value, out phoneModelId);
            try
            {
                if (client.DeleteProduct(phoneModelId))
                {
                    Response.Redirect("Products.aspx?deleted=1");
                }
                else
                {
                    lblMessage.Text = "Delete failed.";
                }
            }
            catch
            {
                lblMessage.Text = "Delete failed.";
            }
        }
    }
}
