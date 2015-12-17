<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllUsers.aspx.cs" Inherits="SOS.Web.AllUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>All Users</title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="Content/Css/Popup.css" rel="stylesheet" />
    <link href="Content/Css/Spinner.css" rel="stylesheet" />
    <link href="Css/DownPane.css" rel="stylesheet" />
    <link href="Css/header.css" rel="stylesheet" />        
    <link href="Css/HelpEntities.css" rel="stylesheet" />

    <script type="text/javascript" src="https://code.jquery.com/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/Reports.js"></script>

     <script type="text/javascript">

        $(function () {

         var currentDate = new Date();

        $("#startDate").datepicker({
            changeMonth: true,
            changeYear: true,           
        });
        
        $("#endDate").datepicker({
            changeMonth: true,
            changeYear: true,
        });

        $("#startDate").datepicker(); //initialise
      
        $("#endDate").datepicker();//initialise
       
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div>
            <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid;">
            </div>
            <div class="fragment Default">
                <div class="header Banner">
                    <h2>Subscribed User List</h2>
                    <div style="text-align: right">
                        <a href="Settings.aspx" class="modal">Admin Features</a>
                    </div>
                </div>
                <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
                    <input type="hidden" id="AuthID" value="" runat="server" />
                    <input type="hidden" id="UType" value="" runat="server" />
                    <input type="hidden" id="GroupId" value="" runat="server" />

                    <table style="width: 100%; vertical-align: top;" border="0">
                        <tr>
                            <td style="align-items: center">
                                <asp:Label ID="Label1" runat="server" Text="User Name:"></asp:Label>
                                <asp:TextBox ID="txtSearchKey" runat="server"></asp:TextBox>
                                 &nbsp;<b>&nbsp; OR&nbsp;&nbsp</b>&nbsp;
                                 Start Date:&nbsp;
                                <asp:TextBox ID="startDate" runat="server" />                                                                                                                     
                                &nbsp;&nbsp;End Date:&nbsp;<asp:TextBox ID="endDate" runat="server" />                                                              
                                <asp:Button ID="btnSearch" runat="server" Font-Names="Calibri (Body)" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                            <td></td>
                        </tr>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvAllUsers" runat="server" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="Gray" ForeColor="White" />
                                    <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body)" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body)" />
                                    <Columns>
                                        <asp:BoundField DataField="N"
                                            HeaderText="Name" />
                                        <asp:BoundField DataField="M"
                                            HeaderText="MobileNumber" />
                                        <asp:BoundField DataField="E"
                                            HeaderText="Email" />
                                        <asp:BoundField DataField="EE"
                                            HeaderText="EnterpriseEmail" />
                                        <asp:BoundField DataField="J"
                                            HeaderText="Joined Date" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No records found for the selected filter criteria
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div id="divImage" style="display: none; align-content: center; margin-left: 640px; margin-top: 0px">
                                    <asp:Image ID="Image1" runat="server" src="Content/Images/load-mask.gif" />
                                    <p style="margin-bottom: 20px">Processing...</p>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
