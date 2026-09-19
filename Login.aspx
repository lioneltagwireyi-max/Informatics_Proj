<%@ Page Title="Login" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PhoneFit.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="section">
        <div class="container">
            <div class="contact-form" style="max-width: 600px; margin: 50px auto; padding: 32px;">
                <h2 style="font-size: var(--text-xl); margin-bottom: var(--s2);">
                    Login to Your Account
                </h2>

                <div class="field">
                    <asp:Label ID="lblEmail" runat="server"
                        AssociatedControlID="txtEmail"
                        Text="Email">
                    </asp:Label>

                    <asp:TextBox ID="txtEmail" runat="server"
                        TextMode="Email"
                        placeholder="you@example.com">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvLoginEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Email address is required."
                        ForeColor="Red"
                        Display="Dynamic"
                        ValidationGroup="LoginGroup">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="field">
                    <asp:Label ID="lblPassword" runat="server"
                        AssociatedControlID="txtPassword"
                        Text="Password">
                    </asp:Label>

                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password"
                        placeholder="Enter your password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvLoginPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Password is required."
                        ForeColor="Red"
                        Display="Dynamic"
                        ValidationGroup="LoginGroup">
                    </asp:RequiredFieldValidator>

                    <a href="#" class="forgot-link">Forgot Password?</a>
                </div>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Login Account"
                    CssClass="btn btn--indigo btn--block"
                    Style="padding: 16px; font-size: var(--text-base); margin-top: var(--s2);"
                    OnClick="btnLogin_Click"
                    ValidationGroup="LoginGroup"/>

                <asp:Label ID="lblMessage" runat="server"
                    EnableViewState="false">
                </asp:Label>

                <p style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute); margin-top: var(--s4); text-align: center; line-height: 1.6;">
                    New to PhoneFit?
                    <a runat="server" href="~/Registration.aspx" style="color: var(--indigo);">
                        Create an account
                    </a>
                </p>
            </div>
        </div>
    </section>
</asp:Content>
