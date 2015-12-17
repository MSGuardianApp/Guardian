<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscribeAction.aspx.cs" Inherits="SOS.Web.SubscribeAction" %>

<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guardian</title>
</head>
<body>
    <form id="buddyUnsubscribe" runat="server">
        <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
        </div>
        <div class="fragment Default">
            <div class="header Banner">
                <div class="logo">
                    <img src="Content/Images/logoHeader.png" />
                </div>
            </div>
            <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;">
            </div>
            <div style="height: 50px"></div>
            <div style="text-align: center; width: 1400px; color: orange; font-size: medium;">
                <asp:Label ID="validationMessage" runat="server" Text="" Font-Names="Calibri" Font-Size="Larger"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
