<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DummyReports.aspx.cs" Inherits="SOS.Web.DummyReports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="Scripts/Reports.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
        </div>
        <div class="fragment Default">
            <div class="header Banner">
                <div class="logo">
                    <img src="Content/Images/logoHeader.png" />
                </div>
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
                 <input type="hidden" id="AuthID" value="" runat="server" />
                <input type="hidden" id="UType" value="" runat="server" />
                <asp:Label ID="lblTable" runat="server"></asp:Label>
               
                <br />
               <asp:Label ID="lblDeltaLiveGeo" runat="server"></asp:Label>
                <br />
                <asp:Button ID="btnUpdateUserName" runat="server" Text="Update UserName in GroupMembership" OnClick="btnUpdateUserName_Click" />
                <br />
                <br />
                <asp:Button ID="btnEncryptPhoneNumber" runat="server" Text="Encrypt Phone Number in PhoneValidation" OnClick="btnEncryptPhoneNumber_Click" />
            </div>
    </div>
    </form>
</body>
</html>
