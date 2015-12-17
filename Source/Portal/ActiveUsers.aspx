<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveUsers.aspx.cs" Inherits="SOS.Web.ActiveUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Guardian Reports</title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="Content/Css/Popup.css" rel="stylesheet" />
    <link href="Content/Css/Spinner.css" rel="stylesheet" />
    <link href="Css/DownPane.css" rel="stylesheet" />
    <link href="Css/header.css" rel="stylesheet" />        
    <link href="Css/HelpEntities.css" rel="stylesheet" />

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


        <div>
            <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid;">
            </div>
            <div class="fragment Default">
                <div class="header Banner">
                    <h2>User Activity Report</h2>
                    <div style="text-align: right">
                        <a href="Settings.aspx" class="modal">Admin Features</a>
                    </div>
                </div>
                <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">

                    <input type="hidden" id="AuthID" value="" runat="server" />
                    <input type="hidden" id="UType" value="" runat="server" />

                    <table style="width: 100%; vertical-align: top;" border="0">
                   

                        <tr style="background: #dcdcdc;vertical-align: top;">
                            <td style="width: 30%;position:relative">Start Date:&nbsp;
                                <asp:TextBox ID="startDate" runat="server" />                               
                                </td>
                            <td style="width: 30%;position:relative">
                                &nbsp;&nbsp; End Date:&nbsp;<asp:TextBox ID="endDate" runat="server" />                               
                                </td>
                                <td>             
                                <asp:Button ID="btnReports" runat="server" Font-Names="Calibri (Body)" Text="Submit" OnClick="btnReports_Click" />
                                </td>
                        </tr>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvSOSReport" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="10">
                                    <HeaderStyle BackColor="Gray" ForeColor="White" />
                                    <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body)" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body)" />
                                    <Columns>

                                        <asp:BoundField DataField="SNo"
                                            HeaderText="SNo" />
                                        <asp:BoundField DataField="MobileNumber"
                                            HeaderText="MobileNumber" />
                                        <asp:BoundField DataField="UserName"
                                            HeaderText="UserName" />
                                        <asp:BoundField DataField="TotalTracks"
                                            HeaderText="TotalTracks" />
                                        <asp:BoundField DataField="TotalSOSs"
                                            HeaderText="TotalSOSs" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalSMSSent"
                                            HeaderText="TotalSMSSent" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalEmailSent"
                                            HeaderText="TotalEmailSent" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
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
