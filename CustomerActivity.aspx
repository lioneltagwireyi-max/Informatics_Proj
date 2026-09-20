<%@ Page Title="Customer Activity"
    Language="C#"
    MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true"
    CodeBehind="CustomerActivity.aspx.cs"
    Inherits="PhoneFit.CustomerActivity" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">

    <style>
        .manager-summary {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 18px;
            margin-bottom: 28px;
        }

        .manager-summary-card {
            padding: 22px;
            background: var(--paper);
            border: 1px solid var(--rule);
            border-radius: var(--r);
        }

        .manager-summary-card .label {
            font-family: var(--ff-mono);
            font-size: 12px;
            color: var(--fg-mute);
            text-transform: uppercase;
            letter-spacing: 0.08em;
        }

        .manager-summary-card .value {
            margin-top: 8px;
            font-family: var(--ff-display);
            font-size: 30px;
            font-weight: 700;
            color: var(--ink);
        }

        .activity-nav {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-bottom: 24px;
        }

        .activity-nav a {
            display: inline-block;
            padding: 10px 14px;
            border: 1px solid var(--rule);
            border-radius: var(--r);
            background: var(--paper);
            color: var(--ink);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
        }

        .activity-nav a[aria-current="page"] {
            border-color: var(--indigo);
            color: var(--indigo);
        }

        .user-table-wrapper {
            overflow-x: auto;
            background: var(--paper);
            border: 1px solid var(--rule);
            border-radius: var(--r);
            margin-bottom: 28px;
        }

        .user-table {
            width: 100%;
            border-collapse: collapse;
        }

        .user-table th,
        .user-table td {
            padding: 15px;
            text-align: left;
            border-bottom: 1px solid var(--rule);
        }

        .user-table th {
            font-family: var(--ff-mono);
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.07em;
            color: var(--fg-mute);
            background: var(--bg);
        }

        .user-table td {
            font-size: 14px;
        }

        .split-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 24px;
            margin-bottom: 28px;
        }

        @media (max-width: 900px) {
            .manager-summary,
            .split-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>

</asp:Content>

<asp:Content
    ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <main id="main">

        <section class="page-head">
            <div class="container">

                <div class="crumbs">
                    <a href="Home.aspx">Home</a>
                    <span class="sep">›</span>
                    <a href="Manager.aspx">User Management</a>
                    <span class="sep">›</span>
                    <span>Customer Activity</span>
                </div>

                <h1>Customer web activity</h1>

                <p>
                    Login, page, cart and checkout signals from customer sessions
                    over the last
                    <asp:Literal ID="litDayWindow" runat="server"></asp:Literal>
                    days.
                </p>

            </div>
        </section>

        <section class="section">
            <div class="container">

                <div class="activity-nav">
                    <a href="Manager.aspx">User Management</a>
                    <a href="CustomerActivity.aspx" aria-current="page">Customer Activity</a>
                </div>

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    ForeColor="Red">
                </asp:Label>

                <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
                    <div style="
                        padding: 40px;
                        text-align: center;
                        background: var(--paper);
                        border: 1px solid var(--rule);
                        border-radius: var(--r);
                        margin-bottom: 28px;">
                        No customer activity has been recorded yet.
                        Browse the shop while logged in as a Customer to generate events.
                    </div>
                </asp:Panel>

                <div class="manager-summary">

                    <div class="manager-summary-card">
                        <div class="label">Active customers</div>
                        <div class="value">
                            <asp:Label ID="lblActiveCustomers" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Customer logins</div>
                        <div class="value">
                            <asp:Label ID="lblLogins" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Events (window)</div>
                        <div class="value">
                            <asp:Label ID="lblEventsWindow" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Page views</div>
                        <div class="value">
                            <asp:Label ID="lblPageViews" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Add to cart</div>
                        <div class="value">
                            <asp:Label ID="lblAddToCart" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Checkout starts</div>
                        <div class="value">
                            <asp:Label ID="lblCheckouts" runat="server"></asp:Label>
                        </div>
                    </div>

                </div>

                <div class="split-grid">

                    <div>
                        <div class="section-head">
                            <h2>Top pages</h2>
                        </div>
                        <div class="user-table-wrapper">
                            <table class="user-table">
                                <thead>
                                    <tr>
                                        <th>Page</th>
                                        <th>Views</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptTopPages" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("Name") %></td>
                                                <td><%# Eval("Count") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <asp:Panel ID="pnlNoPages" runat="server" Visible="false">
                            <p style="color: var(--fg-mute);">No page views in this window.</p>
                        </asp:Panel>
                    </div>

                    <div>
                        <div class="section-head">
                            <h2>Top actions</h2>
                        </div>
                        <div class="user-table-wrapper">
                            <table class="user-table">
                                <thead>
                                    <tr>
                                        <th>Action</th>
                                        <th>Count</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptTopActions" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("Name") %></td>
                                                <td><%# Eval("Count") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <asp:Panel ID="pnlNoActions" runat="server" Visible="false">
                            <p style="color: var(--fg-mute);">No actions in this window.</p>
                        </asp:Panel>
                    </div>

                </div>

                <div class="section-head">
                    <h2>Recent customers</h2>
                </div>

                <div class="user-table-wrapper">
                    <table class="user-table">
                        <thead>
                            <tr>
                                <th>User ID</th>
                                <th>Events</th>
                                <th>Last action</th>
                                <th>Last page</th>
                                <th>Last seen</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptRecentCustomers" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("UserID") %></td>
                                        <td><%# Eval("EventCount") %></td>
                                        <td><%# Eval("LastAction") %></td>
                                        <td><%# Eval("LastPage") %></td>
                                        <td><%# Eval("LastActivityAt", "{0:dd MMM yyyy HH:mm}") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <asp:Panel ID="pnlNoCustomers" runat="server" Visible="false">
                    <p style="color: var(--fg-mute); margin-bottom: 28px;">
                        No recent customer activity.
                    </p>
                </asp:Panel>

                <div class="section-head">
                    <h2>Recent events</h2>
                    <span class="badge">Latest 25</span>
                </div>

                <div class="user-table-wrapper">
                    <table class="user-table">
                        <thead>
                            <tr>
                                <th>When</th>
                                <th>User ID</th>
                                <th>Action</th>
                                <th>Page</th>
                                <th>Detail</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptRecentEvents" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("OccurredAt", "{0:dd MMM yyyy HH:mm:ss}") %></td>
                                        <td><%# FormatUserId(Eval("UserID")) %></td>
                                        <td><%# Eval("ActionType") %></td>
                                        <td><%# Eval("PagePath") %></td>
                                        <td><%# Eval("Detail") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

            </div>
        </section>

    </main>

</asp:Content>
