<html style="width: 100%; height: 100%;">
<head>
    <title></title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script type="text/javascript" src="https://cdn.dev.skype.com/uri/skype-uri.js"></script>

    <link href="Css/Settings.css" rel="stylesheet" />

    <script type="text/javascript" src="Scripts/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Scripts/naturalSort.js"></script>
    <script type="text/javascript" src="Scripts/gconfig.js"></script>
    <script type="text/javascript" src="Scripts/Storage.js"></script>
    <script type="text/javascript" src="Scripts/MarshallManagement.js"></script>
    <script type="text/javascript" src="Scripts/GroupManagement.js"></script>
</head>
<body onload="GetMarshalsToUnAssign();">
    <div class="fragment Settings">
        <div class="Banner">
            <div class="pagetitle">
                Administration
            </div>
        </div>
        <div id="marshal_content" class="MainArea">
            <div id="userList" class="leftpanel">
                <div class="leftpanelitemheader menuitem" style="background-color: lightgrey; color: Navy"><b>Administration</b> </div>
                <div class="leftpanelitemheader menuitem"><a href="AllUsers.aspx" style="text-decoration: none" class="menuitem">All Users</a></div>
                <div class="leftpanelitemheader menuitem" onclick="ShowMarshalsForManaging()">&nbsp;Manage Marshal</div>
                <div class="leftpanelitemheader menuitem" onclick="GetMarshalsToUnAssign()">&nbsp;Unassign Marshals</div>
                <div class="leftpanelitemheader menuitem" onclick="NavigateToGroupManagement()">Manage Group</div>
                <br />
                <div class="leftpanelitemheader menuitem" style="background-color: lightgrey; color: Navy"><b>Reports</b> </div>
                <div class="leftpanelitemheader menuitem"><a href="Report.aspx" style="text-decoration: none" class="menuitem">User Count Report</a></div>
                <div class="leftpanelitemheader menuitem"><a href="UserReport.aspx" style="text-decoration: none" class="menuitem">User Demography</a></div>
                <div class="leftpanelitemheader menuitem"><a href="Incidents.aspx" style="text-decoration: none" class="menuitem">Incidents Report</a></div>
                <div class="leftpanelitemheader menuitem"><a href="ActiveSOSReport.aspx" style="text-decoration: none" class="menuitem">Active SOS Report</a></div>
                <div class="leftpanelitemheader menuitem"><a href="ActiveUsers.aspx" style="text-decoration: none" class="menuitem">User Activity Report</a></div>
            </div>
            <div class="middlepanel" id="manageDiv" style="display: none">
                <span class="sectiontitle">Manage Marshal
                </span>
                <div class="sectiondetails">
                    <table>
                        <tr>
                            <td><b>EmailID: </b></td>
                            <td>
                                <input required type="text" id="LiveId" /><span style="color: red">  *Enter Live Id of a valid app user</span></td>
                        </tr>
                        <tr>
                            <td>
                                <b>Phone Number:</b>
                            </td>
                            <td>
                                <input required type="text" id="MarshalPhoneNum" value="+91" /><span style="color: red">  *Specify the phone number of the profile that you wish to use</span></td>

                        </tr>
                    </table>
                    <br />
                    <div title="Add Marshal">
                        <input type="button" value="Add" onclick="SavemarshalInfo()" />&nbsp;&nbsp;&nbsp;
                        <input type="button" value="Refresh" onclick="displayMarshalList()" />
                    </div>
                </div>
                <div class="sectiondetails" style="padding-top: 10px">
                    <div id="marshalTableDiv" class="tableDisplay">
                    </div>
                </div>
            </div>

            <div class="middlepanel" id="unassignDiv">
                <span class="sectiontitle">UnAssign Marshals
                </span>
                <div class="sectiondetails" style="padding-top: 10px">
                    <div id="marshalTableDivUnassign" class="tableDisplay">
                    </div>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
