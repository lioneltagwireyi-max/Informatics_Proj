<%@ Page Title="Register" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="PhoneFit.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="section">
        <div class="container">
            <div class="contact-form" style="max-width: 900px; margin: 50px auto;">
                <h2 style="font-size: var(--text-xl); margin-bottom: var(--s2);">
                    Create an Account
                </h2>

                <div class="field-row">
                    <div class="field">
                        <asp:Label ID="lblFirstName" runat="server"
                            AssociatedControlID="txtFirstName"
                            Text="First name">
                        </asp:Label>

                        <asp:TextBox ID="txtFirstName" runat="server"
                            placeholder="Mira">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"
                            ControlToValidate="txtFirstName"
                            ErrorMessage="First name is required."
                            ForeColor="Red"
                            Display="Dynamic"
                            ValidationGroup="RegistrationGroup">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="field">
                        <asp:Label ID="lblSurname" runat="server"
                            AssociatedControlID="txtSurname"
                            Text="Last name">
                        </asp:Label>

                        <asp:TextBox ID="txtSurname" runat="server"
                            placeholder="Kapoor">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfvSurname" runat="server"
                            ControlToValidate="txtSurname"
                            ErrorMessage="Surname is required."
                            ForeColor="Red"
                            Display="Dynamic"
                            ValidationGroup="RegistrationGroup">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="field-row">
                    <div class="field">
                        <asp:Label ID="lblEmail" runat="server"
                            AssociatedControlID="txtEmail"
                            Text="Email">
                        </asp:Label>

                        <asp:TextBox ID="txtEmail" runat="server"
                            TextMode="Email"
                            placeholder="you@example.com">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Email address is required."
                            ForeColor="Red"
                            Display="Dynamic"
                            ValidationGroup="RegistrationGroup">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="field">
                        <asp:Label ID="lblPhoneNumber" runat="server"
                            AssociatedControlID="txtPhoneNumber"
                            Text="Phone">
                        </asp:Label>

                        <asp:TextBox ID="txtPhoneNumber" runat="server"
                            TextMode="Phone"
                            placeholder="e.g. 0821234567">
                        </asp:TextBox>
                    </div>
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

                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Password is required."
                        ForeColor="Red"
                        Display="Dynamic"
                        ValidationGroup="RegistrationGroup">
                    </asp:RequiredFieldValidator>
                </div>

                <asp:Button ID="btnRegister" runat="server"
                    Text="Register Account"
                    CssClass="btn btn--indigo btn--block"
                    Style="padding: 16px; font-size: var(--text-base); margin-top: var(--s2);"
                    ValidationGroup="RegistrationGroup"
                    OnClick="btnRegister_Click" />

                <asp:Label ID="lblMessage" runat="server"
                    EnableViewState="false">
                </asp:Label>

                <p style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute); margin-top: var(--s4); text-align: center; line-height: 1.6;">
                    Already have an account?
                    <a runat="server" href="~/Login.aspx" style="color: var(--indigo);">Login here</a>.
                </p>

                <p style="font-family: var(--ff-mono); font-size: 11px; color: var(--fg-mute); margin-top: var(--s2); text-align: center; line-height: 1.6;">
                    By registering, you agree to our
                    <a href="#" style="color: var(--indigo);">privacy policy</a>.
                    We never sell your data or share it with third parties.
                </p>
            </div>
        </div>
    </section>
</asp:Content>
