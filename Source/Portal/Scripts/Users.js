/// <reference path="../js/jquery-1.8.3-vsdoc.js" />
var SearchManager = -1;
var historyTab = false;
var historyAddress;
var SubjectUser = { "profileID": "-1", "userID": "-1", "userIndex": "-1", "pin": "-1", "info": { "infobox": "-1", "description": "-1", "address": "-1" }, "zoom": "15", "currentLocation": { "lat": "17.39", "long": "78.44" } };
var SelectedUserIndex;
var SelectedUserInfobox = -1;
var SelectedUserInfoboxDescription;
var HelpEntities = -1;
var TrackUserIndex = new Array();
var SOSUserIndex = new Array();
var InActiveUserIndex = new Array();
var IncidentsIndex = new Array();
var mediaURIList = new Array();
var AdminID = 0;
var GroupID = 0;
var MarshalList;
var LiveUserList;
var CurrentSOSUserList = new Array();
var LiveUserDetails;
var DefaultGeoLocation = "";
var Profile = { "alias": "", "type": "", "profileID": "", "userID": "", "groupID": "" };
var HelpEntitiesLayer;
var BuddyLayer;
var LoggedInFlag = false;
var UserEntitiesLayer = null;
var PortalMap = -1;
var prevAttachmentCount = 0;
var PreviousSOSIndexCount = 0;
var UserStatusLoaded = false;
var SelectedUserTimer;
var CurrentProfile;
var AnonymousUser = false;
var SOSoutput = "";
var Trackoutput = "";
var ShowIncidents = false;
var startPinDrawn = false;
var initialLat;
var initialLong;
var initialState;
var lastPin;
var lastInfobox;

function initEntitiesLayer() {
    if (PortalMap == -1) {
        setTimeout(initEntitiesLayer, 1000);
    }
    else {
        UserEntitiesLayer = new Microsoft.Maps.EntityCollection();
        PortalMap.entities.push(UserEntitiesLayer);
        HelpEntitiesLayer = new Microsoft.Maps.EntityCollection();
        PortalMap.entities.push(HelpEntitiesLayer);
        MarshalEntitiesLayer = new Microsoft.Maps.EntityCollection();
        PortalMap.entities.push(MarshalEntitiesLayer);
        IncidentEntitiesLayer = null;
        //SubjectUser.userID = -1;
    }
}

$(document).ready(function () {
    $('#logoutLink').hide();
    $("body").prepend('<div class="spinnerBackground"><div class="spinnerBusy"><div class="spinnerImage"><img src="/Content/Images/Spinner.gif" /></div><div class="spinnerText">Loading...</div></div></div>');
    GetMap();

    $("#searchUsers").change(function () {
        localStorage["searchUserID"] = $("#searchUsers").val();
    });

    $(document).delegate('a.modal', 'click', onlaunchmarshalmgmt);
    $('div.modal > div.content > div.actions > button.close, div.modal > div.content > img.close').click(onlaunchmarshalmgmtclose);
    $("#groupSetting").hide();
    if (getSession('email') != null && getSession('userType') != null) {
        $('#errorLabelLogin').empty();
        $('#logoutLink').show();
        $('#defultLoginLink').hide();

        if (getSession('userType') == 'user') {
            document.getElementById('loggedInName').innerText = getSession('userName');
        }
        else {
            document.getElementById("loggedInName").innerText = getSession('userName');;
            $("#groupSetting").show();
        }
        getProfile(getSession('email'), getSession("userType"));

    }


});
function LoadSearchModule() {
    Microsoft.Maps.loadModule('Microsoft.Maps.Search', { callback: StartPortal })
}
function CreateSearchManager() {
    if (SearchManager == -1) {
        PortalMap.addComponent('searchManager', new Microsoft.Maps.Search.SearchManager(PortalMap));
        SearchManager = PortalMap.getComponent('searchManager');
    }
}
function marshalFunctionality() {
    if (SubjectUser.userIndex != -1) {
        getMarshals(GroupID, true);
    }
}
function getAttachments(newitem) {
    if (newitem == null && olditem == null)
        return;   
   
    for (var i = 0; i < newitem.length; i++) {
        if (newitem[i].MediaUri != null) {
            if (mediaURIList.length > 0) {
                var found = false;
                for (var j = 0; j < mediaURIList.length; j++) {
                    if (newitem[i].MediaUri == mediaURIList[j]) {
                        found = true;
                    }
                }
                if (!found)
                    mediaURIList.push(newitem[i].MediaUri);
            }
            else
                mediaURIList.push(newitem[i].MediaUri);
        }
    }
    if (mediaURIList.length > 0) {
        populateHistoryAttachments(mediaURIList);

        if (prevAttachmentCount != mediaURIList.length) {
            ToggleAttachments();
            prevAttachmentCount = mediaURIList.length;
        }
    }
    else {
        hideHistoryDiv();
    }
}
function getProfile(alias, type) {
    busyOn();
    $("#errorLabel").empty();
    $('#errorLabelLogin').empty();
    if (getSession("gSession") === null) {
        clearUserList();
        return false;
    }
    $('#lstSOS li').die();
    clearPortalMap();
    $('#lstSOS li').live('click', selectUserAction);
    $('#lstTrack li').live('click', selectUserAction);
    $('#lstinactive li').live('click', clearPortalMap);
    var profileUrl;
    Profile.alias = alias;
    Profile.type = type;
    LoggedInFlag = true;
    if (type == 'user') {
        profileUrl = GuardianConfig.ServiceURL + '/GetMiniProfile';
        $.ajax({
            url: profileUrl,
            headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
            dataType: 'json',
            timeout: 100000,
            success: function (data) {
                if (data) {
                    if (data == '' || data == 0) {
                        busyOff();
                        $("#errorLabelLogin").text("You are not a registered user in Guardian");
                        Logout();
                        return false;
                    }
                    else {                        
                        Profile.userID = (data.split(','))[0];
                        Profile.profileID = (data.split(','))[1];
                        DynamicGetTotalUsersCount();
                        DynamicGetLiveUsers();
                        setInterval(function () { DynamicGetTotalUsersCount(); }, GuardianConfig.TotalUsersRefreshInterval);
                        setInterval(function () { incrementalUpdatesForStatus(); }, GuardianConfig.PathRefreshInterval);
                    }                    
                }
                else {
                    busyOff();
                    $("#errorLabelLogin").text("You are not a registered user in Guardian");
                    Logout();
                    return false;
                }
                busyOff();
            },
            error: function () {
                busyOff();
                $("#errorLabelLogin").text("Error connecting with Guardian Server. Please try after sometime");
                Logout();
                return false;
            }
        });
    }
    else if (type == 'group') {
        profileUrl = GuardianConfig.ServiceURL + '/GetAdminProfile/' + Profile.alias;
        $.ajax({
            url: profileUrl,
            dataType: 'json',
            headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
            timeout: GuardianConfig.ServiceTimeout,
            success: function (data) {
                if (data) {
                    if (data.Email == null) {
                        busyOff();
                        $("#errorLabelLogin").text("You are not a registered group in Guardian");
                        Logout();                        
                        return false;
                    }
                    else {
                        Profile.groupID = data.Groups[0].GroupID;
                        GroupID = data.Groups[0].GroupID;
                        if (data.Groups[0].ShowIncidents != null)
                            ShowIncidents = data.Groups[0].ShowIncidents;
                        AdminID = data.AdminID;
                        setSession('GroupID', GroupID);
                        setSession('AdminID', AdminID);                        
                        DefaultGeoLocation = data.Groups[0].GeoLocation;                       
                        DynamicGetTotalUsersCount();
                        DynamicGetLiveUsers();
                        setInterval(function () { DynamicGetTotalUsersCount(); }, GuardianConfig.TotalUsersRefreshInterval);
                        setInterval(function () { incrementalUpdatesForStatus(); }, GuardianConfig.PathRefreshInterval);
                        setInterval(function () { incrementalUpdates(); }, GuardianConfig.GetGroupMembersAndIncidentsRefreshInterval);

                        if (ShowIncidents) {
                            getIncidentsAndStartProcess();
                            setInterval(function () { getIncidentsAndStartProcess(); }, GuardianConfig.TotalUsersRefreshInterval);
                        }
                        if (DefaultGeoLocation.length > 0) {
                            var lat = (DefaultGeoLocation.split(','))[0];
                            var long = (DefaultGeoLocation.split(','))[1];
                            FocusLatLong(lat, long, GuardianConfig.DefaultZoomLevel);
                        }
                    }                    
                }
                else {
                    busyOff();
                    $("#errorLabelLogin").text("You are not a registered group in Guardian");
                    Logout();                    
                    return false;
                }
                busyOff();
            },
            error: function (e) {
                busyOff();
                $("#errorLabelLogin").text("Error connecting with Guardian Server. Please try after sometime");
                Logout();
                return false;
            }
        });
    }
    else {
        $("#errorLabelLogin").text("You are not a registered to Guardian. Please try with different account");
        Logout();
        return false;
    }
    return true;
}

function selectUserAction() {
    busyOn();
    clearPortalMap();
    historyTab = false;
    var id = $(this).attr('id');
    clearTimeout(SelectedUserTimer);
    SubjectUser.userIndex = id;
    var currentUser;
    var groupId = Profile.groupID;
    if (groupId == null || groupId == '')
        groupId = '0';
    var myUrl = GuardianConfig.ServiceURL + '/GetProfileLiteByProfileID/' + id + '/' + groupId;
    $.ajax({
        url: myUrl,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        success: function (data) {
            if (data) {
                LiveUserDetails = data;
                currentUser = LiveUserDetails;
                SubjectUser.userID = currentUser.UserID;
                SubjectUser.profileID = currentUser.ProfileID;
                mediaURIList = new Array();
                removeEntities();
                lastPin = null;
                lastInfobox = null;
                getUserLocation(id, 0);
                if (SubjectUser.userIndex != -1)
                    FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.DefaultZoomLevel);
            }
            busyOff();
        },
        error: function (e) {
            busyOff();
        }
    });

    if (GroupID != null && GroupID != 0) {
        marshalFunctionality();
    }
}
function StartPortal() {
    CreateSearchManager();
    loadDirectionsModule();
    mapViewChangeEventHandler();
    RegisterOnClickUserListItem();
    HelpPane();
    mapViewChangeEventHandler();
    if (ParseUrl() == 0) {
        DynamicGetTotalUsersCount();
        DynamicGetLiveUsers();       
        //setInterval(function () { incrementalUpdatesForStatus() }, GuardianConfig.IncrementalTimeInterval);
        //setInterval(function () { incrementalUpdates() }, GuardianConfig.GetGroupMembersAndIncidentsRefreshInterval);

    }
    else if (DefaultGeoLocation.length > 0) {
        var lat = (DefaultGeoLocation.split(','))[0];
        var long = (DefaultGeoLocation.split(','))[1];
        FocusLatLong(lat, long, "15");
    }
    else {
        if(GuardianConfig.DefaultLat && GuardianConfig.DefaultLang && GuardianConfig.DefaultZoomLevel)
            FocusLatLong(GuardianConfig.DefaultLat, GuardianConfig.DefaultLang, GuardianConfig.DefaultZoomLevel);
        else
            FocusLatLong("17.385044", "78.486671", "15");//hyderabad
    }
}
function incrementalUpdates() {
    if (getSession("gSession") === null) {
        clearUserList();
        return;
    }

    if (GroupID != null && GroupID != 0 && MarshalList && MarshalList.length > 0) {
        marshalFunctionality();
    }
}

function incrementalUpdatesForStatus() {
    if (getSession("gSession") === null) {
        clearUserList();
        return;
    }

    DynamicGetLiveUsers();
}

$(window).resize(function () {
    $("#map").height($(window).height() - 130);
    $("#lstSOSdiv").height($(window).height() - 365);
    $("#lstTrackdiv").height($(window).height() - 365);
    $("#lstinactivediv").height($(window).height() - 365);
});


function GetMap() {
    $("#map").height($(window).height() - 130);
    $("#lstSOSdiv").height($(window).height() - 365);
    $("#lstTrackdiv").height($(window).height() - 365);
    $("#lstinactivediv").height($(window).height() - 365);

    Microsoft.Maps.loadModule('Microsoft.Maps.Themes.BingTheme',
        {
            callback: function () {
                PortalMap = new Microsoft.Maps.Map(document.getElementById("map"),
                        {
                            credentials: GuardianConfig.MapCredentials,
                            center: new Microsoft.Maps.Location(GuardianConfig.DefaultLat, GuardianConfig.DefaultLang),
                            mapTypeId: Microsoft.Maps.MapTypeId.road,
                            zoom: GuardianConfig.InitMapZoomLevel,
                            showBreadcrumb: true,
                            customizeOverlays: true,
                            theme: new Microsoft.Maps.Themes.BingTheme()

                        });
                LoadSearchModule();
            }
        });
}

function RegisterOnClickUserListItem() {
    $('#lstHelp li').live('click', function () {
        var id = $(this).attr('id');
        GetHelp(id);
    });
}

function ToggleHover(elm) {
    elm.style = 'OnHoverStyle';
}
function DisplayUserList() {
    var output;
    //Display SOS
    $('#SOSCount').html(SOSUserIndex.length);    
    $('#lstSOS').html(SOSoutput);

    //Display Track
    $('#ActiveCount').html(TrackUserIndex.length);    
    $('#lstTrack').html(Trackoutput);

    if (Profile.type == 'group') {
        $('#lstinactive').html("<B>To view all subscribed users, <br /> please navigate to <br /> Admin Features --> All Users</B>");
    }
}
function getSubjectUser() {
    reverseGeocodeRequest(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, updateAddressSubjectUser);
}
function getHistoryUserAddress(lat, long) {
    reverseGeocodeRequest(lat, long, historylocationData);
}
function historylocationData(result) {
    if (result)
        historyAddress = result.name;

}

function updateAddressSubjectUser(result) { //Called after getting the Addresss
    if (result)
        SubjectUser.info.address = result.name;
    FocusUser(SubjectUser.userIndex);
}


function ParseUrl() {
    clearPortalMap();
    $("#errorLabel").empty();
    $("#errorLabelLogin").empty();
    var query = window.location.search.substring(1);
    var vars = query.split('&');

    var lat;
    var long;

    try {
        if (vars.length > 1) {
            if (vars[0].split('=')[0] == 'V') {
                if (vars.length == 3) {
                    if ((vars[1].split('='))[0] == 'pr') {
                        var profileID = (vars[1].split('='))[1];
                        var sessionID = (vars[2].split('='))[1];
                        UpdateProfileSubject(profileID, sessionID, 0);
                    }
                }
                else if (vars.length == 4) {
                    var userlocaltime = (vars[1].split('='))[1];
                    lat = (vars[2].split('='))[1];
                    long = (vars[3].split('='))[1];
                    ShowDefaultUser(lat, long, true);
                }
                else if (vars.length == 6) {
                    var token = vars[1].substring(2);
                    var userutctime = (vars[2].split('='))[1];
                    var userlocaltime = (vars[3].split('='))[1];
                    lat = (vars[4].split('='))[1];
                    long = (vars[5].split('='))[1];
                    var myUrl = GuardianConfig.ServiceURL + '/SwitchOnSOSviaSMS/' + token + '/' + userutctime + '/' + userlocaltime + '/' + lat + '/' + long;
                    $.ajax({
                        url: myUrl,
                        dataType: 'json',
                        timeout: GuardianConfig.ServiceTimeout,
                        success: function (data) {
                            if (data) {
                                var profileID = data.split(',')[0];
                                var sessionID = data.split(',')[1];
                                UpdateProfileSubject(profileID, sessionID, 0);
                            }
                        },
                        error: function (e) {
                            ShowLoginPopup();
                        }
                    });

                }
                else if (getSession('userType') == null) {
                    ShowLoginPopup();
                }
            }
            else {
                var redirecturl = window.location.href;
                redirecturl = redirecturl.replace(redirecturl.split('?')[0], GuardianConfig.V1PortalUrl);
                window.location = redirecturl;
            }
        }
        else if (getSession('userType') == null) {
            ShowLoginPopup();
        }
    }
    catch (e) {

        if (LoggedInFlag == false) {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            }
            else if (DefaultGeoLocation.length > 0) {
                lat = (DefaultGeoLocation.split(','))[0];
                long = (DefaultGeoLocation.split(','))[1];
                ShowDefaultUser(lat, long, false);
            }
            else {
                lat = GuardianConfig.DefaultLat;
                long = GuardianConfig.DefaultLang;
                ShowDefaultUser(lat, long, false);
            }
        }
        else {
            return 0;
        }
    }
}
function ShowLoginPopup() {
    $('#modalclosebutton').hide();
    $('#modalLogin').show();
    $('#logoutLink').hide();
}
function HideLoginPopup() {
    $('#modalclosebutton').show();
    $('#modalLogin').hide();
    // $('#modalLogin > div.content > div.content-main').html('');
    $('#logoutLink').show();

}
function UpdateProfileSubject(profileID, token, ticks) {    
    clearPortalMap();
    $("#errorLabel").empty();
    $('#errorLabelLogin').empty();
    
    var myUrl = GuardianConfig.ServiceURL + '/GetBasicProfile/' + profileID + '/' + token + '/' + Date.now();
    $.ajax({
        url: myUrl,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        success: function (data) {
            if (data) {
                CurrentProfile = data;
                SubjectUser.userID = CurrentProfile.UserID;
                SubjectUser.profileID = CurrentProfile.ProfileID;
                SubjectUser.userIndex = 0;
                mediaURIList = new Array();
                removeEntities();
                lastPin = null;
                lastInfobox = null;
                GetUserLocationsByToken(profileID, token, ticks);
                if (SubjectUser.userIndex != -1)
                    FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.DefaultZoomLevel);
            }
            else {
                alert('No active tracking session has been found. Either user switched off tracking or you might be looking at old tracking link.');
                ShowLoginPopup();
            }
        },
        error: function (e) {
            ShowLoginPopup();
        }
    });
}
function GetUserLocationsByToken(profileID, token, ticks) {
    $("#errorLabel").empty();
    $('#errorLabelLogin').empty();
    try {
        var myUrl = GuardianConfig.ServiceURL + '/GetUserLocationsByToken/' + profileID + '/' + token + '/' + ticks + '/' + Date.now();
        $.ajax({
            url: myUrl,
            dataType: 'json',
            timeout: GuardianConfig.ServiceTimeout,
            success: function (data) {
                if (data) {
                    var latestTicks = ticks;
                    if (data.LastLocs && CurrentProfile) {
                        var callMeBack = false;
                        var latestTicks = data.LastLocs[data.LastLocs.length - 1].TimeStamp;
                        if (data.IsSOSOn == false && data.IsTrackingOn == false) {
                            // Session has Ended -> Now more incremntal calls
                            // Change the State of data.IsSOS or IsTackingOn by getting from the Last element to avoid any breaks in Focus User
                            var state = (data.LastLocs[data.LastLocs.length - 1].IsSOS) ? "1" : "0";
                            if (state == "0") { // TRACK
                                data.IsTrackingOn = true;
                            } else if (state == "1") { // SOS
                                data.IsSOSOn = true;
                            }
                        } else {
                            callMeBack = true;
                        }
                        AnonymousUser = true;

                        CurrentProfile.LastLocs = data.LastLocs;//as picking only locations
                        CurrentProfile.IsSOSOn = data.IsSOSOn;
                        CurrentProfile.IsTrackingOn = data.IsTrackingOn;
                        CurrentProfile.DataInfo = data.DataInfo;

                        SubjectUser.userID = CurrentProfile.UserID;
                        SubjectUser.profileID = CurrentProfile.ProfileID;
                        if (CurrentProfile.LastLocs != null) {
                            latestUserLocationTicks = CurrentProfile.LastLocs[CurrentProfile.LastLocs.length - 1].TimeStamp;
                            SubjectUser.currentLocation.lat = CurrentProfile.LastLocs[CurrentProfile.LastLocs.length - 1].Lat;
                            SubjectUser.currentLocation.long = CurrentProfile.LastLocs[CurrentProfile.LastLocs.length - 1].Long;

                            if (ticks == 0) {
                                startPinDrawn = false;
                                initialState = (CurrentProfile.LastLocs[0].IsSOS) ? "1" : "0";
                                initialLat = CurrentProfile.LastLocs[0].Lat;
                                initialLong = CurrentProfile.LastLocs[0].Long;
                            }
                            SubjectUser.currentLocation.lat = CurrentProfile.LastLocs[CurrentProfile.LastLocs.length - 1].Lat;
                            SubjectUser.currentLocation.long = CurrentProfile.LastLocs[CurrentProfile.LastLocs.length - 1].Long;
                            getSubjectUser();
                            getAttachments(CurrentProfile.LastLocs);
                        }

                        if (callMeBack) {
                            setTimeout(function myfunction() {
                                GetUserLocationsByToken(profileID, token, latestTicks);
                            }, GuardianConfig.PathRefreshInterval);
                        }
                    }
                    else {
                        if (ticks == 0) {
                            alert('No active tracking session has been found. Either user switched off tracking or you might be looking at old tracking link.');
                            ShowLoginPopup();
                        }
                        else {
                            setTimeout(function myfunction() {
                                GetUserLocationsByToken(profileID, token, latestTicks);
                            }, GuardianConfig.PathRefreshInterval);
                        }
                    }
                }
                else {
                    ShowLoginPopup();
                }
            },
            error: function (e) {
                ShowLoginPopup();
            }
        });
    }
    catch (e) {
        ShowLoginPopup();
    }
}

function showPosition(position) {
    var lat = position.coords.latitude;
    var long = position.coords.longitude;
    ShowDefaultUser(lat, long, false);
}

function ShowDefaultUser(lat, long, sosflag) {
    LiveUserList = [{
        "N": "Guest", "PID": "0", "SID": "0", "S": 1, "M": "", "LastLocs": [{ "Alt": null, "GeoDirection": "", "Lat": "17.385044", "Long": "78.486671", "Speed": 0, "TimeStamp": 0 }, ]
    }];
    LiveUserList[0].LastLocs[0].Lat = lat;
    LiveUserList[0].LastLocs[0].Long = long;
    LiveUserList[0].S = sosflag;
    SOSUserIndex.splice(0, SOSUserIndex.length);
    TrackUserIndex.splice(0, TrackUserIndex.length);
    SOSUserIndex.push(0);
    SubjectUser.currentLocation.lat = lat;
    SubjectUser.currentLocation.long = long;
    SubjectUser.userIndex = 0;
    reverseGeocodeRequest(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, updateAddressSubjectUserUrl);
    $('#nav').css('display', 'none');

}
function updateAddressSubjectUserUrl(result) {
    if (result)
        SubjectUser.info.address = result.name;
    FocusUrlUser();
}
function FocusUrlUser() {
    //Focus to the Selected User Coordinate in the map
    removeEntities();
    var pin;
    if (LiveUserList) {
        var state = (LiveUserList[0].S) ? "1" : "0";
        if (state == "0") {
            pin = PutPushPin(LiveUserList[0].N, SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, 'track');
        }
        else if (state == "1") {
            pin = PutPushPin(LiveUserList[0].N, SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, 'sos');
        }
    }
    var desc = "Location : " + SubjectUser.info.address;

    infoboxOptions = {
        offset: new Microsoft.Maps.Point(0, 25), showPointer: true,
        title: "Guest", description: desc, zIndex: 0, visible: true, actions: [{ label: 'Get Help', eventHandler: HelpPane }]
    };
    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long), infoboxOptions);
    UserEntitiesLayer.push(infobox);    
    SubjectUser.info.infobox = infobox;
    SubjectUser.pin = pin;
    infobox.setLocation(infobox.getLocation());
    
    FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.DefaultZoomLevel);
}
function removeEntities() {

    if (SubjectUser.info.infobox != -1)
        SubjectUser.info.infobox.setOptions({ visible: false });

    if (UserEntitiesLayer && UserEntitiesLayer.getLength() > 0) {
        UserEntitiesLayer.clear();
    }
}

function clearPortalMap() {
    try {
        while (PortalMap.entities.getLength() != 0) {
            for (i = 0; i < PortalMap.entities.getLength() ; i++) {
                PortalMap.entities.remove(PortalMap.entities.get(i));
            }
        }

        if (directionsManager)
            directionsManager.resetDirections();

        initEntitiesLayer();

        $('#attachmentsectiondiv').empty();
    } catch (e) {
        initEntitiesLayer();
    }
}

function getUserLocation(id, lastUserLocationTicks) {

    try {
        if (getSession("gSession") === null) {
            clearUserList();
            return false;
        }
        var resUser = $.grep(LiveUserList, function (e) { return e.PID == id; });
        if (SubjectUser.userIndex != id || (SOSUserIndex.length == 0 && TrackUserIndex.length == 0) || resUser.length == 0) {
            clearTimeout(SelectedUserTimer);
            clearPortalMap();
        }
        else {
            var groupId = Profile.groupID;
            if (groupId == null || groupId == '')
                groupId = '0';
            var myUrl = GuardianConfig.ServiceURL + '/GetLocationDetails/' + id + '/' + groupId + '/' + lastUserLocationTicks+ '/' + Date.now();
            $.ajax({
                url: myUrl,
                headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
                dataType: 'json',
                timeout: GuardianConfig.ServiceTimeout,
                success: function (data) {
                    var latestUserLocationTicks = lastUserLocationTicks;
                    if (data) {
                        if (data.LastLocs && LiveUserDetails) {
                            LiveUserDetails.LastLocs = data.LastLocs;//as picking only locations
                            LiveUserDetails.IsSOSOn = data.IsSOSOn;
                            LiveUserDetails.IsTrackingOn = data.IsTrackingOn;
                            LiveUserDetails.DataInfo = data.DataInfo;

                            currentUser = LiveUserDetails;
                            SubjectUser.userID = currentUser.UserID;
                            SubjectUser.profileID = currentUser.ProfileID;
                            if (currentUser.LastLocs != null) {
                                latestUserLocationTicks = currentUser.LastLocs[currentUser.LastLocs.length - 1].TimeStamp;
                                SubjectUser.currentLocation.lat = currentUser.LastLocs[currentUser.LastLocs.length - 1].Lat;
                                SubjectUser.currentLocation.long = currentUser.LastLocs[currentUser.LastLocs.length - 1].Long;

                                if (lastUserLocationTicks == 0) {
                                    startPinDrawn = false;
                                    initialState = (currentUser.LastLocs[0].IsSOS) ? "1" : "0";
                                    initialLat = currentUser.LastLocs[0].Lat;
                                    initialLong = currentUser.LastLocs[0].Long;
                                }

                                getSubjectUser();
                                getAttachments(currentUser.LastLocs);
                            }
                            else if (DefaultGeoLocation.length > 0) {
                                SubjectUser.currentLocation.lat = (DefaultGeoLocation.split(','))[0];
                                SubjectUser.currentLocation.long = (DefaultGeoLocation.split(','))[1];
                                SubjectUser.info.address = "Location Not Available";
                            }
                            else {
                                SubjectUser.currentLocation.lat = PortalMap.getCenter().latitude;
                                SubjectUser.currentLocation.long = PortalMap.getCenter().longitude;
                                SubjectUser.info.address = "Location Not Available";
                            }
                        }
                        SelectedUserTimer = setTimeout(function() { getUserLocation(id, latestUserLocationTicks); }, GuardianConfig.PathRefreshInterval);
                    }
                },
                error: function (e) {

                }
            });
        }
    }
    catch (e) {

    }
}

function FocusUser(userIndex) {
    //Focus to the Selected User Coordinate in the map
    if (historyTab)
        return;

    //removeEntities(); to leave the path drawn

    var locationAvailable;
    var currentUser;
    var accuracy;

    if (LiveUserDetails == undefined) {
        if (AnonymousUser)
            currentUser = CurrentProfile;
        else
            return;//currentUser = LiveUserList[userIndex]; as dynamicuserlist now doesnt contain any lastlocs
    }
    else
        currentUser = LiveUserDetails;   

    if (currentUser.LastLocs != null) {
        var lastLocs = currentUser.LastLocs[currentUser.LastLocs.length - 1];

        var currentLat = lastLocs.Lat;
        var currentLong = lastLocs.Long;
        var currentTimeStamp = lastLocs.TimeStamp;
        accuracy = lastLocs.Accuracy;
        currentTimeStamp = DateDeserialize(currentTimeStamp);
        locationAvailable = true;
    } else
        locationAvailable = false;
    var currentUserType;
    var pin;
    var infoboxOptions;
    var infobox;

    if (initialState && initialLat && initialLong && (!startPinDrawn)) {//to show up starting pushpin
        if (initialState == "0") {
            PutPushPin(currentUser.Name, initialLat, initialLong, 'trackStart');
        }
        else if (initialState == "1") {
            PutPushPin(currentUser.Name, initialLat, initialLong, 'sosStart');
        }
        startPinDrawn = true;
    }
    if (lastPin)
        UserEntitiesLayer.remove(lastPin);
    if (lastInfobox)
        UserEntitiesLayer.remove(lastInfobox);

    if (currentUser.IsSOSOn == true) {
        currentUserType = "SOS";        
        if (locationAvailable)
            pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'sos');        
        SelectedUserInfoboxDescription = "TimeStamp : " + currentTimeStamp + "<br/> Contact : " + currentUser.MobileNumber + "<br/> Location : " + SubjectUser.info.address;
        
        if (accuracy)
            SelectedUserInfoboxDescription = SelectedUserInfoboxDescription + "<br/> Accuracy : " + accuracy;

        infoboxOptions = {
            offset: new Microsoft.Maps.Point(0, 40), showPointer: true,
            title: currentUser.Name + "(SOS)", description: SelectedUserInfoboxDescription, zIndex: 0, visible: true, actions: [{ label: 'Get Help', eventHandler: HelpPane() }]
        };
        if (locationAvailable)
            infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), infoboxOptions);
        else
            infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(PortalMap.getCenter().latitude, PortalMap.getCenter().longitude), infoboxOptions);
    }
    else if (currentUser.IsTrackingOn == true) {
        currentUserType = "Track";
        pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'track');
        SelectedUserInfoboxDescription = "TimeStamp : " + currentTimeStamp + "<br/> Contact : " + currentUser.MobileNumber + "<br/> Location : " + SubjectUser.info.address;

        if (accuracy)
            SelectedUserInfoboxDescription = SelectedUserInfoboxDescription + "<br/> Accuracy : " + accuracy;

        infoboxOptions = {
            offset: new Microsoft.Maps.Point(0, 40), visible: true,
            title: currentUser.Name + "(TRACK)", description: SelectedUserInfoboxDescription, zIndex: 0, visible: true, actions: [{ label: 'Get Help', eventHandler: HelpPane }]
        };
        infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), infoboxOptions);
    }
    else {
        clearPortalMap();
        return true;
    }
    if (currentUser.IsTrackingOn == true || currentUser.IsSOSOn == true) {
        lastPin = pin;
        lastInfobox = infobox;
        UserEntitiesLayer.push(infobox);

        //PortalMap.entities.push(infobox);
        SubjectUser.info.infobox = infobox;
        SubjectUser.pin = pin;
        infobox.setLocation(infobox.getLocation());

        if (locationAvailable) {
            var activeUserPoints = new Array();
            var pushpinClick = Microsoft.Maps.Events.addHandler(pin, 'click', displayInfo);

            var state = (currentUser.LastLocs[0].IsSOS) ? "1" : "0";
            var lastDrawnPoint;

            for (var i = 0; i < currentUser.LastLocs.length; i++) {
                var currentstate = (currentUser.LastLocs[i].IsSOS) ? "1" : "0";
                if (state == currentstate) {
                    activeUserPoints.push(new Microsoft.Maps.Location(currentUser.LastLocs[i].Lat, currentUser.LastLocs[i].Long));
                } else {
                    if (state == "0") {
                        DrawPath(activeUserPoints, 'track');
                    }
                    else if (state == "1") {
                        DrawPath(activeUserPoints, 'sos');
                    }

                    lastDrawnPoint = activeUserPoints[activeUserPoints.length - 1];
                    state = currentstate;
                    activeUserPoints = new Array();
                    activeUserPoints.push(new Microsoft.Maps.Location(currentUser.LastLocs[i].Lat, currentUser.LastLocs[i].Long));
                }
            }
            var lastLocation = new Microsoft.Maps.Location(currentUser.LastLocs[currentUser.LastLocs.length - 1].Lat, currentUser.LastLocs[currentUser.LastLocs.length - 1].Long);

            if (lastDrawnPoint != lastLocation) {
                if (state == "0") {
                    DrawPath(activeUserPoints, 'track');
                }
                else if (state == "1") {
                    DrawPath(activeUserPoints, 'sos');
                }
            }

            FocusLatLong(currentUser.LastLocs[currentUser.LastLocs.length - 1].Lat, currentUser.LastLocs[currentUser.LastLocs.length - 1].Long, GuardianConfig.DefaultZoomLevel);
            //if (currentUser.IsSOSOn == true)
            //    DrawPath(points, 'sos');
            //else if (currentUser.IsTrackingOn == true)
            //    DrawPath(points, 'track');
        }
    }
}

function displayInfo() {
    SubjectUser.info.infobox.setOptions({ visible: true });
}
function DrawPath(points, type)//Draw PolyLine
{
    var color;
    if (type == 'sos')
        color = new Microsoft.Maps.Color(200, 237, 125, 49);

    else if (type == 'track')
        color = new Microsoft.Maps.Color(200, 112, 173, 71);
    else
        color = new Microsoft.Maps.Color(200, 0, 0, 0);
    var line = new Microsoft.Maps.Polyline(points, { strokeColor: color, strokeThickness: 4 });
    UserEntitiesLayer.push(line);
    //PortalMap.entities.push(line);
}
function FocusAll(userIndex) {
    var currentLat;
    var currentLong;
    var locationAvailable;
    for (var i = 0; i < userIndex.length; i++) {
        var currentUser = LiveUserList[userIndex[i]];

        var lastLocs = currentUser.LastLocs;
        if (lastLocs == null) {
            locationAvailable = false;
            continue;
        }
        else
            locationAvailable = true;
        if (locationAvailable) {
            currentLat = lastLocs[lastLocs.length - 1].Lat;
            currentLong = lastLocs[lastLocs.length - 1].Long;
            var currentTimeStamp = lastLocs[lastLocs.length - 1].Time;
        }
        var currentUserType;
        var pin;
        var infoboxOptions;
        var infobox;
        if (currentUser.IsSOSOn == true) {
            currentUserType = "SOS";
            pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'sos');
            infoboxOptions = { title: currentUser.Name + "(SOS)", pushpin: pin };
        }
        else {
            currentUserType = "Track";
            pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'track');
            infoboxOptions = {
                title: currentUser.Name + "(Track)", pushpin: pin
            };
        }
        var infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), infoboxOptions);
        PortalMap.entities.push(infobox);
        pin.id = userIndex[i];
        pin.infobox = infobox;
        Microsoft.Maps.Events.addHandler(pin, 'click', UserDetailedView);
    }
    FocusLatLong(currentLat, currentLong, GuardianConfig.DefaultZoomLevel);
}
function UserDetailedView(e) {
    e.target.infobox.setOptions({ visible: false });
    var id = e.target.id;
    SubjectUser.userIndex = id;
    var foundUser = $.grep(LiveUserList, function(e){ return e.PID == id; });
    if (foundUser >= 0) {
        var currentUser = foundUser[0];
        SubjectUser.userID = currentUser.UserID;
        SubjectUser.profileID = currentUser.profileID;
        SubjectUser.currentLocation.lat = currentUser.LastLocs[currentUser.LastLocs.length - 1].Lat;
        SubjectUser.currentLocation.long = currentUser.LastLocs[currentUser.LastLocs.length - 1].Long;
        FocusUserZoomLevel = GuardianConfig.DefaultZoomLevel;
        setTimeout(function () { FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.DefaultZoomLevel) }, 500);
        getSubjectUser();
    }

}
function FocusAllSOS() {
    //HelpPane();
    removeEntities();
    //FocusAll(SOSUserIndex);
    SubjectUser.userIndex = -1;
}
function FocusAllTrack() {
    //HelpPane();
    removeEntities();
    //FocusAll(TrackUserIndex);
    SubjectUser.userIndex = -1;
}
function FocusLatLong(lat, long, zoomLevel) {
    PortalMap.setView({ zoom: zoomLevel, center: new Microsoft.Maps.Location(lat, long) });
}

function DynamicGetTotalUsersCount() {
    var myUrl, id;
    $("#errorLabel").empty();

    if (Profile.type == 'user') {
        myUrl = GuardianConfig.ServiceURL + '/GetUserBuddiesCount/' + Profile.userID + '/' + Date.now();
    } else if (Profile.type == 'group') {
        myUrl = GuardianConfig.ServiceURL + '/GetGroupMembersCount/' + Profile.groupID + '/' + Date.now();
    } else
        throw (0);
    $.ajax({
        url: myUrl,
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        success: function (data) {
            if (data) {
                var totalMembersCount = data;
                $('#InactiveCount').html(totalMembersCount);
            }
        },
        error: function (e) {
            //  $("#errorLabel").text("Error : " + e.statusText);
        }
    });
}

function LoadUsers() {
    var myUrl;

    var searchKey = $.trim($("#searchKeyName").val());
    if (searchKey == '')
        searchKey = 'Empty';

    if (Profile.type == 'user') {
        myUrl = GuardianConfig.ServiceURL + '/GetFilteredLocateBuddies/' + Profile.userID + '/' + searchKey + '/' + Date.now();
    } else if (Profile.type == 'group') {
        myUrl = GuardianConfig.ServiceURL + '/GetFilteredGroupMembers/' + Profile.groupID + '/' + searchKey + '/' + Date.now();
    } else
        throw (0);
    $.ajax({
        url: myUrl,
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        success: function (data) {
            if (data) {
                if (data.List) {
                    var UserList = data.List;

                    UserList = UserList.sort();

                    $("#searchUsers").empty();

                    for (var i = 0; i < UserList.length; i++) {
                        $("#searchUsers").append($("<option></option>").val(UserList[i].PID).html(UserList[i].N + ", " + UserList[i].M));
                    }
                    if(Profile.type=='user')
                    {
                        $("#searchUsers").append($("<option></option>").val(Profile.profileID).html('Self'));
                    }
                }
            }
        },
        error: function (e) {
            //  $("#errorLabel").text("Error : " + e.statusText);
        }
    });
}

function DynamicGetLiveUsers() {
    var myUrl, id;
    $("#errorLabel").empty();
    //TotalUsers = LiveUserList.length;
    //PreviousSOSIndexCount = SOSUserIndex.length;

    if (Profile.type == 'user') {
        myUrl = GuardianConfig.ServiceURL + '/GetLiveLocateBuddies/' + Profile.userID + '/' + Date.now();
    } else if (Profile.type == 'group') {        
        myUrl = GuardianConfig.ServiceURL + '/GetLiveGroupMembers/' + Profile.groupID + '/' + Date.now();
    } else
        throw (0);
    $.ajax({
        url: myUrl,
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        success: function (data) {
            if (data) {
                if (data.List) {
                    LiveUserList = data.List;

                    LiveUserList = LiveUserList.sort();
                    SOSUserIndex.splice(0, SOSUserIndex.length);
                    TrackUserIndex.splice(0, TrackUserIndex.length);
                    
                    var LatestSOSUserList = new Array();

                    SOSoutput = "";
                    Trackoutput = "";
                    for (var i = 0; i < LiveUserList.length; i++) {

                        if (LiveUserList[i].S == 2) {
                            SOSUserIndex.push(LiveUserList[i].PID);
                            SOSoutput += "<li id='" + LiveUserList[i].PID + "'>" + LiveUserList[i].N + '</li>';
                            LatestSOSUserList.push(LiveUserList[i]);
                        }
                        else if (LiveUserList[i].S == 1) {
                            TrackUserIndex.push(LiveUserList[i].PID);
                            Trackoutput += "<li id='" + LiveUserList[i].PID + "'>" + LiveUserList[i].N + '</li>';
                        }
                    }

                    DisplayUserList();
                    PopupNewSOSUsers(LatestSOSUserList);
                }
            }
        },
        error: function (e) {
            //  $("#errorLabel").text("Error : " + e.statusText);
        }
    });
}

function PopupNewSOSUsers(LatestSOSUserList) {
    var newSOSUserList = new Array();
    if (CurrentSOSUserList.length > 0 && LatestSOSUserList.length > 0) {        
        for (var i = 0; i < LatestSOSUserList.length; i++) {
            var found = false;
            for (var j = 0; j < CurrentSOSUserList.length; j++) {
                if (LatestSOSUserList[i].PID == CurrentSOSUserList[j].PID) {
                    found = true;
                }
            }
            if (!found)
                newSOSUserList.push(LatestSOSUserList[i]);
        }       
    }
    else if (LatestSOSUserList.length > 0) {
        newSOSUserList = LatestSOSUserList;
    }

    if (LatestSOSUserList.length == 0 || LatestSOSUserList.length < CurrentSOSUserList.length) {
        CurrentSOSUserList = LatestSOSUserList;
    }

    if (newSOSUserList.length > 0) {
        CurrentSOSUserList = LatestSOSUserList;

        var usrType = getSession('userTypeAuth');
        var sosLabel = '';

        if (usrType == 'a') {
            sosLabel = '<table border="1"><tr><th width="40%">Name</th><th width="25%">MobileNumber</th><th width="25%">AssignTo</th><th width="10%">Action</th></tr>';
            for (var j = 0; j < newSOSUserList.length; j++) {
                var assignedName = '';//to avoid null displays
                if (newSOSUserList[j].AT)
                    assignedName = newSOSUserList[j].AT;
                sosLabel = sosLabel + '<tr><td width="40%">' + newSOSUserList[j].N + '</td>';
                sosLabel = sosLabel + '<td width="25%">' + newSOSUserList[j].M + '</td>';
                sosLabel = sosLabel + '<td width="25%"><input type="text" id="txt' + j + '" value="' + assignedName + '" maxlength="100" /></td>';
                sosLabel = sosLabel + '<td width="10%"><button type="button" onclick="javascript: DispatchClick(\'txt' + j + '\',\'' + newSOSUserList[j].PID + '\',\'' + newSOSUserList[j].SID + '\');">Dispatch</button></td></tr>';
            }
            sosLabel = sosLabel + '<tr><td width="30%" align="center" colspan="3"><button type="button" onclick="javascript: CloseTheDialog();"><B>Close</B></button></td></tr></table>';
        }
        else {
            sosLabel = '<table border="1"><tr><th width="60%">Name</th><th width="40%">MobileNumber</th></tr>';
            for (var j = 0; j < newSOSUserList.length; j++) {
                sosLabel = sosLabel + '<tr><td width="60%">' + newSOSUserList[j].N + '</td>';
                sosLabel = sosLabel + '<td width="40%">' + newSOSUserList[j].M + '</td>';
            }
            sosLabel = sosLabel + '<tr><td width="30%" align="center" colspan="3"><button type="button" onclick="javascript: CloseTheDialog();"><B>Close</B></button></td></tr></table>';
        }
        $('#sosDiv').html(sosLabel);
        playSound("Content/Images/NewSOS.wav");
        $('#modalSOS').show();
    }
}

function PutPushPin(name, lat, long, flagType) {
    var iconUrl = "Content/Images/sos.png";
    var label;
    if (flagType == 'sos') {
        iconUrl = "Content/Images/sos.png";
        label = 'T';
    } else if (flagType == 'track') {
        iconUrl = "Content/Images/track.png";
        label = 'T';
    } else if (flagType == 'marshal') {
        iconUrl = "Content/Images/buddy.png";
        label = 'M';
    } else if (flagType == 'trackStart') {
        iconUrl = "Content/Images/track.png";
        label = 'S';
    } else if (flagType == 'sosStart') {
        iconUrl = "Content/Images/sos.png";
        label = 'S';
    }
    var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(lat, long), { text: label, icon: iconUrl, width: 24, height: 40 });
    if (flagType != 'marshal') //Don't push Buddy Pin on Portal Map
        UserEntitiesLayer.push(pin);
    //PortalMap.entities.push(pin);

    return pin;


}
function CallEvent() {
    alert('Calling...');
}
function SMSEvent(userIndex) {
    $("#smsPopup").dialog({
        dialogClass: "popup",
        modal: true,
        resizable: false,
        draggable: true,
        width: 350,
        height: 180,
        zIndex: 2,
        top: 100,
        left: 300,
        position: "absolute"
    });
}
function CloseDialog() {
    $('.ui-dialog-content:visible').dialog('close');

}
function CloseTheDialog() {
    //$('#modalSOS').dialog().dialog('close');
    $('#modalSOS').hide();
}
function DispatchClick(assignedToID, prfId, sessId) {    
    var assignedTo = $('#' + assignedToID + '').val();
    assignedTo = $.trim(assignedTo);
    if (assignedTo == '') {
        alert('No Person has been assigned. Please enter the name');
        return;
    }

    var saveAssignedUrl = GuardianConfig.ServiceURL + '/SaveDispatchInfo';
    var postData = {};
    postData.ProfileID = prfId;
    postData.SessionID = sessId;
    postData.GroupID = Profile.groupID;
    postData.AssignedTo = assignedTo;
    var jsonPostData = JSON.stringify(postData);

    $.ajax({
        type: "POST",
        url: saveAssignedUrl,
        dataType: "json",
        data: jsonPostData,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: "application/json; charset=utf-8",
        success: function (data) {            
            if (data && (data.ResultType == 0)) {
                alert('Dispatch information has been saved successfully.');
            } else {
                alert('Failed to save the dispatch information! Please try again later...');
            }
        },
        error: function (data) {
        }
    });
}
$(function () {

    $("#datepickerFrom").datepicker({
        changeMonth: true,
        changeYear: true,
    });
    $("#datepickerTo").datepicker({
        changeMonth: true,
        changeYear: true
    });
});

function onlaunchmarshalmgmt(e) {
    $('#modal> div.content > div.content-main').html($(this).hasClass('image') ? ('<img src="' + $(this).attr('href') + '"/>') : ('<iframe src="' + $(this).attr('href') + '" width="99.6%" height="99.6%"></iframe>'));
    $('#modal').show();
    e.preventDefault();
    return false;
}
function onlaunchmarshalmgmtclose() {
    $('#modal').hide();
    $('#modal > div.content > div.content-main').html('');
}
function clearUserList() {
    $('#lstSOS').html("");
    $('#lstTrack').html("");
    $('#lstinactive').html("");

    $('#SOSCount').html(0);
    $('#ActiveCount').html(0);
    $('#InactiveCount').html(0);
    $('#IncidentsCount').html(0);

    LiveUserList = [];
    SOSUserIndex.splice(0, SOSUserIndex.length);
    TrackUserIndex.splice(0, TrackUserIndex.length);
    InActiveUserIndex.splice(0, InActiveUserIndex.length);
    TrackUserIndex.slice(0, 0);
    SOSUserIndex.splice(0, 0)
    AdminID = 0;
    GroupID = 0;

    lat = GuardianConfig.DefaultLat;
    long = GuardianConfig.DefaultLang;
    clearPortalMap();
    return;
}

function groupSOSClick() {
    IncidentDisplayFlag = false;

    $("#SOStile").addClass("selected");
    $("#Activetile").removeClass("selected");
    $("#Inactivetile").removeClass("selected");
    $("#Incidentstile").removeClass("selected");

    $("#lstSOSdiv").removeClass("disappear");
    $("#lstTrackdiv").removeClass("appear");
    $("#lstinactivediv").removeClass("appear");
    $("#lstincidentdiv").removeClass("appear");

    $("#lstSOSdiv").addClass("appear");
    $("#lstTrackdiv").addClass("disappear");
    $("#lstinactivediv").addClass("disappear");
    $("#lstincidentdiv").addClass("disappear");
}

function groupActiveClick() {
    IncidentDisplayFlag = false;
    $("#SOStile").removeClass("selected");
    $("#Activetile").addClass("selected");
    $("#Inactivetile").removeClass("selected");
    $("#Incidentstile").removeClass("selected");

    $("#lstTrackdiv").removeClass("disappear");
    $("#lstSOSdiv").removeClass("appear");
    $("#lstinactivediv").removeClass("appear");
    $("#lstincidentdiv").removeClass("appear");

    $("#lstTrackdiv").addClass("appear");
    $("#lstSOSdiv").addClass("disappear");
    $("#lstinactivediv").addClass("disappear");
    $("#lstincidentdiv").addClass("disappear");
}

function groupInactiveClick() {
    IncidentDisplayFlag = false;
    $("#SOStile").removeClass("selected");
    $("#Activetile").removeClass("selected");
    $("#Inactivetile").addClass("selected");
    $("#Incidentstile").removeClass("selected");

    $("#lstinactivediv").removeClass("disappear");
    $("#lstSOSdiv").removeClass("appear");
    $("#lstTrackdiv").removeClass("appear");
    $("#lstincidentdiv").removeClass("appear");

    $("#lstinactivediv").addClass("appear");
    $("#lstSOSdiv").addClass("disappear");
    $("#lstTrackdiv").addClass("disappear");
    $("#lstincidentdiv").addClass("disappear");

    if (getSession('userTypeAuth') == 'a')
        SubjectUser.userIndex = -1;
}

function groupIncidentsClick() {
    IncidentDisplayFlag = true;
    $("#SOStile").removeClass("selected");
    $("#Activetile").removeClass("selected");
    $("#Inactivetile").removeClass("selected");
    $("#Incidentstile").addClass("selected");
    
    $("#lstSOSdiv").removeClass("appear");
    $("#lstTrackdiv").removeClass("appear");
    $("#lstinactivediv").removeClass("appear");
    
    $("#lstSOSdiv").addClass("disappear");
    $("#lstTrackdiv").addClass("disappear");
    $("#lstinactivediv").addClass("disappear");

    if (ShowIncidents) {
        $("#lstincidentdiv").removeClass("disappear");
        $("#lstincidentdiv").addClass("appear");
        getIncidentsAndStartProcess();
    }
}

function playSound(soundfile) {
    document.getElementById("NewSOS").innerHTML =
      "<embed src=\"" + soundfile + "\" hidden=\"true\" autostart=\"true\" loop=\"false\" />";
}

function busyOn() {
    $(".spinnerBackground").addClass("show");
}

function busyOff() {
    $(".spinnerBackground").removeClass("show");
}