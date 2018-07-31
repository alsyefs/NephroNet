<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Physician.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="NephroNet.Accounts.Physician.ChangePassword" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <br />
    <h2><%: Title %></h2>
        <div class="panel panel-default">
        <div class="panel-body">
            <%--Type the new password:--%>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblP1" runat="server" Text="Type new password" Font-Size="Medium"></asp:Label>
            &nbsp;
            <asp:TextBox ID="txtP1" runat="server" type="password" Font-Size="Medium"></asp:TextBox>
            &nbsp;
            <asp:Label ID="lblP1Error" runat="server" Text="P1 error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
            <%--Repeat the new password:--%>
            <br /><br />
            <asp:Label ID="lblP2" runat="server" Text="Repeat new password" Font-Size="Medium"></asp:Label>
            &nbsp;
            <asp:TextBox ID="txtP2" runat="server" type="password" Font-Size="Medium"></asp:TextBox>
            &nbsp;
            <asp:Label ID="lblP2Error" runat="server" Text="P1 error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
            <%--Submit--%>
                <br /><br />
               <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnSubmit_Click"/>
                &nbsp;
                
                <%--Cancel button--%>    
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Go back" BackColor="red" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnCancel_Click"  />
            <br />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
            <asp:Label ID="lblSuccess" runat="server" ForeColor="green" Text="Label" Visible="False"></asp:Label>

            </div>
            </div>
        </div>
</asp:Content>
