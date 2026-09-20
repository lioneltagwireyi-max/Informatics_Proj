<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs" Inherits="PhoneFit.Invoices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="section">
  <div class="container">
    <h1>Previous invoices</h1>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
      <p>No invoices yet. Complete a checkout to create one.</p>
    </asp:Panel>
    <asp:Repeater ID="rptOrders" runat="server">
      <HeaderTemplate>
        <table style="width:100%;border-collapse:collapse;background:var(--paper);border:1px solid var(--rule);">
          <thead>
            <tr style="text-align:left;border-bottom:1px solid var(--rule);">
              <th style="padding:12px;">Order</th>
              <th style="padding:12px;">Date</th>
              <th style="padding:12px;">Status</th>
              <th style="padding:12px;">Total</th>
              <th style="padding:12px;"></th>
            </tr>
          </thead>
          <tbody>
      </HeaderTemplate>
      <ItemTemplate>
        <tr style="border-bottom:1px solid var(--rule);">
          <td style="padding:12px;">#<%# Eval("OrderID") %></td>
          <td style="padding:12px;"><%# Eval("OrderDate", "{0:dd MMM yyyy HH:mm}") %></td>
          <td style="padding:12px;"><%# Eval("OrderStatus") %></td>
          <td style="padding:12px;">R<%# Eval("TotalAmount", "{0:N2}") %></td>
          <td style="padding:12px;"><a href='Invoice.aspx?id=<%# Eval("OrderID") %>'>View invoice</a></td>
        </tr>
      </ItemTemplate>
      <FooterTemplate></tbody></table></FooterTemplate>
    </asp:Repeater>
  </div>
</section>
</asp:Content>
