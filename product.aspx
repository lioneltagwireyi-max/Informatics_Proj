<%@ Page Title="Phone Details" Language="C#" MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="PhoneFit.product" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
      <div class="crumbs" style="padding-top: var(--s5)"><a href="Home.aspx">Home</a> <span class="sep">›</span> <a
          href="shop.aspx">Shop Phones</a> <span class="sep">›</span> <span>Phone Details</span></div>

      <section class="product-detail">

        <div class="gallery">
          <figure class="gallery-main">
            <asp:Image ID="imgMainPhone" runat="server"
            AlternateText="Selected smartphone" />
          </figure>
        </div>

        <div class="pdp-info">
          <span class="pdp-cat">
            <asp:Label ID="lblBrandName" runat="server"></asp:Label>
          </span>
          <h1>
            <asp:Label ID="lblModelName" runat="server"></asp:Label>
          </h1>

            <div class="rating-row">
                <span style="color: var(--emerald); display: inline-flex; align-items: center; gap: 6px;">
                    <span style="width: 6px; height: 6px; background: var(--emerald); border-radius: 999px; display: inline-block;">
                    </span>

                    In stock ·
                    <asp:Label ID="lblStockQuantity" runat="server"></asp:Label>
                    items
                </span>
            </div>
          
          <p class="desc">
            <asp:Label ID="lblDescription" runat="server">
            </asp:Label>
          </p>

          <div class="price-row">
            <span class="now">
                R<asp:Label ID="lblStartingPrice" runat="server"></asp:Label>
            </span>
          </div>

          <div class="option-block">
            <div class="label">Available Variants</div>

            <div class="option-pills">
                <asp:Repeater ID="rptVariants" runat="server" OnItemCommand="rptVariants_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnSelectVariant" runat="server"
                            CssClass="variant-option"
                            CommandName="SelectVariant"
                            CommandArgument='<%# Eval("VariantID") %>'
                            CausesValidation="false">

                            <strong>
                                <%# Eval("RAMGB") %>GB RAM ·
                                <%# Eval("StorageGB") %>GB Storage
                            </strong>

                            <span><%# Eval("Colour") %></span>

                            <span>
                                R<%# Eval("Price", "{0:N2}") %>
                            </span>

                            <span>
                                <%# Eval("StockQuantity") %> in stock
                            </span>

                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:HiddenField ID="hfSelectedVariantID" runat="server" />
            </div>
        </div>

          <div class="pdp-cta">
            <div class="qty">
              <asp:TextBox
                  ID="txtQuantity"
                  runat="server"
                  Text="1"
                  TextMode="Number"
                  min="1"
                  aria-label="Quantity">
              </asp:TextBox>
            </div>
            <asp:Button
                ID="btnAddToCart"
                runat="server"
                Text="Add to cart →"
                CssClass="btn btn--indigo"
                Style="flex:1; min-width:160px"
                ValidationGroup="CartGroup"
                OnClick="btnAddToCart_Click" />
          </div>

          <div style="margin-top: 12px;">
            <asp:RequiredFieldValidator
                ID="rfvQuantity"
                runat="server"
                ControlToValidate="txtQuantity"
                ValidationGroup="CartGroup"
                ErrorMessage="Please enter a quantity."
                Display="Dynamic"
                ForeColor="Red">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator
                ID="rvQuantity"
                runat="server"
                ControlToValidate="txtQuantity"
                ValidationGroup="CartGroup"
                Type="Integer"
                MinimumValue="1"
                MaximumValue="99"
                ErrorMessage="Quantity must be between 1 and 99."
                Display="Dynamic"
                ForeColor="Red">
            </asp:RangeValidator>
          </div>

          <div class="pdp-features">
            <div class="pf"><span class="ic">⚡</span><span>Free shipping over R1 000</span></div>
            <div class="pf"><span class="ic">↺</span><span>30-day free returns</span></div>
            <div class="pf"><span class="ic">★</span><span>2-year limited warranty</span></div>
            <div class="pf"><span class="ic">✓</span><span>Authentic smartphone product</span></div>
          </div>
        </div>

      </section>

      <section class="section" style="padding-top: 0;">
            <h2 style="font-size: var(--text-2xl); margin-bottom: var(--s5);">
                Specifications
            </h2>

            <table style="width: 100%; border-collapse: collapse; font-size: var(--text-sm);">
                <tbody>
                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Processor</td>
                        <td class="spec-value"><asp:Label ID="lblProcessor" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Display</td>
                        <td class="spec-value"><asp:Label ID="lblDisplay" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Refresh rate</td>
                        <td class="spec-value"><asp:Label ID="lblRefreshRate" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Battery</td>
                        <td class="spec-value"><asp:Label ID="lblBattery" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Rear camera</td>
                        <td class="spec-value"><asp:Label ID="lblRearCamera" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Front camera</td>
                        <td class="spec-value"><asp:Label ID="lblFrontCamera" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">5G support</td>
                        <td class="spec-value"><asp:Label ID="lblSupports5G" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Dual SIM</td>
                        <td class="spec-value"><asp:Label ID="lblDualSIM" runat="server" /></td>
                    </tr>

                    <tr style="border-bottom: 1px solid var(--rule);">
                        <td class="spec-label">Expandable storage</td>
                        <td class="spec-value"><asp:Label ID="lblExpandableStorage" runat="server" /></td>
                    </tr>

                    <tr>
                        <td class="spec-label">Water resistance</td>
                        <td class="spec-value"><asp:Label ID="lblWaterResistance" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
        </section>

      
        <asp:Label ID="lblMessage" runat="server"
            ForeColor="Red">
        </asp:Label>
    </div>
</asp:Content>