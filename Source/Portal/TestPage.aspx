<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="SOS.Web.TestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//js.live.net/v5.0/wl.js"></script>
    <script type="text/javascript" src="Scripts/gconfig.js"></script>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/TestPage.js"></script>
   
       <style type="text/css">
        #LiveAuthID {
            width: 873px;
        }
        #LiveUserID {
            width: 871px;
        }
        #txtLiveAuthID {
            width: 760px;
        }
        #txtLiveUserID {
            width: 759px;
        }
    </style>

</head>
<body>
     <form id="form1" runat="server">
     <div>
     <a id="groupLoginLink" href="#loginLink"  >User Login</a> | 
    </div> <br /><br />
    <div id="LiveDetails" >
        
        <asp:Button ID="btnLiveID" runat="server" Text="Get Live UserID & AuthID" OnClick="btnLiveID_Click"  />
        <br /> <br />
        <table>
            <tr>
                <td>Live AuthID</td>
                <td><input type="text" id="txtLiveAuthID" runat="server" /> </td>
            </tr>
            <tr>
                <td>Live UserID</td>
                <td><input type="text" id="txtLiveUserID" runat="server" /> </td>
            </tr>
        </table>

    </div>
     
     </form>
     
</body>
</html>
