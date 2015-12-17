<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserReport.aspx.cs" Inherits="SOS.Web.UserReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Guardian User Demography Report</title>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/Reports.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true" AsyncPostBackTimeOut="36000"></asp:ScriptManager>
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
        <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
        </div>
        <div class="fragment Default">
            <div class="header Banner">
                 <h2>Guardian User Demography Report</h2>
                <div style="text-align: right">
                    <a href="Settings.aspx" class="modal">Admin Features</a>
                </div>
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
            </div>
            <input type="hidden" id="AuthID" value="" runat="server" />
            <input type="hidden" id="UType" value="" runat="server" />
            <table style="width: 100%; vertical-align: top;" border="0">
               
                <tr style="background: #f5f5f5">

                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnReports" runat="server" Text="Click Here to Get the Report" OnClick="btnReports_Click" />

                                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="false" horizontalalign="Center">
                                    <HeaderStyle BackColor="Gray" ForeColor="White" />
                                    <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body) " />
                                    <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body) " />
                                     <Columns>
                                        <asp:BoundField DataField="SNo"
                                            HeaderText="SNo" />
                                        <asp:BoundField DataField="UserName"
                                            HeaderText="User Name" />
                                        <asp:BoundField DataField="BuddiesCount"
                                            HeaderText="BuddiesCount" />
                                        <asp:BoundField DataField="GroupCount"
                                            HeaderText="GroupCount" />
                                        <asp:BoundField DataField="FacebookLinked"
                                            HeaderText="FacebookLinked"/>                                                                       
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <div id="divImage" style="display: none; align-content: center; margin-left: 640px; margin-top: 120px">
                                    <asp:Image ID="Image1" runat="server" src="Content/Images/load-mask.gif" />
                                    <p style="margin-bottom: 20px">Processing...</p>
                                </div>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                </tr>
            </table>
    </form>
</body>
</html>
