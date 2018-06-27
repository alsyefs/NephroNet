﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NephroNet.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>
        Login
    </h3>
    <br />
    <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
    &nbsp;&nbsp;
    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblUsernameError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
    &nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtPassword" runat="server" type="password"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    &nbsp;
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
    <br />


</asp:Content>
