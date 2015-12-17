<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveSOSReport.aspx.cs" Inherits="SOS.Web.ActiveSOSReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Active SOS Sessions Report</title>
    <script type="text/javascript">

        function sosJSFunction(user) {

            PageMethods.SOSStop(user, StopSOS_Result);
        }

        function StopSOS_Result(ResultString) {
            if(!ResultString)
                alert('Profile Doesnot belong to this Group....');
        }       

    </script>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/Reports.js"></script>
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
        <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid; ">
        </div>
        <div class="fragment Default">
            <div class="header Banner">
               <h2>Live SOS Users</h2>
                <div style="text-align: right">
                    <a href="Settings.aspx" class="modal">Admin Features</a>
                </div>               
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">             
              
            <asp:UpdatePanel id ="panel1" runat="server">
                <ContentTemplate>
                    
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Button ID="btnReports" runat="server" Text="Click Here to get the Report / Refresh" OnClick="btnReports_Click" /><br />
                        <input type="hidden" id="AuthID" value="" runat="server" />
                        <input type="hidden" id="UType" value="" runat="server" />

                        <asp:Label ID="lblTable" runat="server"></asp:Label>


                        <div id="divImage" style="display: none; align-content: center; margin-left: 640px; margin-top: 120px">
                            <asp:Image ID="Image1" runat="server" src="Content/Images/load-mask.gif" />
                            <p style="margin-bottom: 20px">Processing...</p>
                        </div>
                        <br />



                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
