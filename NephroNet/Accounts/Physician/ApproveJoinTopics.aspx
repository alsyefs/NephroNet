﻿<%@ Page Title="Approve Topics" Language="C#" MasterPageFile="~/Physician.Master" AutoEventWireup="true" CodeBehind="ApproveJoinTopics.aspx.cs" Inherits="NephroNet.Accounts.Physician.ApproveJoinTopics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%--Body start--%>
    <div class="container">
        <br />
        <h2><%: Title %></h2>
        <div class="panel panel-default">
            <div class="panel-body">

                <%--Message to be displaied if there is nothing to show:--%>
                <asp:Label ID="lblMessage" runat="server" Text="There is nothing to display!" Visible="false" ForeColor="Red" Font-Size="Medium" Font-Bold="true"></asp:Label>
                <div id="table">
                <asp:GridView ID="grdTopics" runat="server" Width="100%" HorizontalAlign="Center" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" PageSize="20" AllowPaging="True" OnPageIndexChanging="grdTopics_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/Accounts/Physician/ReviewJoinTopic.aspx?id={0}" SortExpression="id" Text="Review" />
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
    <%--Body End--%>
</asp:Content>
