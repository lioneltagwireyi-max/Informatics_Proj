<%@ Page Title="Home — PhoneFit" Language="C#" MasterPageFile="~/Phonefit.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PhoneFit.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <section class="hero-home">
    <div class="container">
      <div class="hero-kicker">PhoneFit storefront</div>
      <h1>PhoneFit</h1>
      <p class="lead">
        Browse smartphones that fit your budget and habits. Compare specs,
        check live stock, and check out with clear tax, shipping and loyalty rules.
      </p>
      <div class="hero-actions">
        <a class="btn btn--indigo" href="shop.aspx">Shop phones</a>
        <a class="btn btn--ghost" href="Login.aspx">Sign in</a>
      </div>
    </div>
  </section>

  <section class="section">
    <div class="container">
      <div class="section-head">
        <h2>Why shop with PhoneFit</h2>
        <span class="badge">Built for IFM2B10</span>
      </div>
      <div class="shop-grid">
        <article class="product-card">
          <div style="font-family:var(--ff-mono);font-size:11px;color:var(--fg-mute);letter-spacing:.08em;text-transform:uppercase;">Catalogue</div>
          <div class="name">Service-backed phones</div>
          <p>Every listing is loaded from PhoneFitService with brand, price, stock and variants.</p>
        </article>
        <article class="product-card">
          <div style="font-family:var(--ff-mono);font-size:11px;color:var(--fg-mute);letter-spacing:.08em;text-transform:uppercase;">Checkout</div>
          <div class="name">Transparent totals</div>
          <p>VAT at 15%, free shipping from R1 000, and a 5% loyalty discount for returning customers.</p>
        </article>
        <article class="product-card">
          <div style="font-family:var(--ff-mono);font-size:11px;color:var(--fg-mute);letter-spacing:.08em;text-transform:uppercase;">Accounts</div>
          <div class="name">Roles that matter</div>
          <p>Customers shop and view invoices. Admins manage users, products and reports.</p>
        </article>
      </div>
    </div>
  </section>
</asp:Content>
