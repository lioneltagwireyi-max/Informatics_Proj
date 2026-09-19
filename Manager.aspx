<%@ Page Title="User Management"
    Language="C#"
    MasterPageFile="~/Phonefit.Master"
    AutoEventWireup="true"
    CodeBehind="Manager.aspx.cs"
    Inherits="PhoneFit.Manager" %>

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

        .user-table-wrapper {
            overflow-x: auto;
            background: var(--paper);
            border: 1px solid var(--rule);
            border-radius: var(--r);
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

        .status-active,
        .status-inactive {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 999px;
            font-size: 12px;
            font-weight: 600;
        }

        .status-active {
            color: #087443;
            background: #dff7eb;
        }

        .status-inactive {
            color: #a11a1a;
            background: #fde5e5;
        }

        @media (max-width: 760px) {
            .manager-summary {
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
                    <span>User Management</span>
                </div>

                <h1>User Management</h1>

                <p>
                    View registered PhoneFit users,
                    assigned roles and account statuses.
                    Admin and Manager roles can access this page.
                </p>

                <p style="margin-top: 16px;">
                    <a href="Products.aspx"
                       style="color: var(--indigo); font-weight: 600; margin-right: 18px;">
                        Product management →
                    </a>
                    <a href="Reports.aspx"
                       style="color: var(--indigo); font-weight: 600; margin-right: 18px;">
                        Sales &amp; user reports →
                    </a>
                    <a href="CustomerActivity.aspx"
                       style="color: var(--indigo); font-weight: 600;">
                        Customer web activity statistics →
                    </a>
                </p>

            </div>
        </section>

        <section class="section">
            <div class="container">

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    ForeColor="Red">
                </asp:Label>

                <div class="manager-summary">

                    <div class="manager-summary-card">
                        <div class="label">Total users</div>

                        <div class="value">
                            <asp:Label
                                ID="lblTotalUsers"
                                runat="server">
                            </asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Active accounts</div>

                        <div class="value">
                            <asp:Label
                                ID="lblActiveUsers"
                                runat="server">
                            </asp:Label>
                        </div>
                    </div>

                    <div class="manager-summary-card">
                        <div class="label">Inactive accounts</div>

                        <div class="value">
                            <asp:Label
                                ID="lblInactiveUsers"
                                runat="server">
                            </asp:Label>
                        </div>
                    </div>

                </div>

                <div class="section-head">
                    <h2>Registered users</h2>

                    <span class="badge">
                        User Management in progress
                    </span>
                </div>

                <asp:Panel
                    ID="pnlNoUsers"
                    runat="server"
                    Visible="false">

                    <div style="
                        padding: 40px;
                        text-align: center;
                        background: var(--paper);
                        border-radius: var(--r);">

                        No registered users were found.

                    </div>

                </asp:Panel>

                <asp:Panel
                    ID="pnlUsers"
                    runat="server">

                    <div class="user-table-wrapper">

                        <table class="user-table">

                            <thead>
                                <tr>
                                    <th>User ID</th>
                                    <th>Full name</th>
                                    <th>Email address</th>
                                    <th>Phone number</th>
                                    <th>Role</th>
                                    <th>Status</th>
                                    <th>Date created</th>
                                </tr>
                            </thead>

                            <tbody>

                                <asp:Repeater
                                    ID="rptUsers"
                                    runat="server">

                                    <ItemTemplate>

                                        <tr>
                                            <td>
                                                <%# Eval("UserID") %>
                                            </td>

                                            <td>
                                                <%# Eval("FullName") %>
                                            </td>

                                            <td>
                                                <%# Eval("UserEmail") %>
                                            </td>

                                            <td>
                                                <%# DisplayPhoneNumber(
                                                    Eval("UserPhoneNumber")) %>
                                            </td>

                                            <td>
                                                <%# Eval("RoleName") %>
                                            </td>

                                            <td>
                                                <span class='<%#
                                                    GetStatusClass(
                                                        Eval("UserIsActive")) %>'>

                                                    <%# GetStatusText(
                                                        Eval("UserIsActive")) %>

                                                </span>
                                            </td>

                                            <td>
                                                <%# Eval(
                                                    "UserAccountCreated",
                                                    "{0:dd MMM yyyy HH:mm}") %>
                                            </td>
                                        </tr>

                                    </ItemTemplate>

                                </asp:Repeater>

                            </tbody>

                        </table>

                    </div>

                </asp:Panel>

                <div style="
                    margin-top: 24px;
                    padding: 18px;
                    background: var(--bg);
                    border-radius: var(--r);">

                    <strong>Development status:</strong>

                    Registration, login, password hashing,
                    account roles and active-account validation
                    are implemented. Account editing and activation
                    controls are planned for the next phase.

                </div>

            </div>
        </section>

    </main>

</asp:Content>