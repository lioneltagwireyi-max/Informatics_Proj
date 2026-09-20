<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="PhoneFit.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="section">
  <div class="container" style="max-width:720px;margin:40px auto;">
    <h1>Checkout</h1>
    <p>Confirm your order. PhoneFit applies tax (15%), free shipping from R1 000, and a 5% loyalty discount for returning customers.</p>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlPreview" runat="server">
      <div style="margin:24px 0;padding:20px;border:1px solid var(--rule);border-radius:var(--r);background:var(--paper);">
        <p><strong>Cart items:</strong> <asp:Literal ID="litItemCount" runat="server"></asp:Literal></p>
        <p><strong>Merchandise subtotal:</strong> R<asp:Literal ID="litSubtotal" runat="server"></asp:Literal></p>
        <p style="color:var(--fg-mute);font-size:13px;">Exact tax, shipping and loyalty amounts are calculated when you place the order.</p>
      </div>
      <asp:Button ID="btnPlaceOrder" runat="server" Text="Place order &amp; create invoice"
        CssClass="btn btn--indigo" OnClick="btnPlaceOrder_Click"
        OnClientClick="return confirm('Place this order now?');" />
      <a href="cart.aspx" style="margin-left:12px;">Back to cart</a>
    </asp:Panel>
  </div>
</section>
</asp:Content>
