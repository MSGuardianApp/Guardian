var GroupID; 
var AdminID;
function deleteMarshal() {
    var id = $(this).attr('id');
    var deleteMarshalUrl = GuardianServiceURL + '/GroupService.svc/DeleteMarshal/';
    for (i = 0; i < MarshalList.length; i++) {
        if (MarshalList[i].ProfileID == id) {
            if (MarshalList[i].LocateBuddies != null) {
                alert("Marshal has buddies assigned to it. Cant be deleted!!");
                return;
            }
        }
    }
    $.ajax({

        url: deleteMarshalUrl + AdminId + '/' + GroupId + '/' + ID,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            if (data.ResultType == 0) {
                displayMarshalList();
                return true;
            }
            else {
                alert("OOPS! marshal not deleted");
                return;
            }
        },
        error: function (data) {
            alert("marshal not deleted");
            return;
        }

    });
}
function displayMarshalList() {
    //MarshalList = [{ "DataInfo": null, "Email": "renukasharath@live.com", "FBAuthID": null, "FBID": null, "LiveDetails": null, "MobileNumber": "+917702690005", "Name": "renuka", "RegionCode": null, "UserID": "d20da772-9bc5-48db-80dd-52c7a61eba45", "IsSOSOn": true, "IsTrackingOn": true, "LastLocs": [{ "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581458804)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581611992)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "45.435943700000000000", "Long": "-121", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374578575943)\/" }], "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "SOSToken": null, "TinyURI": null, "TrackingToken": null, "LocateBuddies": null }];
    GroupID=getSession('GroupID');
    AdminID = getSession('AdminID');
    getMarshals(GroupID,false);
     var rowhtml = '';
    $.each(MarshalList, function (index, marshal) {
        var id = marshal.ProfileID;
        rowhtml += '<tr><td>' + marshal.Name + '</td><td>' + marshal.Email + '</td><td>' + marshal.MobileNumber + '</td>';
        rowhtml += '<td><a href="#" data-LinkAction="DeleteCard" onclick="deleteMarshal()" id="' + id + '"><img class="DeleteImage" src="/Content/Images/delete.png" height="32" width="32"/></a></td></tr>';
    });
    $("#marshalTableDiv").html('<table class="marshalTable"><thead><tr><td>Name</td><td>LiveId</td><td>Phone Number</td><td></td></tr></thead>'
                              + '<tbody>' + rowhtml + '</tbody></table>');
    $("#marshalTableDiv").show();
}