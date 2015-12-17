var MyBuddiesList;
var HelpEnitiesRequestRetry = { "id": -1, "flag": false };
function GetMyBuddies() {
    ShowBuddies();

}
function ShowBuddies() {
    var currentLat, currentLong;
    if (BuddyLayer)
        BuddyLayer.clear();
    BuddyLayer = new Microsoft.Maps.EntityCollection();
    PortalMap.entities.push(BuddyLayer);

    for (i = 0; i < MyBuddiesList.length; i++) {
        var currentMemeber = MyBuddiesList[i].Membership;
        currentLat = currentMemeber.LastLocs[currentMemeber.LastLocs.length - 1].Lat;
        currentLong = currentMemeber.LastLocs[currentMemeber.LastLocs.length - 1].Long;
        var myDescription = 'TimeStamp : ' + currentMemeber.LastLocs[currentMemeber.LastLocs.length - 1].Time;
        var pin = PutPushPin(currentMemeber.Name, currentLat, currentLong, 'buddy');
        BuddyLayer.push(pin);
        var infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLat, currentLong), {
            title: currentMemeber.Name, description: myDescription, actions: [{ label: 'Call', eventHandler: CallEvent },
    { label: 'SMS', eventHandler: SMSEvent }], pushpin: pin
        });
        BuddyLayer.push(infobox);
    }
}

function ToggleTracking() {
    SubjectUser.userIndex = -1;
    clearPortalMap();
    $("#trackingsectiondiv").removeClass("disappear");
    $("#trackingsectiondiv").addClass("appear");
    $("#attachmentsectiondiv").removeClass("appear");
    $("#attachmentsectiondiv").addClass("disappear");

    $("#historyDiv").removeClass("disappear");
    $("#historyDiv").addClass("appear");

    var today = new Date();
    var today1 = (today.getMonth() + 1 + "/" + today.getDate() + "/" + today.getFullYear());

    if ($("#datepickerFrom").val() == "")
        $("#datepickerFrom").val(today1);
    if ($("#datepickerTo").val() == "")
        $("#datepickerTo").val(today1);
}
function ToggleAttachments() {

    $("#attachmentsectiondiv").removeClass("disappear");
    $("#attachmentsectiondiv").addClass("appear");
    $("#trackingsectiondiv").removeClass("appear");
    $("#trackingsectiondiv").addClass("disappear");

    $("#historyDiv").removeClass("disappear");
    $("#historyDiv").addClass("appear");
}

function hideHistoryDiv() {
    $("#historyDiv").removeClass("appear");
    $("#historyDiv").addClass("disappear");
}
function HelpPane(e) {
    //$("#rightpanelhideout").removeClass("appear");
    //$("#rightpanelhideout").addClass("disappear");
    //$("#rightpanelhidein").addClass("appear");
    //$("#help").removeClass("disappear");
    //$(".mapview").removeClass("mapexpand");

    searchHelp('police', true);
}
function HideHelpPane() {

    $("#rightpanelhidein").removeClass("appear");
    $("#rightpanelhidein").addClass("disappear");
    $("#rightpanelhideout").addClass("appear");
    $("#help").addClass("disappear");
    $(".mapview").addClass("mapexpand");

}

function GetHelp(id) {
    if (SubjectUser.userIndex == -1) {
        alert('Please choose a user before you find help');
        return;
    }
    if (HelpEntitiesLayer)
        HelpEntitiesLayer.clear();
    HelpEnitiesRequestRetry.id = id;
    searchHelp(id, true);
}
function searchHelp(id, retryFlag) {
    var what;
    if (id == 'police')
        what = 'Police Stations';
    else if (id == 'hospital')
        what = 'hospital';
    else if (id == 'school')
        what = 'school';
    else if (id == 'govt')
        what = 'Government Office';
    else
        what = 'Police Stations + hospital + school +govt';
    var where = SubjectUser.info.address.trim();

    if (retryFlag == false) {
        HelpEnitiesRequestRetry.id = -1; // for the refernece of retry Logic TOSTOP IT
        where = getSearchfriendlyQuery(where);
    }
    if (SearchManager) {
        searchRequest(what, where);
    }
    else {
        Microsoft.Maps.loadModule('Microsoft.Maps.Search', { callback: searchRequest(what, where) });
    }
}

function getSearchfriendlyQuery(keyWords) {
    //removing Indian Postal Code
    var regOrderNo = new RegExp("\\d{6}");
    var pinCode = regOrderNo.exec(keyWords);
    if (pinCode == null) {
        return keyWords;
    }
    var pinCodeStart = keyWords.indexOf(regOrderNo.exec(keyWords));
    keyWords = keyWords.substring(0, pinCodeStart - 1).trim() + keyWords.substring(pinCodeStart + 6, keyWords.length).trim();
    return keyWords;
}
function searchRequest(what, where) {
    var request =
        {
            what: what,
            where: where.replace(',', ' '),
            count: 10,
            startIndex: 0,
            bounds: PortalMap.getBounds(),
            callback: search_onSearchSuccess,
            errorCallback: search_onSearchFailure

        };
    SearchManager.search(request);
}
function search_onSearchSuccess(result) {
    var searchResults = result && result.searchResults;
    if (searchResults) {
        for (var i = 0; i < searchResults.length; i++) {
            search_createMapPin(searchResults[i]);
        }
        if (result.searchRegion && result.searchRegion.mapBounds) {
        }
        else {
            //No results returned, Please try after sometime.');
        }

        if (searchResults.length > 0) {
            if (SubjectUser.userIndex != -1)
                FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.DefaultZoomLevel);
            else
                FocusLatLong(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long, GuardianConfig.HelpDefualtZoomLevel);//for initial hyd view
        } else {
            // failed Try once again MAX RETRY ==1
            if (HelpEnitiesRequestRetry.id != -1)
                searchHelp(HelpEnitiesRequestRetry.id, false);
        }

    }
}
function search_createMapPin(result) {
    if (result) {
        var pin = new Microsoft.Maps.Pushpin(result.location, null);
        HelpEntitiesLayer.push(pin);
        var currInfobox = new Microsoft.Maps.Infobox(
       result.location,
       {
           title: result.name,
           description: [result.address, result.city, result.state, result.country, result.phone].join(' '),
           showPointer: true,
           titleAction: null,
           titleClickHandler: null,
           visible: true,
           pushpin: pin,
           actions: [{ label: 'Directions', eventHandler: directionFromHelpToUser, id: result.location.latitude + ',' + result.location.longitude }]
       });
        HelpEntitiesLayer.push(currInfobox);
    }
}
function directionFromHelpToUser() {

    var id = this.id;
    var latLong = id.split(',');

    var helpLocation = new Microsoft.Maps.Location(latLong[0], latLong[1]);
    var userLocation = new Microsoft.Maps.Location(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long);
    createDrivingRoute(helpLocation, userLocation);
}
function search_showInfoBox(result) {
    var currInfobox = new Microsoft.Maps.Infobox(
        result.location,
        {
            title: result.name,
            description: [result.address, result.city, result.state, result.country, result.phone].join(' '),
            showPointer: true,
            titleAction: null,
            titleClickHandler: null
        });
    currInfobox.setOptions({ visible: true });
    HelpEntitiesLayer.push(currInfobox);
}
function search_onSearchFailure(result) {
    //alert('Network Problem !!');
}



