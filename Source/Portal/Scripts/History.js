var geoLocationList = [];
var attachmentList = [];
var currentSessionID = '';
function GetUserHistory() {
    $('#historyData').empty();

    var historyUrl, id;
    var fromDate, toDate, profileID;
    $("#reqFrom").hide();
    $("#reqSearchUser").hide();
    $("#reqTo").hide();
    profileID = localStorage["searchUserID"];
    fromDate = $("#datepickerFrom").datepicker("getDate");
    toDate = $("#datepickerTo").datepicker("getDate");
    if (profileID == null || profileID == '') {
        $("#reqSearchUser").show();
        return;
    }
    if (fromDate == null || fromDate == '') {
        $("#reqFrom").show();
        return;
    }
    else if (toDate == null || toDate == '') {
        $('#reqTo').show();
        return;
    }
    fromDate = DateFormat(fromDate);
    toDate = DateFormat(toDate);

    var historyUrl = GuardianConfig.ServiceURL + '/GetHistorySessions/' + profileID + '/' + fromDate + '/' + toDate + '/' + Date.now();
    $.ajax({
        url: historyUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        timeout: GuardianConfig.ServiceTimeout,
        success: function (data) {
            if (data && data.List) {
                
                populateHistoryDetail(data.List, profileID);
            }
        },
        error: function (e) {
            //  alert('Error occurred!! ' + e.statusText);
        }
    });
}
function populateHistoryDetail(item, profileID) {
    geoLocationList = [];
    $('#historyData').empty();
    var r = new Array(), j = -1;
    r[++j] = '<tr>';
    r[++j] = '<th class="trackingdetailsheader hcol1">Start Time</th>';
    r[++j] = '<th class="trackingdetailsheader hcol2">End Time</th>';
    r[++j] = '<th class="trackingdetailsheader hcol3">Duration(secs)</th>';
    r[++j] = '<th class="trackingdetailsheader hcol4" colspan="2">Action</th>';
    r[++j] = '</tr>';
    for (var i = 0; i < item.length; i++) {
        var mediaCount = 0;
        var geoInstances = item[i].GeoInstances;
        geoLocationList.push(geoInstances);
        var duration = (item[i].PeriodEndDate != 'Unknown') ? datediff(item[i].PeriodStartDate, item[i].PeriodEndDate, 'seconds') : 'NA';
        var pEndDate = (item[i].PeriodEndDate != 'Unknown') ? DateDeserialize(item[i].PeriodEndDate) : item[i].PeriodEndDate;
        r[++j] = '<tr><td class="trackingdetailsdatarow rcol1">';
        r[++j] = DateDeserialize(item[i].PeriodStartDate);
        r[++j] = '</td><td class="trackingdetailsdatarow rcol2">';
        r[++j] = pEndDate;
        r[++j] = '</td><td class="trackingdetailsdatarow rcol3">';
        r[++j] = duration;        
        r[++j] = '</td><td class="trackingdetailsdatarow rcol5"><span><a  href="#" class="" onclick="GetUserHistoryLocation(\'' + item[i].SessionID + '\',' + profileID + ',\'' + pEndDate + '\');">View</a></span>';
        if (item[i].IsEvidenceAvailable)
            r[++j] = ' | <a style="cursor: pointer;" onclick="GetAttachments(\'' + item[i].SessionID + '\');" ><img width="15px" height="15px"  src="Content/Images/Attachment.png" alt="View Attachments" /></a>';
        r[++j] = '</td><td class="trackingdetailsdatarow rcol5"><span><a  href="#" class="" onclick="DeleteHistoryDetails(\'' + item[i].SessionID + '\',' + profileID + ');">Delete</a></span>';
        r[++j] = '</td></tr>';
    }
    $('#historyData').html(r.join(''));

}
function GetAttachments(sessionID) {
    if (currentSessionID == sessionID) {
        if (mediaURIList.length > 0) {
            populateHistoryAttachments(mediaURIList);
            ToggleAttachments();
        }
        else
            alert('There are no evidences available.');
    }
}
function DeleteHistoryDetails(sessionId, profileID) {    
    var groupID = getSession('GroupID');
    if (groupID == null || groupID == '')
        groupID = '0';

    var historyUrl = GuardianConfig.ServiceURL + '/DeleteHistoryDetails/' + profileID + '/' + sessionId + '/' + groupID + '/' + Date.now();
    $.ajax({
        url: historyUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        timeout: GuardianConfig.ServiceTimeout,
        success: function (data) {
            if (data) {                
                GetUserHistory();
            }
        },
        error: function (e) {
            alert('Unable to delete the history for SessionID: ' + sessionId + ',\n Please contact administrator.');
        }
    });
}
function GetUserHistoryLocation(sessionId, profileID, currentTimeStamp) {
    clearPortalMap();
    var historyUrl = GuardianConfig.ServiceURL + '/GetHistoryLocations/' + profileID + '/' + sessionId + '/' + Date.now();
    $.ajax({
        url: historyUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        timeout: GuardianConfig.ServiceTimeout,
        success: function (data) {
            if (data && data.List && data.List.length > 0) {
                currentSessionID = sessionId;
                var sosPoints = new Array();
                var trackPoint = new Array();
                mediaURIList = new Array();
                var historyLatLong = data.List;

                var endLat, endLong;

                var lastLat;
                var lastLong;

                var points = new Array();
                var state = (historyLatLong[0].IsSOS) ? "1" : "0";
                lastLat = historyLatLong[0].Lat;
                lastLong = historyLatLong[0].Long;

                if (state == "0") {
                    PutPushPin("", lastLat, lastLong, 'trackStart');
                } else {
                    PutPushPin("", lastLat, lastLong, 'sosStart');
                }
                var lastDrawnPoint;
                for (var i = 0; i < historyLatLong.length; i++) {
                    if (historyLatLong[i].MediaUri != null) {
                        if (historyLatLong[i].MediaUri.toString().length > 10)
                            mediaURIList.push(historyLatLong[i].MediaUri);
                    }

                    var currentstate = (historyLatLong[i].IsSOS) ? "1" : "0";
                    if (state == currentstate) {
                        points.push(new Microsoft.Maps.Location(historyLatLong[i].Lat, historyLatLong[i].Long));
                        if (historyLatLong[i].Lat != '') {
                            lastLat = historyLatLong[i].Lat;
                            lastLong = historyLatLong[i].Long;
                        }
                    } else {
                        if (state == "0") {
                            DrawPath(points, 'track');
                        }
                        else if (state == "1") {
                            DrawPath(points, 'sos');
                        }

                        lastDrawnPoint = points[points.length - 1];
                        state = (historyLatLong[0].IsSOS) ? "1" : "0";
                        points = new Array();
                        points.push(new Microsoft.Maps.Location(historyLatLong[i].Lat, historyLatLong[i].Long));
                        if (historyLatLong[i].Lat != '') {
                            lastLat = historyLatLong[i].Lat;
                            lastLong = historyLatLong[i].Long;
                        }
                    }
                }
                historyTab = true;
                var lastLocation = new Microsoft.Maps.Location(historyLatLong[historyLatLong.length - 1].Lat, historyLatLong[historyLatLong.length - 1].Long);

                if (lastDrawnPoint != lastLocation) {
                    if (state == "0") {
                        DrawPath(points, 'track');
                    }
                    else if (state == "1") {
                        DrawPath(points, 'sos');
                    }
                }
                getHistoryUserAddress(lastLat, lastLong);
                HistoryFocusUser(profileID, lastLat, lastLong, currentTimeStamp);
            }
            else {
                alert('No locations to show');
            }
        },
        error: function (e) {
            //  alert('Error occurred!! ' + e.statusText);
        }
    });
}
function populateHistoryAttachments(item) {
    var li = new Array(), j = -1;
    $('#attachmentsectiondiv').empty();

    li[++j] = '<ul id="attachmentUL" class="attachmentlist">';
    for (var k = 0; k < item.length; k++) {
        if (item[k].toString() != "") {
            li[++j] = '<li><a target="_blank" href="' + item[k] + '"><img src="' + item[k] + '" width="60" height="60" /></a></li>';
        }
    }
    li[++j] = '</ul>';
    $('#attachmentsectiondiv').html(li.join(''));
    //$("area[rel^='Evidence']").Evidence();
    //$(".gallery:first a[rel^='Evidence']").Evidence({ animation_speed: 'normal', theme: 'light_square' });

}
function ClearUserHistory() {
   // $("#searchUsers").empty();
    $("#datepickerFrom").empty();
    $("#datepickerTo").empty();
    $("#searchKeyName").empty();
    $("#searchKeyName").val("");
    $("#searchUsers").empty();
    $("#searchUsers").val("");
    $("#datepickerFrom").val("");
    $("#datepickerTo").val("");
    $('#historyData').empty();
    $('#attachmentsectiondiv').empty();    
}

function HistoryFocusUser(profileID, lat, long, currentTimeStamp) {
    var currentUser;
    var groupID = getSession('GroupID');
    if (groupID == null || groupID == '')
        groupID = '0';
    profileUrl = GuardianConfig.ServiceURL + '/GetProfileLiteByProfileID/' + profileID + '/' + groupID;
    $.ajax({
        url: profileUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        timeout: 10000,
        success: function (data) {
            if (data) {
                historyInfo(data, lat, long, currentTimeStamp);
            }

        },
        error: function () {
            //$("#errorLabel").text("Error : " + e.statusText);


        }
    });

}
function historyInfo(currentUser, currentLat, currentLong, currentTimeStamp) {
    var infoboxOptions;
    var infobox;

    if (currentUser.IsSOSOn == true) {
        currentUserType = "SOS";
        if (locationAvailable)
            pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'sos');
        SelectedUserInfoboxDescription = "TimeStamp : " + currentTimeStamp + "<br/> Contact : " + currentUser.MobileNumber + "<br/> Location : " + SubjectUser.info.address;
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

        infoboxOptions = {
            offset: new Microsoft.Maps.Point(0, 40), visible: true,
            title: currentUser.Name + "(TRACK)", description: SelectedUserInfoboxDescription, zIndex: 0, visible: true, actions: [{ label: 'Get Help', eventHandler: HelpPane }]
        };
        infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), infoboxOptions);
    }
    else {
        currentUserType = "InActive";
        pin = PutPushPin(currentUser.Name, currentLat, currentLong, 'track');
        SelectedUserInfoboxDescription = "TimeStamp : " + currentTimeStamp + "<br/> Contact : " + currentUser.MobileNumber + "<br/> Location : " + historyAddress;

        infoboxOptions = {
            offset: new Microsoft.Maps.Point(0, 40), visible: true,
            title: currentUser.Name + "(INACTIVE)", description: SelectedUserInfoboxDescription, zIndex: 0, visible: true, actions: [{ label: 'Get Help', eventHandler: HelpPane }]
        };
        infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), infoboxOptions);
    }


    UserEntitiesLayer.push(infobox);
    //PortalMap.entities.push(infobox);
    SubjectUser.info.infobox = infobox;
    SubjectUser.pin = pin;
    infobox.setLocation(infobox.getLocation());

    FocusLatLong(currentLat, currentLong, GuardianConfig.DefaultZoomLevel);
}