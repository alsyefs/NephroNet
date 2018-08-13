<%@ Page Title="" Language="C#" MasterPageFile="~/Physician.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="NephroNet.Accounts.Physician.Profile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--Body start--%>
    <div class="container">
        <br />
        <h2><%: Title %></h2>
        <div class="panel panel-default">
            <div class="panel-body">
                <%--Content start--%>
                <asp:Label ID="lblShortProfileInformation" runat="server" Text=" "></asp:Label>
                <%--Content end--%>
            </div>
        </div>
    </div>
    <%--Body end--%>
</asp:Content>
