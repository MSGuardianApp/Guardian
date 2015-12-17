var IncidentEntitiesLayer = null;
var IncidentDisplayFlag = false;
var Clusters = [];
var Incidents;
var cluster;
var AllIncidentsIndex = new Array();
var AccidentIncidentsIndex = new Array();
var TeaseIncidentsIndex = new Array();
var RaggingIncidentsIndex = new Array();
var RobberyIncidentsIndex = new Array();
var OtherIncidentsIndex = new Array();
var CalledIncidentEvent;

function getIncidentsAndStartProcess() {
    // incidentsInit(Incidents);
    var today = new Date();
    var today1 = (today.getMonth() + 1 + "/" + today.getDate() + "/" + today.getFullYear());

    if ($("#IncidentStartDate").val() == "")
        $("#IncidentStartDate").val(today1);
    if ($("#IncidentEndDate").val() == "")
        $("#IncidentEndDate").val(today1);

    var startDate = $("#IncidentStartDate").datepicker("getDate");
    var endDate = $("#IncidentEndDate").datepicker("getDate");
    startDate = DateFormat(startDate);
    endDate = DateFormat(endDate);

    var myUrl = GuardianConfig.ServiceURL + '/GetIncidentsbyDates/' + Date.now() + '/' + startDate + '/' + endDate; //Pseudo 1 

    $.ajax({
        url: myUrl,
        dataType: 'json',
        timeout: GuardianConfig.ServiceTimeout,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        success: function (data) {
            if (data) {
                if (data.List) {
                    incidentsInit(data);
                    IncidentsAll();
                }
            }
        },
        error: function (e) {
            //alert(JSON.stringify(e));
        }
    });
}

function incidentsInit(serviceResult) {
    Incidents = serviceResult.List;
    $('#IncidentsCount').html(Incidents.length);
    CalledIncidentEvent = null;
    if (IncidentEntitiesLayer == null) {
        IncidentEntitiesLayer = new Microsoft.Maps.EntityCollection();

        PortalMap.entities.push(IncidentEntitiesLayer);
    } else {
        IncidentEntitiesLayer.clear();
    }
    AllIncidentsIndex.splice(0, AllIncidentsIndex.length);
    AccidentIncidentsIndex.splice(0, AccidentIncidentsIndex.length);
    TeaseIncidentsIndex.splice(0, TeaseIncidentsIndex.length);
    RaggingIncidentsIndex.splice(0, RaggingIncidentsIndex.length);
    RobberyIncidentsIndex.splice(0, RobberyIncidentsIndex.length);
    OtherIncidentsIndex.splice(0, OtherIncidentsIndex.length);
    if (Incidents != null) {
        for (var i = 0; i < Incidents.length && (AllIncidentsIndex.push(i)) ; i++) {
            if (Incidents[i].Type.toLowerCase() == "accident")
                AccidentIncidentsIndex.push(i);
            else if (Incidents[i].Type.toLowerCase() == "harassment")
                TeaseIncidentsIndex.push(i);
            else if (Incidents[i].Type.toLowerCase() == "ragging")
                RaggingIncidentsIndex.push(i);
            else if (Incidents[i].Type.toLowerCase() == "robbery")
                RobberyIncidentsIndex.push(i);
            else
                OtherIncidentsIndex.push(i);
        }
    }
}

function IncidentsAll() {
    CalledIncidentEvent = 'IncidentsAll()';
    clusterMappingInit(AllIncidentsIndex);
    plotClusters();

}

function incidentMedia() {
    var url;

}
function IncidentsAccident() {
    CalledIncidentEvent = 'IncidentsAccident()';
    clusterMappingInit(AccidentIncidentsIndex);
    plotClusters();
}

function IncidentsTease() {
    CalledIncidentEvent = 'IncidentsTease()';
    clusterMappingInit(TeaseIncidentsIndex);
    plotClusters();
}

function IncidentsRagging() {
    CalledIncidentEvent = 'IncidentsRagging()';
    clusterMappingInit(RaggingIncidentsIndex);
    plotClusters();
}

function IncidentsRobbery() {
    CalledIncidentEvent = 'IncidentsRobbery()';
    clusterMappingInit(RobberyIncidentsIndex);
    plotClusters();
}

function IncidentsOther() {
    CalledIncidentEvent = 'IncidentsOther()';
    clusterMappingInit(OtherIncidentsIndex);
    plotClusters();
}

var latSensitivity = 2;
var longSensitivity = 2;

function plotClusters() {

    if (IncidentEntitiesLayer == null) {
        IncidentEntitiesLayer = new Microsoft.Maps.EntityCollection();

        PortalMap.entities.push(IncidentEntitiesLayer);
    } else {
        IncidentEntitiesLayer.clear();
    }

    RenderIncident();
}

function RenderIncident() {
    for (var i = 0; i < Clusters.length; i++) {
        var cluster = Clusters[i];
        var iconUrl = "Content/Images/smallIncident.png";
        var labelCount = cluster.Places.length;
        var lat = cluster.Center.Lat;
        var long = cluster.Center.Long;
        var x = "" + labelCount + "";
        var type = '';
        var descriptionInfo = '';
        var imageLabel = '';
        var type1 = '';
        var timeStamp = '';

        imageLabel = '<div id="" style="overflow-y: scroll; height:400px;"><table><tr>';
        for (var j = 0; j < labelCount; j++) {
            var currentIncident = Incidents[cluster.Places[j].IncidentIndex];
            type = type + currentIncident.Type + ", ";
            type1 = currentIncident.Type;
            timeStamp = DateDeserialize(currentIncident.DateTime);
            //descriptionInfo = descriptionInfo + currentIncident.AdditionalInfo + ", ";

            var imageURL = currentIncident.MediaUri;
            if (imageURL != null)
                imageLabel = imageLabel + '<td><table><tr><td>' + type1 + '</td></tr><tr><td><table><tr><td><a target="_blank" href="' + imageURL + '"><img alt="No Pic" src="' + imageURL + '" width="90" height="90"/></a></td></tr></table></td></tr><tr><td nowrap>ID: ' + currentIncident.IncidentID + '<br>' + timeStamp + '</td></tr></table></td>';
            else
                imageLabel = imageLabel + '<td><table><tr><td>' + type1 + '</td></tr><tr><td><table><tr><td style="background-color:#C0C0C0;width:90px;height:90px;"><H3>No Pic</H3></td></tr></table></td></tr><tr><td nowrap>ID: ' + currentIncident.IncidentID + '<br>' + timeStamp + '</td></tr></table></td>';

            if ((j + 1) % 3 == 0) imageLabel += '</tr></table><br><table><tr>';
        }
        imageLabel = imageLabel + '</tr></table></div>';

        type = type.substr(0, type.length - 2);
        //descriptionInfo = descriptionInfo.substr(0, descriptionInfo.length - 2);
        if (labelCount > 1) type = 'INCIDENTS';

        var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(lat, long), { text: x, icon: iconUrl, width: 30, height: 30, typeName: 'incidentInfo' });
        IncidentEntitiesLayer.push(pin);

        var infobox = new Microsoft.Maps.Infobox(
        new Microsoft.Maps.Location(lat, long),
        {
            title: type,
            description: imageLabel,
            visible: true,
            pushpin: pin
        });
        IncidentEntitiesLayer.push(infobox);
    }
}

function clusterMappingInit(indexPointers) {
    Clusters = [];
    for (var i = 0; i < indexPointers.length ; i++) {
        if (checkAndAddToClusters(indexPointers[i]) == true) {

        } else {
            createAndAddNewCluster(indexPointers[i]);
        }
    }
}

function createAndAddNewCluster(incidentIndex) {
    var incident = Incidents[incidentIndex];
    Clusters.push(createClusterObject(incident.IncidentID, incident.AdditionalInfo, incident.Lat, incident.Long, incidentIndex));
}

function createClusterObject(id, info, lat, long, placeIndex, pushpPin) {
    return { "PushPin": pushpPin, "Id": id, "Info": info, "Center": { "Lat": lat, "Long": long }, "Places": [createIncidentPlaceObject(placeIndex, lat, long)] };
}

function createIncidentPlaceObject(index, lat, long) {
    return { "IncidentIndex": index, "Lat": lat, "Long": long }
}

function checkAndAddToClusters(incidentIndex) {
    var incident = Incidents[incidentIndex];
    for (var i = 0; i < Clusters.length; i++) {
        var cluster = Clusters[i];

        if (((cluster.Center.Lat + latSensitivity) > incident.Lat)
            && ((cluster.Center.Lat - latSensitivity) < incident.Lat)
            && ((cluster.Center.Long + longSensitivity) > incident.Long)
            && ((cluster.Center.Long - longSensitivity) < incident.Long)) {
            cluster.Places.push(createIncidentPlaceObject(incidentIndex, incident.Lat, incident.Long));
            return true;
        }
    }
    return false;
}

$(function () {

    $("#IncidentStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
    });
    $("#IncidentEndDate").datepicker({
        changeMonth: true,
        changeYear: true
    });
});