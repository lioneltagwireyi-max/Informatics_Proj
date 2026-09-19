<%@ Page Title="Invoice" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="PhoneFit.Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="section">
  <div class="container" style="max-width:860px;margin:40px auto;">
    <h1>Invoice</h1>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlInvoice" runat="server" Visible="false">
      <div style="padding:24px;border:1px solid var(--rule);border-radius:var(--r);background:var(--paper);">
        <p><strong>Invoice #</strong> <asp:Literal ID="litOrderId" runat="server"></asp:Literal></p>
        <p><strong>Date:</strong> <asp:Literal ID="litOrderDate" runat="server"></asp:Literal></p>
        <p><strong>Status:</strong> <asp:Literal ID="litStatus" runat="server"></asp:Literal></p>
        <table style="width:100%;margin-top:18px;border-collapse:collapse;">
          <thead>
            <tr style="text-align:left;border-bottom:1px solid var(--rule);">
              <th>Item</th><th>Qty</th><th>Unit</th><th>Line</th>
            </tr>
          </thead>
          <tbody>
            <asp:Repeater ID="rptLines" runat="server">
              <ItemTemplate>
                <tr style="border-bottom:1px solid var(--rule);">
                  <td><%# Eval("ModelName") %><br /><span style="color:var(--fg-mute);font-size:12px;"><%# Eval("VariantDescription") %></span></td>
                  <td><%# Eval("Quantity") %></td>
                  <td>R<%# Eval("UnitPrice", "{0:N2}") %></td>
                  <td>R<%# Eval("LineTotal", "{0:N2}") %></td>
                </tr>
              </ItemTemplate>
            </asp:Repeater>
          </tbody>
        </table>
        <div style="margin-top:18px;">
          <p>Subtotal: R<asp:Literal ID="litSubtotal" runat="server"></asp:Literal></p>
          <p>Loyalty discount: −R<asp:Literal ID="litDiscount" runat="server"></asp:Literal></p>
          <p>Shipping: R<asp:Literal ID="litShipping" runat="server"></asp:Literal></p>
          <p>VAT (15%): R<asp:Literal ID="litTax" runat="server"></asp:Literal></p>
          <p><strong>Total: R<asp:Literal ID="litTotal" runat="server"></asp:Literal></strong></p>
          <p style="color:var(--fg-mute);font-size:13px;"><asp:Literal ID="litNotes" runat="server"></asp:Literal></p>
        </div>
      </div>
      <p style="margin-top:16px;"><a href="Invoices.aspx">View previous invoices</a> · <a href="shop.aspx">Continue shopping</a></p>
    </asp:Panel>
  </div>
</section>
</asp:Content>
