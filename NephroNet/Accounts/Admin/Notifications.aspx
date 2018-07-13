<%@ Page Title="Alerts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="NephroNet.Accounts.Admin.Notifications" %>
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

            <asp:Label ID="lblNewUsers" runat="server" Text="Label"></asp:Label> &nbsp;
            <asp:Button ID="btnNewUsers" runat="server" Text="Review Users" Width="190px" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" OnClick="btnNewUsers_Click" />
            <br />
            <asp:Label ID="lblNewTopics" runat="server" Text="Label"></asp:Label> &nbsp;
            <asp:Button ID="btnNewTopics" runat="server" Text="Review Topics" Width="190px" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" OnClick="btnNewTopics_Click" />
            <br />
            <asp:Label ID="lblNewMessages" runat="server" Text="Label"></asp:Label> &nbsp;
            <asp:Button ID="btnNewMessages" runat="server" Text="Review Messages" Width="190px" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" OnClick="btnNewMessages_Click"/>
            <br />

            </div>
            </div>
        </div>
</asp:Content>
