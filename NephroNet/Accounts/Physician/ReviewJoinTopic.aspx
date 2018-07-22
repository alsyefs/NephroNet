﻿<%@ Page Title="Review Join Request" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewJoinTopic.aspx.cs" Inherits="NephroNet.Accounts.Physician.ReviewJoinTopic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default" runat="server">
    <%--Header start --%>
    <a class="navbar-brand" runat="server" href="Home">Nephro Net</a>
    <ul class="nav navbar-nav">
        <li><a runat="server" href="CreateTopic">Create Topic</a></li>
        <li><a runat="server" href="About">About</a></li>
        <li><a runat="server" href="Contact">Contact</a></li>
    </ul>
    <ul class="nav navbar-nav navbar-right">
        <li><a runat="server" href="Notifications">Alerts
            <asp:Label ID="lblAlerts" runat="server" Text=" (0)"></asp:Label>
        </a></li>
        <li><a runat="server" href="~/Logout">Logout</a></li>
    </ul>
    <%--Header end--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><%: Title %>:</h2>
        <div class="panel panel-default">
            <div class="panel-body">
                <%--Content start:--%>
                <asp:Label ID="lblRequesterInfo" runat="server" Text="Label"></asp:Label>
                <br />
                <asp:Label ID="lblTopicInformation" runat="server" Text="Label"></asp:Label>
                <%--Approve--%>
                <br />
                <br />
                <asp:Button ID="btnApprove" runat="server" Text="Accept" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnApprove_Click" />
                <%--Deny:--%>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDeny" runat="server" Text="Deny" BackColor="red" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnDeny_Click" />
                <%--Go back:--%>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Go Back" BackColor="yellow" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnCancel_Click" />
                <br />
                <br />
                &nbsp;
            <asp:Label ID="lblMessage" runat="server" ForeColor="green" Text="Label" Visible="False"></asp:Label>
                <%--Content end.--%>
            </div>
        </div>
    </div>
</asp:Content>
