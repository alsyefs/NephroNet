<%@ Page Title="Review Message" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewMessage.aspx.cs" Inherits="NephroNet.Accounts.Admin.ReviewMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default" runat="server">
    <%--Page header start:--%>
    <a class="navbar-brand" runat="server" href="Home">Nephro Net</a>
    <ul class="nav navbar-nav">            
            <li><a runat="server" href="About">About</a></li>
            <li><a runat="server" href="Contact">Contact</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-right">                       
            <li><a runat="server" href="Notifications">Alerts</a></li>
                <li><a runat="server" href="~/Logout">Logout</a></li>
    </ul>
    <%--Page header end.--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%--Page body start:--%>
    <div class="container">
    <h2><%: Title %>:</h2>
        <div class="panel panel-default">
        <div class="panel-body">



             </div>
            </div>
        </div>
    <%--Page body end.--%>
</asp:Content>
