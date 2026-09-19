<%@ Page Title="Product Management"
    Language="C#"
    MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true"
    CodeBehind="Products.aspx.cs"
    Inherits="PhoneFit.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
    .product-admin-actions { display:flex; flex-wrap:wrap; gap:12px; margin:18px 0 24px; }
    .product-admin-table { width:100%; border-collapse:collapse; background:var(--paper); border:1px solid var(--rule); }
    .product-admin-table th, .product-admin-table td { padding:12px 14px; text-align:left; border-bottom:1px solid var(--rule); font-size:14px; }
    .product-admin-table th { font-family:var(--ff-mono); font-size:11px; text-transform:uppercase; letter-spacing:.07em; color:var(--fg-mute); background:var(--bg); }
    .status-pill { display:inline-block; padding:4px 10px; border-radius:999px; font-size:12px; font-weight:600; }
    .status-on { background:#dff7eb; color:#087443; }
    .status-off { background:#fde5e5; color:#a11a1a; }
  </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <main id="main">
    <section class="page-head">
      <div class="container">
        <div class="crumbs">
          <a href="Home.aspx">Home</a> <span class="sep">›</span>
          <a href="Manager.aspx">Admin</a> <span class="sep">›</span>
          <span>Products</span>
        </div>
        <h1>Product management</h1>
        <p>Add, edit, or remove phones from the PhoneFit catalogue. Soft-deleted products leave the shop but keep order history intact.</p>
      </div>
    </section>

    <section class="section">
      <div class="container">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>

        <div class="product-admin-actions">
          <a class="btn btn--indigo" href="ProductAdd.aspx">Add product</a>
          <asp:Button ID="btnSeed" runat="server" Text="Ensure ≥20 products"
            CssClass="btn" OnClick="btnSeed_Click"
            OnClientClick="return confirm('Seed missing demo phones until at least 20 products exist?');" />
          <asp:Label ID="lblCount" runat="server" style="align-self:center;color:var(--fg-mute);"></asp:Label>
        </div>

        <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
          <p>No products found. Add a product or run the catalogue seed.</p>
        </asp:Panel>

        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
          <HeaderTemplate>
            <table class="product-admin-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Brand</th>
                  <th>Model</th>
                  <th>OS</th>
                  <th>Price</th>
                  <th>Stock</th>
                  <th>Status</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td><%# Eval("PhoneModelID") %></td>
              <td><%# Eval("BrandName") %></td>
              <td><%# Eval("ModelName") %></td>
              <td><%# Eval("OperatingSystem") %></td>
              <td>R<%# Eval("StartingPrice", "{0:N2}") %></td>
              <td><%# Eval("StockQuantity") %></td>
              <td>
                <span class='<%# (bool)Eval("IsActive") ? "status-pill status-on" : "status-pill status-off" %>'>
                  <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                </span>
              </td>
              <td>
                <a href='ProductEdit.aspx?id=<%# Eval("PhoneModelID") %>'>Edit</a>
                &nbsp;|&nbsp;
                <asp:LinkButton ID="btnDelete" runat="server"
                  CommandName="DeleteProduct"
                  CommandArgument='<%# Eval("PhoneModelID") %>'
                  Text="Delete"
                  OnClientClick="return confirm('Deactivate this product and its variants?');"
                  Visible='<%# (bool)Eval("IsActive") %>' />
              </td>
            </tr>
          </ItemTemplate>
          <FooterTemplate></tbody></table></FooterTemplate>
        </asp:Repeater>

        <p style="margin-top:24px;">
          <a href="Manager.aspx">← User management</a> ·
          <a href="Reports.aspx">Reports</a> ·
          <a href="ProductAdd.aspx">Add product</a>
        </p>
      </div>
    </section>
  </main>
</asp:Content>
