﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="NephroNet.Accounts.Admin.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default" runat="server">
    <a class="navbar-brand" runat="server" href="Home">Nephro Net</a>
    <ul class="nav navbar-nav">            
            <li><a runat="server" href="About">About</a></li>
            <li><a runat="server" href="Contact">Contact</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-right">                               
                <li><a runat="server" href="~/Logout">Logout</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>