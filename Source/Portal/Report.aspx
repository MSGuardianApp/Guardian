<%@ Page Async="true" AsyncTimeout="1" Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="SOS.Web.Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Guardian Report</title>

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="Content/Css/Popup.css" rel="stylesheet" />
    <link href="Content/Css/Spinner.css" rel="stylesheet" />
    <link href="Css/DownPane.css" rel="stylesheet" />
    <link href="Css/header.css" rel="stylesheet" />        
    <link href="Css/HelpEntities.css" rel="stylesheet" />

    <style type="text/css">
        .auto-style5 {
            width: 284px;
        }


        .auto-style6 {
            width: 48px;
            height: 36px;
        }

        /**/
        .auto-style7 {
            width: 297px;
            height: 80px;
        }
    </style>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/Reports.js"></script>

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
            if ($("#startDate").val() == "")
                $("#startDate").datepicker('setDate', currentDate);//set date
            $("#endDate").datepicker();//initialise
            if ($("#endDate").val() == "")
                $("#endDate").datepicker('setDate', currentDate); //set date
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <script type="text/javascript">
            // Get the instance of PageRequestManager.
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            // Add initializeRequest and endRequest
            prm.add_initializeRequest(prm_InitializeRequest);

            prm.add_endRequest(prm_EndRequest);

            // Called when async postback begins
            function prm_InitializeRequest(sender, args) {
                // get the divImage and set it to visible
                var panelProg = $get('divImage');
                panelProg.style.display = '';

                // reset label text
                 <%--var lbl = $get('<%= this.lbltext.clientid %>');--%>
                //lbl.innerHTML = '';

                // Disable button that caused a postback
                $get(args._postBackElement.id).disabled = true;

            }

            // Called when async postback ends
            function prm_EndRequest(sender, args) {
                // get the divImage and hide it again
                var panelProg = $get('divImage');
                panelProg.style.display = 'none';

                // Enable button that caused a postback
                var btn = sender._postBackSettings.sourceElement.id;
                btn.disabled = false;
            }
        </script>

        <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid;" />
        <div class="fragment Default">
            <div class="header Banner">
                 <h2>Demography Report</h2>
              
                <div style="text-align: right">
                    <a href="Settings.aspx" class="modal">Admin Features</a>
                </div>
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;" />             
            <input type="hidden" id="AuthID" value="" runat="server" />
            <input type="hidden" id="UType" value="" runat="server" />
            <table style="width: 100%; vertical-align: top;" border="0">
            
                 <tr style="background: #E2EBFC">
                    <td colspan="2" style="text-align: center">
                        
                        <asp:Button ID="btnReports" runat="server" Text="Click Here To Get Report" OnClick="btnReports_Click" />
                    </td>
                </tr>
                <tr style="background: #dcdcdc">
                    <td class="auto-style5">Total User Count</td>
                    <td>
                        <asp:Label ID="lblUserCount" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr style="background: #f5f5f5">
                    <td class="auto-style5">Group Wise User Count</td>
                    <td>

                        <asp:GridView ID="gvGroups" runat="server">
                            <HeaderStyle BackColor="Gray" ForeColor="White" />
                            <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body) " />
                            <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body) " />

                        </asp:GridView>
                    </td>

                </tr>
                <tr style="background: #dcdcdc">
                    <td class="auto-style5">Activation Missed User Count</td>
                    <td>
                        <asp:Label ID="lblMissed" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>

                <tr style="background: #f5f5f5">
                    <td valign="top" class="auto-style5">Active SOS Sessions Report</td>
                    <td>

                        <asp:GridView ID="grdActiveSOS" runat="server">
                            <HeaderStyle BackColor="Gray" ForeColor="White" />
                            <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body) " />
                            <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body) " />

                        </asp:GridView>
                        <asp:Label ID="lblActiveSOS" runat="server" Text="No Active SOS Sessions At Present" Visible="False"></asp:Label>
                        <br />

                        <br />
                        <br />
                    </td>

                </tr>
            </table>

            <div>
                <table>
                    <tr style="background: #dcdcdc;vertical-align:top">
                        <td>History SOS Sessions Report</td>
                        <td>Start Date:&nbsp;<asp:TextBox ID="startDate" runat="server"  />
                         </td>
                        <td>End Date:&nbsp;<asp:TextBox ID="endDate" runat="server" />                           
                            
                        </td>
                        <td colspan="2" style="vertical-align: top;" bgcolor="white">
                            <asp:Button ID="btnHistorySOS" runat="server" Text="Click Here to get History SOS Data" OnClick="btnHistorySOS_Click" />
                        </td>

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvSOSReport" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="10">
                                    <HeaderStyle BackColor="Gray" ForeColor="White" />
                                    <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body) " />
                                    <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body) " />
                                    <Columns>

                                        <asp:BoundField DataField="SNo"
                                            HeaderText="SNo" />
                                        <asp:BoundField DataField="MobileNumber"
                                            HeaderText="MobileNumber" />
                                        <asp:BoundField DataField="UserName"
                                            HeaderText="User Name" />
                                        <asp:BoundField DataField="TotalTimeinSOS"
                                            HeaderText="TotalTimeinSOS(Secs)" />
                                        <asp:BoundField DataField="SOSAlerts"
                                            HeaderText="#SOSAlerts" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="SOSBuddies"
                                            HeaderText="#SOSBuddies" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="EmailAlerts"
                                            HeaderText="#EmailAlerts" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="EmailBuddies"
                                            HeaderText="#EmailBuddies" ItemStyle-HorizontalAlign="Center" />

                                    </Columns>
                                </asp:GridView>
                                <div id="divImage" style="display: none; align-content: center; margin-left: 500px; margin-top: 0px">
                                    <asp:Image ID="Image1" runat="server" src="Content/Images/load-mask.gif" />
                                    <p style="margin-bottom: 20px">Processing...</p>
                                </div>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                </table>
                <p>&nbsp;</p>
            </div>
        </div>
    </form>
</body>
</html>
