﻿<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="NephroNet.Accounts.Admin.Contact" %>
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
    
    
        

    <address>
        <strong>Support:</strong>   <a href="mailto:Saleh.Alsyefi@gmail.com">Saleh.Alsyefi@gmail.com</a><br />
        <strong>Support:</strong>   <a href="mailto:Alsyefi.Saleh@uwlax.edu">Alsyefi.Saleh@uwlax.edu</a><br />
    </address>

            </div>
            </div>
        </div>
</asp:Content>
