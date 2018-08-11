<%@ Page Title="View Topic" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewTopic.aspx.cs" Inherits="NephroNet.Accounts.Admin.ViewTopic" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--Body start--%>
    <div class="container">
        <br />
        <%--<h2><%: Title %></h2>--%>
        <div class="panel panel-default">
            <div class="panel-body">
                <%--Content start--%>
                <asp:Label ID="lblHeader" runat="server" Text="Header" Font-Bold="True"></asp:Label>
                <br />
                <%--Ajax start--%>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer> 
                <asp:UpdatePanel ID="upMessages" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblContents" runat="server" Text="Contents"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="Timer1" EventName="Tick" />
                            </Triggers>
                </asp:UpdatePanel>
                <%--Ajax end--%>
                <%--<asp:Label ID="lblContents" runat="server" Text="Contents"></asp:Label>--%>
                <br />
                <asp:Label ID="lblEntry" runat="server" Text="Message"></asp:Label>
                &nbsp;
                <style>
                    .content {
                        min-width: 100%;
                    }
                </style>
                <asp:TextBox ID="txtEntry" runat="server" Height="130px" Width="959px" TextMode="MultiLine" CssClass="content"></asp:TextBox>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblEntryError" runat="server" Text="Invalid input: Please type a description." Visible="false" ForeColor="red"></asp:Label>
                <br />
                <br />

                
                <asp:FileUpload ID="FileUpload1" runat="server" Width="385px" AllowMultiple="true" onchange="onInputChange(event)" class="btn btn-primary" />
                <div id='fileNames'></div>
                &nbsp;
                <asp:Label ID="lblImageError" runat="server" Text="Image" Visible="false" ForeColor="red"></asp:Label>
                <%--Submit--%><br />
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor="Green" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnSubmit_Click" />
                &nbsp;
                
                <%--Cancel button--%>    
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Go back" BackColor="red" Font-Bold="True" Font-Size="Medium" Height="34px" Width="140px" OnClick="btnCancel_Click" />
                <%--Error message--%>
                <br />
                <br />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
                <%--Content end--%>
                <%--Popup message--%>
                <script type="text/javascript">
                    function OpenPopup(site) { popup(site); }
                    // copied from http://www.dotnetfunda.com/codes/code419-code-to-open-popup-window-in-center-position-.aspx
                    function popup(url) {
                        var width = 500;
                        var height = 300;
                        var left = (screen.width - width) / 2;
                        var top = (screen.height - height) / 2;
                        var params = 'width=' + width + ', height=' + height;
                        params += ', top=' + top + ', left=' + left;
                        params += ', directories=no';
                        params += ', location=no';
                        params += ', menubar=no';
                        params += ', resizable=no';
                        params += ', scrollbars=no';
                        params += ', status=no';
                        params += ', toolbar=no';
                        newwin = window.open(url, 'windowname5', params);
                        if (window.focus) { newwin.focus() }
                        return false;
                    }
                </script>
                <script type="text/javascript">
                    function onInputChange(e) {
                        //alert('Just clicked upload!');
                        var res = "";
                        for (var i = 0; i < $('#<%= FileUpload1.ClientID %>').get(0).files.length; i++) {
                            res += $('#<%= FileUpload1.ClientID %>').get(0).files[i].name + "<br />";
                        }
                        $('#fileNames').html(res);
                    }
                </script>
                
            </div>
        </div>
    </div>
    <%--Body end--%>
</asp:Content>
