<%@ Page Title="Change your security questions and answers" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ChangeSecurityQuestions.aspx.cs" Inherits="NephroNet.Accounts.Admin.ChangeSecurityQuestions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <br />
        <h2><%: Title %></h2>
        <div class="panel panel-default">
            <div class="panel-body">

                <%--First question & answer:--%>
                <asp:Label ID="lblQ1" runat="server" Text="Question#1"></asp:Label>
                <asp:TextBox ID="txtQ1" runat="server"></asp:TextBox>
                <asp:Label ID="lblQ1Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <br />
                &nbsp;&nbsp;
                <asp:Label ID="lblA1" runat="server" Text="Answer#1"></asp:Label>
                <asp:TextBox ID="txtA1" runat="server"></asp:TextBox>
                <asp:Label ID="lblA1Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <br /><br />
                <%--Second Q&A:--%>
                <asp:Label ID="lblQ2" runat="server" Text="Question#2"></asp:Label>
                <asp:TextBox ID="txtQ2" runat="server"></asp:TextBox>
                <asp:Label ID="lblQ2Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <br />
                &nbsp;&nbsp;
                <asp:Label ID="lblA2" runat="server" Text="Answer#2"></asp:Label>
                <asp:TextBox ID="txtA2" runat="server"></asp:TextBox>
                <asp:Label ID="lblA2Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <br /><br />
                <%--Third Q&A:--%>
                <asp:Label ID="lblQ3" runat="server" Text="Question#3"></asp:Label>
                <asp:TextBox ID="txtQ3" runat="server"></asp:TextBox>
                <asp:Label ID="lblQ3Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <br />
                &nbsp;&nbsp;
                <asp:Label ID="lblA3" runat="server" Text="Answer#3"></asp:Label>
                <asp:TextBox ID="txtA3" runat="server"></asp:TextBox>
                <asp:Label ID="lblA3Error" runat="server" Text="Error" ForeColor="Red" Visible="False"></asp:Label>
                <%--Submit--%>
                <br /><br /><br />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnSubmit_Click" />
                <%--Clear all fields--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClearAll" runat="server" Text="Clear fields" BackColor="Yellow" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnClearAll_Click" />
                <%--Cancel--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" BackColor="Red" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnCancel_Click" />
                <br />
                <asp:Label ID="lblSuccess" runat="server" Text="Error" ForeColor="green" Visible="False"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
