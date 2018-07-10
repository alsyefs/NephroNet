﻿<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NephroNet.Accounts.Patient.About" %>
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
    <div class="container">
    <h2><%: Title %>:</h2>
        <div class="panel panel-default">
        <div class="panel-body">
        
    <h3>Nephrology social network</h3>
    <p>This website serves as a social network for people who need to communicate with others having the same medical situations to find help.</p>
    </div>
            </div>
        </div>
</asp:Content>
