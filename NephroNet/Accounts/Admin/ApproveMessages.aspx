<%@ Page Title="Approve Messages" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApproveMessages.aspx.cs" Inherits="NephroNet.Accounts.Admin.ApproveMessages" %>
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

            <%--Message to be displaied if there is nothing to show:--%>
            <asp:Label ID="lblMessage" runat="server" Text="There is nothing to display!" Visible="false" ForeColor="Red" Font-Size="Medium" Font-Bold="true"></asp:Label>

            <div id="table">
            <asp:GridView ID="grdMessages" runat="server" Width="100%" HorizontalAlign="Center" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical"  PageSize ="20" AllowPaging="True" OnPageIndexChanging="grdMessages_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/Accounts/Admin/ReviewMessage.aspx?id={0}" SortExpression="id" Text="Review" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
                </div>
            </div>
            </div>
        </div>
</asp:Content>
