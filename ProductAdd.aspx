<%@ Page Title="Add Product"
    Language="C#"
    MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true"
    CodeBehind="ProductAdd.aspx.cs"
    Inherits="PhoneFit.ProductAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
    .product-form { max-width:720px; margin:0 auto; display:grid; gap:14px; }
    .product-form label { display:block; font-family:var(--ff-mono); font-size:11px; text-transform:uppercase; letter-spacing:.06em; color:var(--fg-mute); margin-bottom:6px; }
    .product-form input, .product-form select, .product-form textarea { width:100%; padding:10px 12px; border:1px solid var(--rule); border-radius:var(--r); background:var(--paper); }
    .product-form .row2 { display:grid; grid-template-columns:1fr 1fr; gap:14px; }
    @media (max-width:640px) { .product-form .row2 { grid-template-columns:1fr; } }
  </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <main id="main">
    <section class="page-head">
      <div class="container">
        <div class="crumbs">
          <a href="Products.aspx">Products</a> <span class="sep">›</span> <span>Add</span>
        </div>
        <h1>Add product</h1>
        <p>Create a phone model and its default variant (price, stock, colour, memory).</p>
      </div>
    </section>
    <section class="section">
      <div class="container">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <div class="product-form">
          <div class="row2">
            <div>
              <label>Brand</label>
              <asp:DropDownList ID="ddlBrand" runat="server"></asp:DropDownList>
            </div>
            <div>
              <label>Or new brand name</label>
              <asp:TextBox ID="txtNewBrand" runat="server" placeholder="Leave blank to use dropdown"></asp:TextBox>
            </div>
          </div>
          <div>
            <label>Model name *</label>
            <asp:TextBox ID="txtModelName" runat="server"></asp:TextBox>
          </div>
          <div class="row2">
            <div>
              <label>Operating system *</label>
              <asp:TextBox ID="txtOS" runat="server" Text="Android"></asp:TextBox>
            </div>
            <div>
              <label>Release year</label>
              <asp:TextBox ID="txtYear" runat="server" TextMode="Number" Text="2024"></asp:TextBox>
            </div>
          </div>
          <div>
            <label>Description *</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
          </div>
          <div>
            <label>Image path</label>
            <asp:TextBox ID="txtImagePath" runat="server" Text="assets/img/phones/placeholder.png"></asp:TextBox>
          </div>
          <div class="row2">
            <div>
              <label>RAM (GB)</label>
              <asp:TextBox ID="txtRam" runat="server" TextMode="Number" Text="8"></asp:TextBox>
            </div>
            <div>
              <label>Storage (GB)</label>
              <asp:TextBox ID="txtStorage" runat="server" TextMode="Number" Text="128"></asp:TextBox>
            </div>
          </div>
          <div class="row2">
            <div>
              <label>Colour</label>
              <asp:TextBox ID="txtColour" runat="server" Text="Black"></asp:TextBox>
            </div>
            <div>
              <label>Price (R)</label>
              <asp:TextBox ID="txtPrice" runat="server" Text="4999.00"></asp:TextBox>
            </div>
          </div>
          <div class="row2">
            <div>
              <label>Stock quantity</label>
              <asp:TextBox ID="txtStock" runat="server" TextMode="Number" Text="20"></asp:TextBox>
            </div>
            <div>
              <label>Active</label>
              <asp:CheckBox ID="chkActive" runat="server" Checked="true" Text="Listed in shop" />
            </div>
          </div>
          <div>
            <asp:Button ID="btnSave" runat="server" Text="Save product" CssClass="btn btn--indigo" OnClick="btnSave_Click" />
            <a href="Products.aspx" style="margin-left:12px;">Cancel</a>
          </div>
        </div>
      </div>
    </section>
  </main>
</asp:Content>
