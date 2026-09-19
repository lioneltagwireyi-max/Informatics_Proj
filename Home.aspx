<%@ Page Title="Home" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PhoneFit.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main">
        <section class="page-head">
            <div class="container">
                <h1>PhoneFit</h1>
                <p>Browse smartphones and find a device that fits your needs.</p>
                <p style="margin-top: 18px;">
                    <a class="btn btn--indigo" href="shop.aspx">Shop phones</a>
                </p>
            </div>
        </section>
    </main>
</asp:Content>
