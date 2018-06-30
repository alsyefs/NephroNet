<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="NephroNet.Register" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="default" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Please fill the registration form</h1>

    <br /> <br />



    <%--First name--%>
    <asp:Label ID="lblFirstname" runat="server" Text="First name" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtFirstname" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblFirstnameError" runat="server" Text="Firstname error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Last name--%>
    <br /> <br />
    <asp:Label ID="lblLastname" runat="server" Text="Last name" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtLastname" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblLastnameError" runat="server" Text="Lastname error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Email--%>
    <br /> <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblEmail" runat="server" Text="Email" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtEmail" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblEmailError" runat="server" Text="Email error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--City--%>
    <br /> <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblCity" runat="server" Text="City" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtCity" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblCityError" runat="server" Text="City error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--State--%>
    <br /> <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblState" runat="server" Text="State" Font-Size="Medium"></asp:Label>
    &nbsp;

    <asp:DropDownList ID="drpStates" runat="server" Width="145px" Font-Size="Medium">
        <asp:ListItem>Select a state</asp:ListItem>
        <asp:ListItem>Alabama</asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
        <asp:ListItem></asp:ListItem>
    </asp:DropDownList>

    <%--<asp:TextBox ID="txtState" runat="server" Font-Size="Medium"></asp:TextBox>--%>
    &nbsp;
    <asp:Label ID="lblStateError" runat="server" Text="State error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Zip code--%>
    <br /> <br />
    &nbsp;
    <asp:Label ID="lblZip" runat="server" Text="Zip code" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtZip" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblZipError" runat="server" Text="Zip error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Address--%>
    <br /> <br />
    &nbsp;
    <asp:Label ID="lblAddress" runat="server" Text="Address" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtAddress" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblAddressError" runat="server" Text="Address error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Phone--%>
    <br /> <br />
    &nbsp;&nbsp;
    <asp:Label ID="lblPhone" runat="server" Text="Phone#" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:TextBox ID="txtPhone" runat="server" Font-Size="Medium"></asp:TextBox>
    &nbsp;
    <asp:Label ID="lblPhoneError" runat="server" Text="Phone error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>
     <%--Role--%>
    <br /> <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblRole" runat="server" Text="Role" Font-Size="Medium"></asp:Label>
    &nbsp;
    <asp:DropDownList ID="drpRole" runat="server" Width="145px" Font-Size="Medium">
        <asp:ListItem>Select a role</asp:ListItem>
        <asp:ListItem>Admin</asp:ListItem>
        <asp:ListItem>Physician</asp:ListItem>
        <asp:ListItem>Patient</asp:ListItem>
    </asp:DropDownList>
    &nbsp;
    <asp:Label ID="lblRoleError" runat="server" Text="Role error" ForeColor="Red" Visible="False" Font-Size="Medium"></asp:Label>



    <%--Submit button--%>
    <br /> <br />
    <br /> <br />
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnSubmit_Click" />
    &nbsp;
    <asp:Label ID="lblResult" runat="server" Text="Result" Visible="False" Font-Size="Medium"></asp:Label>
    <%--Cancel button--%>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" BackColor="red" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnCancel_Click" />


</asp:Content>
