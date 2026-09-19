<%@ Page Title="Reports"
    Language="C#"
    MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs"
    Inherits="PhoneFit.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
    .report-filters {
      display: flex;
      flex-wrap: wrap;
      gap: 16px;
      align-items: end;
      margin-bottom: 28px;
      padding: 20px;
      background: var(--paper);
      border: 1px solid var(--rule);
      border-radius: var(--r);
    }
    .report-filters label {
      display: block;
      font-family: var(--ff-mono);
      font-size: 11px;
      text-transform: uppercase;
      letter-spacing: 0.06em;
      color: var(--fg-mute);
      margin-bottom: 6px;
    }
    .report-metrics {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 18px;
      margin-bottom: 28px;
    }
    .report-card {
      padding: 22px;
      background: var(--paper);
      border: 1px solid var(--rule);
      border-radius: var(--r);
    }
    .report-card .label {
      font-family: var(--ff-mono);
      font-size: 12px;
      color: var(--fg-mute);
      text-transform: uppercase;
      letter-spacing: 0.08em;
    }
    .report-card .value {
      margin-top: 8px;
      font-family: var(--ff-display);
      font-size: 28px;
      font-weight: 700;
      color: var(--ink);
    }
    .report-table {
      width: 100%;
      border-collapse: collapse;
      background: var(--paper);
      border: 1px solid var(--rule);
      border-radius: var(--r);
      overflow: hidden;
    }
    .report-table th,
    .report-table td {
      padding: 12px 14px;
      text-align: left;
      border-bottom: 1px solid var(--rule);
      font-size: 14px;
    }
    .report-table th {
      font-family: var(--ff-mono);
      font-size: 11px;
      text-transform: uppercase;
      letter-spacing: 0.07em;
      color: var(--fg-mute);
      background: var(--bg);
    }
    @media (max-width: 760px) {
      .report-metrics { grid-template-columns: 1fr; }
    }
  </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <main id="main">
    <section class="page-head">
      <div class="container">
        <div class="crumbs">
          <a href="Home.aspx">Home</a>
          <span class="sep">›</span>
          <a href="Manager.aspx">Admin</a>
          <span class="sep">›</span>
          <span>Reports</span>
        </div>
        <h1>Sales &amp; user reports</h1>
        <p>
          Filter by date range to review products sold, stock on hand,
          registrations per day, and other store metrics.
        </p>
      </div>
    </section>

    <section class="section">
      <div class="container">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

        <div class="report-filters">
          <div>
            <label for="<%= txtFrom.ClientID %>">From date</label>
            <asp:TextBox ID="txtFrom" runat="server" TextMode="Date"></asp:TextBox>
          </div>
          <div>
            <label for="<%= txtTo.ClientID %>">To date</label>
            <asp:TextBox ID="txtTo" runat="server" TextMode="Date"></asp:TextBox>
          </div>
          <div>
            <asp:Button ID="btnApplyFilter" runat="server" Text="Apply filter"
              CssClass="btn btn--indigo" OnClick="btnApplyFilter_Click" />
          </div>
          <div>
            <asp:Button ID="btnLast30" runat="server" Text="Last 30 days"
              CssClass="btn" OnClick="btnLast30_Click" CausesValidation="false" />
          </div>
        </div>

        <div class="report-metrics">
          <div class="report-card">
            <div class="label">Different products sold</div>
            <div class="value"><asp:Label ID="lblProductsSold" runat="server" Text="0"></asp:Label></div>
          </div>
          <div class="report-card">
            <div class="label">Registered users in range</div>
            <div class="value"><asp:Label ID="lblUsersInRange" runat="server" Text="0"></asp:Label></div>
          </div>
          <div class="report-card">
            <div class="label">Orders in range</div>
            <div class="value"><asp:Label ID="lblOrders" runat="server" Text="0"></asp:Label></div>
          </div>
          <div class="report-card">
            <div class="label">Revenue in range</div>
            <div class="value">R<asp:Label ID="lblRevenue" runat="server" Text="0.00"></asp:Label></div>
          </div>
          <div class="report-card">
            <div class="label">Active customer accounts</div>
            <div class="value"><asp:Label ID="lblActiveCustomers" runat="server" Text="0"></asp:Label></div>
          </div>
          <div class="report-card">
            <div class="label">Total registered users</div>
            <div class="value"><asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></div>
          </div>
        </div>

        <div class="section-head">
          <h2>Stock on hand (products sold in range)</h2>
        </div>
        <asp:Panel ID="pnlNoStock" runat="server" Visible="false">
          <p>No sold products in this date range.</p>
        </asp:Panel>
        <asp:Repeater ID="rptStock" runat="server">
          <HeaderTemplate>
            <table class="report-table">
              <thead>
                <tr>
                  <th>Model</th>
                  <th>Variant</th>
                  <th>Stock on hand</th>
                </tr>
              </thead>
              <tbody>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td><%# Eval("ModelName") %></td>
              <td><%# Eval("VariantDescription") %></td>
              <td><%# Eval("StockQuantity") %></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate></tbody></table></FooterTemplate>
        </asp:Repeater>

        <div class="section-head" style="margin-top:32px;">
          <h2>Registered users per day</h2>
        </div>
        <asp:Panel ID="pnlNoUsersDay" runat="server" Visible="false">
          <p>No registrations in this date range.</p>
        </asp:Panel>
        <asp:Repeater ID="rptUsersPerDay" runat="server">
          <HeaderTemplate>
            <table class="report-table">
              <thead>
                <tr>
                  <th>Day</th>
                  <th>New registrations</th>
                </tr>
              </thead>
              <tbody>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td><%# Eval("Day", "{0:dd MMM yyyy}") %></td>
              <td><%# Eval("UserCount") %></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate></tbody></table></FooterTemplate>
        </asp:Repeater>

        <p style="margin-top:24px;">
          <a href="Manager.aspx">← User management</a>
          ·
          <a href="CustomerActivity.aspx">Customer activity stats</a>
        </p>
      </div>
    </section>
  </main>
</asp:Content>
