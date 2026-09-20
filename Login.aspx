<%@ Page Title="Login — PhoneFit" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PhoneFit.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <section class="section">
    <div class="container">
      <div class="auth-shell">
        <div class="crumbs"><span>Account</span><span class="sep">›</span><span>Login</span></div>
        <h1>Welcome back</h1>
        <p class="sub">Sign in to PhoneFit to shop, track invoices, or open the admin workspace.</p>

        <div class="field">
          <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email"></asp:Label>
          <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="you@example.com"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvLoginEmail" runat="server"
            ControlToValidate="txtEmail"
            ErrorMessage="Email address is required."
            ForeColor="Red"
            Display="Dynamic"
            ValidationGroup="LoginGroup">
          </asp:RequiredFieldValidator>
        </div>

        <div class="field">
          <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Password"></asp:Label>
          <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvLoginPassword" runat="server"
            ControlToValidate="txtPassword"
            ErrorMessage="Password is required."
            ForeColor="Red"
            Display="Dynamic"
            ValidationGroup="LoginGroup">
          </asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="btnLogin" runat="server"
          Text="Sign in"
          CssClass="btn btn--indigo btn--block"
          OnClick="btnLogin_Click"
          ValidationGroup="LoginGroup" />

        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" style="display:block;margin-top:12px;"></asp:Label>

        <p style="font-family:var(--ff-mono);font-size:11px;color:var(--fg-mute);margin-top:1.25rem;text-align:center;line-height:1.6;">
          New to PhoneFit?
          <a runat="server" href="~/Registration.aspx" style="color:var(--teal-deep);">Create an account</a>
        </p>
      </div>
    </div>
  </section>
</asp:Content>
