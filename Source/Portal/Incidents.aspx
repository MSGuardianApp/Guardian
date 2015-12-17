<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Incidents.aspx.cs" Inherits="SOS.Web.Incidents" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guardian Incidents Report</title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="Content/Css/Popup.css" rel="stylesheet" />
    <link href="Content/Css/Spinner.css" rel="stylesheet" />
    <link href="Css/DownPane.css" rel="stylesheet" />
    <link href="Css/header.css" rel="stylesheet" />        
    <link href="Css/HelpEntities.css" rel="stylesheet" />

     <style type="text/css">
  

         td.startDate {
             width: 50%;
         }       
  </style>

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
        <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid;">
        </div>
        <div class="fragment Default">
            <div class="header Banner">  
                 <h2>Incidents Report</h2>                                
                <div style="text-align: right">
                    <a href="Settings.aspx" class="modal">Admin Features</a>
                </div>
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid;" />            
            <input type="hidden" id="AuthID" value="" runat="server" />
            <input type="hidden" id="UType" value="" runat="server" />           
                    <table style="table-layout:fixed;width: 100%; vertical-align: top;" border="0">                       
                        <tr style="background: #dcdcdc;vertical-align: top;" >
                            <td style="width: 30%;position:relative">Start Date:&nbsp;
                                <asp:TextBox ID="startDate" runat="server" />                                
                                </td>                          
                                <td style="width: 27%;vertical-align:top">
                                End Date:&nbsp;<asp:TextBox ID="endDate" runat="server" />
                               
                               </td>
                                <td style="background: white;width: 3%;vertical-align:top">
                                    <b>OR&nbsp;&nbsp</b>
                                </td>
                                 <td>
                                     &nbsp;&nbsp;&nbsp;IncidentID:&nbsp;
                                     <asp:TextBox ID="incidentID" runat="server" Width="119px" />
                                <br />                                                                                                                                                           
                                                               
                                </td>

                                <td  style="width: 16%;vertical-align: top;">
                                
                                     <asp:Button ID="submitBtn" runat="server" Height="33px" OnClick="submitBtn_Click" Text="Get Incidents"/>
                                </td>
                        </tr>
                        </table>
                    <table style="width: 100%; vertical-align: top;" border="0">
                        <tr style="background: #f5f5f5; width: 100%">
                            <td>
                                
                               <asp:UpdatePanel runat="server">
                             <ContentTemplate>
                                

                                <div id="gridDiv">
                                    <asp:GridView ID="gvIncidents" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="Gray" ForeColor="White" Font-Names="Calibri (Body)" />
                                        <RowStyle BackColor="LightGray" ForeColor="Black" Font-Names="Calibri (Body) " />
                                        <AlternatingRowStyle BackColor="White" ForeColor="Black" Font-Names="Calibri (Body) " />
                                        <Columns>                                            
                                            <asp:BoundField DataField="IncidentID"
                                                HeaderText="IncidentID" />
                                            <asp:BoundField DataField="Type"
                                                HeaderText="Type" />
                                             <asp:BoundField DataField="Name"
                                                HeaderText="Name" />
                                             <asp:BoundField DataField="MobileNumber"
                                                HeaderText="MobileNumber" />
                                            <asp:BoundField DataField="DateTime"
                                                HeaderText="Reported On"
                                                DataFormatString="{0:d}" />
                                            <asp:HyperLinkField Text="link" DataNavigateUrlFields="MediaUri"
                                                HeaderText="Evidence"
                                                Target="_blank" />                                          
                                            <asp:TemplateField HeaderText ="Description">
                                                   <ItemTemplate>
                                                         <div style="overflow:hidden;text-overflow:ellipsis;white-space:nowrap;width:75px">
                                                              <asp:Label ID="review" runat="server" Text='<%# Bind("AdditionalInfo") %>' Tooltip='<%#Bind("AdditionalInfo")%>'></asp:Label>
                                                         </div>
                                                   </ItemTemplate>
                                                   <HeaderStyle Wrap="false" Width="75" HorizontalAlign="Left" />
                                                   <ItemStyle Wrap="false" Width="75"></ItemStyle>
                                        </asp:TemplateField>  
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                  <div id="divImage" style="display: none; align-content: center; margin-left: 640px; margin-top: 0px">
                                    <asp:Image ID="Image1" runat="server" src="Content/Images/load-mask.gif" />
                                    <p style="margin-bottom: 20px">Processing...</p>
                                </div>
                                  </ContentTemplate>
                                   </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
               
            <p>&nbsp;</p>
        </div>
    </form>
</body>
</html>
