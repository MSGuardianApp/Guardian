<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SOS.Web.Map" %>

<!DOCTYPE html>
<html>

<head id="Head1" runat="server">
    <meta name="description" content="Guardian is the ultimate proactive security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures you are safe. Anytime,Anywhere" />
    <link rel="alternate" media="handheld" href="https://guardianportal.cloudapp.net/" />
    <title id="pageTitle">Welcome to Guardian Portal</title>
    <meta property="og:image" content="Content/Images/logo.png" />
    <link rel="shortcut icon" href="Content/Images/logoicon.png" />
    <noscript>
        <meta http-equiv="X-Frame-Options" content="deny" />
    </noscript>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="Content/Css/Popup.css" rel="stylesheet" />
    <link href="Content/Css/Spinner.css" rel="stylesheet" />
    <link href="Css/DownPane.css" rel="stylesheet" />
    <link href="Css/header.css" rel="stylesheet" />
    <link href="Css/Main.css" rel="stylesheet" />
    <link href="Css/Login.css" rel="stylesheet" />
    <link href="Css/HelpEntities.css" rel="stylesheet" />

    <script type="text/javascript" src="https://code.jquery.com/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script type="text/javascript" src="https://cdn.dev.skype.com/uri/skype-uri.js"></script>
    <script type="text/javascript" src="Scripts/jquery.session.js"></script>
    <script type="text/javascript" src="//js.live.net/v5.0/wl.js"></script>

    <script type="text/javascript" src="Scripts/gconfig.js"></script>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/Utility.js"></script>
    <script type="text/javascript" src="Scripts/HelpEntities.js"></script>
    <script type="text/javascript" src="Scripts/DownPane.js"></script>
    <script type="text/javascript" src="Scripts/js/LiveLogin.js"></script>
    <script type="text/javascript" src="Scripts/DirectionModule.js"></script>
    <script type="text/javascript" src="Scripts/Incidents.js"></script>
    <script type="text/javascript" src="Scripts/MarshallManagement.js"></script>
    <script type="text/javascript" src="Scripts/History.js"></script>
    <script type="text/javascript" src="Scripts/MapBehaviour.js"></script>
    <script type="text/javascript" src="Scripts/Users.js"></script>
</head>
<body style="margin: 0px">
    <script type="text/javascript">
        var appInsights = window.appInsights || function (config) {
            function s(config) { t[config] = function () { var i = arguments; t.queue.push(function () { t[config].apply(t, i) }) } } var t = { config: config }, r = document, f = window, e = "script", o = r.createElement(e), i, u; for (o.src = config.url || "//az416426.vo.msecnd.net/scripts/a/ai.0.js", r.getElementsByTagName(e)[0].parentNode.appendChild(o), t.cookie = r.cookie, t.queue = [], i = ["Event", "Exception", "Metric", "PageView", "Trace"]; i.length;) s("track" + i.pop()); return config.disableExceptionTracking || (i = "onerror", s("_" + i), u = f[i], f[i] = function (config, r, f, e, o) { var s = u && u(config, r, f, e, o); return s !== !0 && t["_" + i](config, r, f, e, o), s }), t
        }({
            instrumentationKey: "<%=@Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey%>"
        });

    window.appInsights = appInsights;
    appInsights.trackPageView();

    </script>

    <div class="fragment Default" style="margin: 0px">
        <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3">
                    <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;" />
                    <div class="header Banner">
                        <div class="Bannermenu">
                            <div>
                                Welcome <b><span id="loggedInName">Guest</span></b>
                                <span>
                                    <a id="defultLoginLink" href="#" onclick="ShowLoginPopup();"><b>Sign in </b></a>
                                    <a id="logoutLink"></a>
                                </span>
                            </div>
                        </div>
                        <div class="logo">
                            <img src="Content/Images/logoHeader.png" />
                        </div>

                        <div class="errorMsg">
                            <label id="errorLabel"></label>
                        </div>
                        <div class="Bannersearch">
                            <div>
                                <span id="groupSetting">
                                    <a href="Settings.aspx" class="modal">Admin Features</a>
                                </span> | 
                                 <span>
                                     <a href="Home.htm"  target="_blank">Help</a>

                                 </span> | 
                                <span>
                                    <a href="mailto:guardianapp@outlook.com" target="_blank">Support </a>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;" />
                </td>
            </tr>
            <tr>
                <td style="width: 180px; vertical-align: top;">
                    <div id="userList" class="leftpanel">
                        <table id="leftpanegroups">
                            <tr>
                                <td>
                                    <div id="SOStile" class="groupTile selected" onclick="groupSOSClick();">
                                        <div class="tileCountStyle"><span id="SOSCount">0</span></div>
                                        <div class="tileTextStyle">SOS</div>
                                    </div>
                                </td>
                                <td>
                                    <div id="Activetile" class="groupTile" onclick="groupActiveClick();">
                                        <div class="tileCountStyle"><span id="ActiveCount">0</span></div>
                                        <div class="tileTextStyle">Active</div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Inactivetile" class="groupTile" onclick="groupInactiveClick();">
                                        <div class="tileCountStyle"><span id="InactiveCount">0</span></div>
                                        <div class="tileTextStyle">Users</div>
                                    </div>
                                </td>
                                <td>
                                    <div id="Incidentstile" class="groupTile" onclick="groupIncidentsClick();">
                                        <div class="tileCountStyle"><span id="IncidentsCount">0</span></div>
                                        <div class="tileTextStyle">Incidents</div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="lstSOSdiv" class="appear">
                            <ul class="leftpanelSOSitem" id="lstSOS"></ul>
                        </div>
                        <div id="lstTrackdiv" class="disappear">
                            <ul class="leftpanelTrackitem" id="lstTrack"></ul>
                        </div>
                        <div id="lstinactivediv" class="disappear">
                            <ul class="leftpanelinactiveitem" id="lstinactive"></ul>
                        </div>
                        <div id="lstincidentdiv" class="disappear">
                            <table>
                                <tr>
                                    <td>Start Date</td>
                                    <td>
                                        <input type="text" id="IncidentStartDate" style="width: 70px" /></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>End Date</td>
                                    <td>
                                        <input type="text" id="IncidentEndDate" style="width: 70px" /></td>
                                    <td>
                                        <button type="button" onclick="getIncidentsAndStartProcess()" style="width: 35px">Go</button></td>
                                </tr>
                            </table>
                            <ul class="leftpanelincidentitem" id="lstincident">
                                <li onclick="IncidentsAll();">All</li>
                                <li onclick="IncidentsAccident();">Accident</li>
                                <li onclick="IncidentsTease();">Harassment</li>
                                <li onclick="IncidentsRagging();">Ragging</li>
                                <li onclick="IncidentsRobbery();">Robbery</li>
                                <li onclick="IncidentsOther();">Others</li>
                            </ul>
                        </div>
                        <br />
                        <div class="TrackingArea">
                            <div class="leftpanel">
                                <div class="leftpanelitemheader attachmentsection" onclick="ToggleAttachments();">Attachments</div>
                                <div class="leftpanelitemheader trackingsection" onclick="ToggleTracking();">History</div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="width: 85%; vertical-align: top;">
                    <div id='map' class="mapview"></div>
                </td>
                <td style="width: 10%; vertical-align: top; margin-left: 1px">
                    <div class="rightpanel" id="help">

                        <div class="rightpanelbackground">
                            Help Nearby
                        </div>
                        <div class="rightpanelhelpitem" id="lstHelp">
                            <ul>
                                <li id="police">Police Station</li>
                                <li id="hospital">Hospitals</li>
                                <li id="school">Schools</li>
                                <li id="govt">Government Office</li>
                            </ul>
                        </div>
                        <div>
                            <span id="NewSOS"></span>

                            <div class="rightpanelbackground">
                                Information
                            </div>
                            <div id="lstInformation" class="rightpanelhelpitemInfo">
                                <ul>
                                    <table>
                                        <tr>
                                            <td>Lat</td>
                                            <td>
                                                <input type="text" id="lstInformationLat" style="width: 80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Long</td>
                                            <td>
                                                <input type="text" id="lstInformationLong" style="width: 80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center">
                                                <button type="button" onclick="getAddress()">Find Address</button></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div id="addressdisplay"></div>
                                            </td>
                                        </tr>
                                    </table>

                                </ul>
                            </div>
                            <br></br>

                            <div class="rightpanelbackground">
                                Directions 
                            </div>
                            <div id="directionInfo" class="rightpanelhelpitem directionInfoContent">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <footer id="footer">
        <div id="navcontainer">
            <ul>
                <li>© 2013 Guardian      &nbsp;</li>
                <li><a href="Privacy.htm" target="_blank">Privacy statement</a>
                </li>
                <li><%--<a href="TermsOfUse.htm">Terms of use</a>--%>
                </li>
                <li><a href="mailto:guardianapp@outlook.com" target="_blank">Contact us</a>
                </li>
            </ul>
        </div>
    </footer>

    <div class="LoginModal" id="modalLogin">
        <div class="content">

            <img id="modalclosebutton" src="Content/Images/close.png" alt="close" class="close" />
            <div class="content-main">
                <div id="dialog-message" class="fragment Login">
                    <div class="Banner">
                        <div class="pagetitle">
                            Login
                        </div>
                    </div>
                    <div class="MainArea">
                        <div class="errorMsgLogin">
                            <label id="errorLabelLogin"></label>
                        </div>
                        <div class="sectiontitle">
                            Select type of login and sign in
                        </div>

                        <div class="sectiondetails">

                            <div class="leftside">
                                <input id="GroupLogin" type="radio" name="login" value="Group Login" checked="checked" />
                                Group Login  
                            </div>
                            <div class="rightside">
                                <input type="radio" name="login" value="User Login" />User Login 
                            </div>
                            <div class="bottom">
                                <a id="loginLink"></a>
                                <a id="loginButton" onclick="javascript: BypassLiveLogin();">DEV-LOGIN
                                </a>
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <div class="LoginModal" id="modalSOS">
        <div class="content">
            <img id="modalSOSclosebutton" src="Content/Images/close.png" alt="close" class="close" onclick="CloseTheDialog();" />
            <div class="content-main" style="overflow-y: scroll;">
                <div class="Banner1" style="text-align:center;">
                    <div class="pagetitle">
                        <h2>New SOS Alerts</h2> 
                    </div>               
                </div>
                <div>
                    <div>
                        <div id="sosDiv">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal" id="modal">
        <div class="content">
            <img id="Img1" src="Content/Images/close.png" alt="close" class="close" />
            <div class="content-main"></div>
        </div>
    </div>
    <div id="historyDiv" class="disappear">
        <img id="historyImage" src="Content/Images/close.png" alt="" onclick="hideHistoryDiv()" />
        <div id="attachmentsectiondiv" class="middlepanel appear"></div>
        <div id="trackingsectiondiv" class="middlepanel disappear">
            <div class="ui-widget">
                <table>
                    <tr>
                        <td style="white-space: nowrap; vertical-align: top">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <label for="searchUsers">FilterUser</label>
                                    </td>
                                    <td style="text-align: left">
                                        <input type="text" id="searchKeyName" style="width: 100px" />
                                        <input type="submit" onclick="LoadUsers();" value="Search" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <label for="searchUsers">User</label>
                                    </td>
                                    <td>
                                        <!--<input id="searchUsers" />-->
                                        <select id="searchUsers" style="width: 200px">
                                            <option></option>
                                        </select>
                                        <label id="reqSearchUser" style="color: red; display: none">*</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <label for="from">From Date</label>
                                    </td>
                                    <td>
                                        <input type="text" id="datepickerFrom" /><label id="reqFrom" style="color: red; display: none">*</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <label for="from">To Date</label>
                                    </td>
                                    <td>
                                        <input type="text" id="datepickerTo" /><label id="reqTo" style="color: red; display: none">*</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <input type="submit" onclick="GetUserHistory();" value="Search" />
                                        <input type="submit" onclick="ClearUserHistory();" value="Clear" />
                                    </td>
                                </tr>
                            </table>
                        </td>

                        <td>
                            <div class="trackingdetails">
                                <table id="historyData" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%-- <div class="spinnerBackground">
    <div class="spinnerBusy">
        <div class="spinnerImage"><img src="/Content/Images/Spinner.gif" /></div>
        <div class="spinnerText">Loading...</div>
    </div>
    </div>--%>
</body>
</html>
