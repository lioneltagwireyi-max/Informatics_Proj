<%@ Page Title="Shop Phones" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="shop.aspx.cs" Inherits="PhoneFit.shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="page-head">
      <div class="container">
        <div class="crumbs"><a href="Home.aspx">Home</a> <span class="sep">›</span> <span>Shop Phones</span>
        </div>
            <h1>Shop Smartphones</h1>
            <p>
                Browse PhoneFit's available smartphones and select a device
                to view its variants, price, stock and specifications.
            </p>
      </div>
    </section>

    <section class="section">
      <div class="container">

          <%--<aside class="filters" aria-label="Filters">
            <div class="filter-block">
              <h3>Category</h3>
              <label><input type="checkbox" checked> Smartphones <span class="ct">76</span></label>
              <label><input type="checkbox"> Laptops &amp; Desktops <span class="ct">42</span></label>
              <label><input type="checkbox"> Smart Watches <span class="ct">28</span></label>
              <label><input type="checkbox"> Cameras <span class="ct">42</span></label>
              <label><input type="checkbox"> Headphones &amp; Buds <span class="ct">35</span></label>
              <label><input type="checkbox"> Gaming <span class="ct">31</span></label>
              <label><input type="checkbox"> Accessories <span class="ct">112</span></label>
            </div>
            <div class="filter-block">
              <h3>Price range</h3>
              <div
                style="display:flex; justify-content:space-between; font-family:var(--ff-mono); font-size:11px; color:var(--fg-mute)">
                <span>R120</span><span>R3 400</span></div>
              <div class="range-bar" aria-hidden="true"></div>
              <div
                style="display:flex; justify-content:space-between; font-family:var(--ff-mono); font-size:var(--text-xs); font-weight:600; color:var(--ink); margin-top:var(--s2)">
                <span>R420</span><span>R2 380</span></div>
            </div>
            <div class="filter-block">
              <h3>Brand</h3>
              <label><input type="checkbox"> Apple <span class="ct">84</span></label>
              <label><input type="checkbox" checked> Samsung <span class="ct">72</span></label>
              <label><input type="checkbox"> Sony <span class="ct">36</span></label>
              <label><input type="checkbox"> Canon <span class="ct">22</span></label>
              <label><input type="checkbox"> HP <span class="ct">28</span></label>
              <label><input type="checkbox"> Huawei <span class="ct">19</span></label>
              <label><input type="checkbox"> Logitech <span class="ct">31</span></label>
            </div>
            <div class="filter-block">
              <h3>Rating</h3>
              <label><input type="checkbox"> ★★★★★ &amp; up <span class="ct">186</span></label>
              <label><input type="checkbox"> ★★★★☆ &amp; up <span class="ct">242</span></label>
              <label><input type="checkbox"> ★★★☆☆ &amp; up <span class="ct">296</span></label>
            </div>
            <div class="filter-block">
              <h3>Availability</h3>
              <label><input type="checkbox" checked> In stock <span class="ct">298</span></label>
              <label><input type="checkbox"> On sale <span class="ct">64</span></label>
              <label><input type="checkbox"> New arrivals <span class="ct">28</span></label>
            </div>
            <a href="#" class="btn btn--indigo btn--block">Apply filters</a>
          </aside>--%>

          <div>
            <div class="shop-toolbar" style="display:flex;flex-wrap:wrap;gap:16px;align-items:center;justify-content:space-between;">
              <asp:Label ID="lblPhoneCount" runat="server" CssClass="count" Text="Showing smartphones"></asp:Label>
              <div style="display:flex; gap:var(--s3); align-items:center">
                <span style="font-size:var(--text-xs); color:var(--fg-mute); font-family:var(--ff-mono)">SORT</span>
                <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                  <asp:ListItem Value="name_asc" Text="Name A–Z" Selected="True"></asp:ListItem>
                  <asp:ListItem Value="name_desc" Text="Name Z–A"></asp:ListItem>
                  <asp:ListItem Value="price_asc" Text="Price: low to high"></asp:ListItem>
                  <asp:ListItem Value="price_desc" Text="Price: high to low"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>

            <div class="shop-grid">
                <asp:Repeater ID="rptPhones" runat="server">
                    <ItemTemplate>
                        <article class="product-card">
                            <div class="img-wrap">
                                <span class="badge">Available</span>

                                <asp:Image ID="imgPhone" runat="server"
                                    ImageUrl='<%# Eval("ImagePath") %>'
                                    AlternateText='<%# Eval("ModelName") %>' />
                            </div>

                            <div class="stock">
                                <span class="dot"></span>
                                In stock · <%# Eval("StockQuantity") %> items
                            </div>

                            <div style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute);">
                                <%# Eval("BrandName") %>
                            </div>

                            <a href='<%# "product.aspx?id=" + Eval("PhoneModelID") %>'
                                class="name">
                                <%# Eval("ModelName") %>
                            </a>

                            <p>
                                <%# Eval("Description") %>
                            </p>

                            <div class="price">
                                <span class="now">
                                    From R<%# Eval("StartingPrice", "{0:N2}") %>
                                </span>
                            </div>

                            <a href='<%# "product.aspx?id=" + Eval("PhoneModelID") %>'
                                class="btn">
                                View Details &#8594;
                            </a>
                        </article>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            </div>

          </div>
    </section>

</asp:Content>