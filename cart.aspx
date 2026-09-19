<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="PhoneFit.cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main id="main">

        <section class="page-head">
            <div class="container">
                <div class="crumbs">
                    <a href="Home.aspx">Home</a>
                    <span class="sep">›</span>
                    <span>Shopping cart</span>
                </div>

                <h1>Your cart</h1>

                <asp:Literal ID="litCartHeading" runat="server"></asp:Literal>
            </div>
        </section>

        <section class="section">
            <div class="container">

                <asp:Panel ID="pnlCartEmpty" runat="server" Visible="false">
                    <div style="padding: 60px 30px; text-align: center; background: var(--paper); border-radius: var(--r);">
                        <div style="font-size: 48px; margin-bottom: 18px;">🛒</div>

                        <h2>Your cart is empty</h2>

                        <p style="margin-bottom: 24px;">
                            Browse the PhoneFit catalogue and add a phone to your cart.
                        </p>

                        <a href="shop.aspx" class="btn btn--indigo">
                            Browse phones
                        </a>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlCartContent" runat="server">

                    <div class="cart-layout">

                        <div>

                            <asp:Label ID="lblMessage" runat="server"></asp:Label>

                            <div class="cart-list">

                                <asp:Repeater ID="rptCartItems" runat="server"
                                    OnItemCommand="rptCartItems_ItemCommand">

                                    <ItemTemplate>

                                        <article class="cart-row">

                                            <div class="pic">
                                                <asp:Image
                                                    ID="imgPhone"
                                                    runat="server"
                                                    ImageUrl='<%# Eval("ImagePath") %>'
                                                    AlternateText='<%# Eval("ModelName") %>' />
                                            </div>

                                            <div class="info">
                                                <div class="name">
                                                    <a href='<%# "product.aspx?id=" + Eval("PhoneModelID") %>'>
                                                        <%# Eval("ModelName") %>
                                                    </a>
                                                </div>

                                                <div class="variant">
                                                    <%# Eval("VariantDescription") %>
                                                </div>

                                                <div style="font-size: 12px; color: var(--fg-mute); margin-top: 5px;">
                                                    Unit price:
                                                    <%# Eval("UnitPrice", "{0:N2}") %>
                                                </div>
                                            </div>

                                            <div class="qty">

                                                <asp:LinkButton
                                                    ID="btnDecrease"
                                                    runat="server"
                                                    CssClass="qty-button"
                                                    CommandName="Decrease"
                                                    CommandArgument='<%# Eval("CartItemID") %>'
                                                    CausesValidation="false"
                                                    aria-label="Decrease quantity">
                                                    −
                                                </asp:LinkButton>

                                                <span style="min-width: 36px; text-align: center;">
                                                    <%# Eval("Quantity") %>
                                                </span>

                                                <asp:LinkButton
                                                    ID="btnIncrease"
                                                    runat="server"
                                                    CssClass="qty-button"
                                                    CommandName="Increase"
                                                    CommandArgument='<%# Eval("CartItemID") %>'
                                                    CausesValidation="false"
                                                    aria-label="Increase quantity">
                                                    +
                                                </asp:LinkButton>

                                            </div>

                                            <span class="subtotal">
                                                <%# Eval("LineTotal", "{0:N2}") %>
                                            </span>

                                            <asp:LinkButton
                                                ID="btnRemove"
                                                runat="server"
                                                CssClass="remove"
                                                CommandName="Remove"
                                                CommandArgument='<%# Eval("CartItemID") %>'
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Remove this phone from your cart?');"
                                                aria-label="Remove product">
                                                ✕
                                            </asp:LinkButton>

                                        </article>

                                    </ItemTemplate>

                                </asp:Repeater>

                            </div>

                            <div style="margin-top: var(--s5); display: flex; gap: var(--s3); flex-wrap: wrap;">

                                <a href="shop.aspx" class="btn btn--ghost">
                                    ← Continue shopping
                                </a>

                                <asp:Button
                                    ID="btnClearCart"
                                    runat="server"
                                    Text="Clear cart"
                                    CssClass="btn btn--ghost"
                                    OnClick="btnClearCart_Click"
                                    OnClientClick="return confirm('Remove all products from your cart?');" />

                            </div>

                            <div style="margin-top: var(--s7); display: grid; grid-template-columns: repeat(3, 1fr); gap: var(--s4); padding: var(--s5); background: var(--bg); border-radius: var(--r);">

                                <div style="display: flex; align-items: center; gap: var(--s3);">
                                    <div style="width: 40px; height: 40px; background: var(--indigo-soft); color: var(--indigo); border-radius: 999px; display: grid; place-items: center; font-size: 18px;">
                                        ⚡
                                    </div>

                                    <div>
                                        <div style="font-family: var(--ff-display); font-weight: 700; font-size: var(--text-sm);">
                                            Fast delivery
                                        </div>

                                        <div style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute);">
                                            Delivery options at checkout
                                        </div>
                                    </div>
                                </div>

                                <div style="display: flex; align-items: center; gap: var(--s3);">
                                    <div style="width: 40px; height: 40px; background: var(--indigo-soft); color: var(--indigo); border-radius: 999px; display: grid; place-items: center; font-size: 18px;">
                                        ↺
                                    </div>

                                    <div>
                                        <div style="font-family: var(--ff-display); font-weight: 700; font-size: var(--text-sm);">
                                            Secure shopping
                                        </div>

                                        <div style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute);">
                                            Your cart is safely stored
                                        </div>
                                    </div>
                                </div>

                                <div style="display: flex; align-items: center; gap: var(--s3);">
                                    <div style="width: 40px; height: 40px; background: var(--indigo-soft); color: var(--indigo); border-radius: 999px; display: grid; place-items: center; font-size: 18px;">
                                        ★
                                    </div>

                                    <div>
                                        <div style="font-family: var(--ff-display); font-weight: 700; font-size: var(--text-sm);">
                                            Quality phones
                                        </div>

                                        <div style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute);">
                                            Compare verified specifications
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <aside class="cart-summary">

                            <h3>Order summary</h3>

                            <div class="cart-line">
                                <span>
                                    Subtotal ·
                                    <asp:Label ID="lblItemCount" runat="server"></asp:Label>
                                </span>

                                <span style="font-family: var(--ff-display); font-weight: 600; color: var(--ink);">
                                    <asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                                </span>
                            </div>

                            <div class="cart-line">
                                <span>Shipping</span>

                                <span style="color: var(--emerald); font-weight: 600;">
                                    Free
                                </span>
                            </div>

                            <div class="cart-line is-total">
                                <span>Total</span>
                                <span>
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </span>
                            </div>

                            <asp:Button
                                ID="btnCheckout"
                                runat="server"
                                Text="Proceed to checkout →"
                                CssClass="btn btn--indigo btn--block"
                                OnClick="btnCheckout_Click" />

                            <p style="margin-top: var(--s5); font-size: 11px; font-family: var(--ff-mono); color: var(--fg-mute); text-align: center; line-height: 1.6;">
                                Secure PhoneFit checkout. Stock will be verified before the order is completed.
                            </p>

                        </aside>

                    </div>

                </asp:Panel>

            </div>
        </section>

    </main>

</asp:Content>