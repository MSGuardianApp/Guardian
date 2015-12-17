function mapViewChangeEventHandler() {
    viewchange = Microsoft.Maps.Events.addHandler(PortalMap, 'viewchange', function (e) {
        onViewChange(e);
    });
}
function detachMapViewChangeEventHandler() {
    Microsoft.Maps.Events.removeHandler(viewchange);
    //alert('event detached');
}
function onViewChange(e) {
    
    var latlon = PortalMap.getCenter();
    var dynamicZoomLevel = PortalMap.getZoom();
    
    if (IncidentDisplayFlag == true)
    {
        if (dynamicZoomLevel > 12) {
            if (latSensitivity != 0 && longSensitivity != 0) {
                latSensitivity = 0;
                longSensitivity = 0;
                setTimeout(CalledIncidentEvent , 1);
            }
        } else {

           if (latSensitivity == 0 && longSensitivity == 0) {
               latSensitivity = 2;
               longSensitivity = 2;
               setTimeout(CalledIncidentEvent, 1);
           }
        }
    }

  //  $('#lstInformationLat').html('Lat : ' + (latlon.latitude).toString().substring(0, 10));
  //  $('#lstInformationLong').html('Long : ' + (latlon.longitude).toString().substring(0, 10));

    document.getElementById("lstInformationLat").value = (latlon.latitude).toString().substring(0, 10);
    document.getElementById("lstInformationLong").value = (latlon.longitude).toString().substring(0, 10);
}

function getAddress() {

    while (PortalMap.entities.getLength() != 0) {
        for (i = 0; i < PortalMap.entities.getLength() ; i++) {
            PortalMap.entities.remove(PortalMap.entities.get(i));
        }
    }

    var UserEntitiesLayer = null;
    UserEntitiesLayer = new Microsoft.Maps.EntityCollection();
    PortalMap.entities.push(UserEntitiesLayer);

    var currentLat = document.getElementById("lstInformationLat").value;
    var currentLong = document.getElementById("lstInformationLong").value;
    var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(currentLat, currentLong), { text: 'A', width: 24, height: 40 });
    UserEntitiesLayer.push(pin);
    PortalMap.setView({ zoom: '12', center: new Microsoft.Maps.Location(currentLat, currentLong) });
    getLocation(currentLat, currentLong);

}

function getLocation(Lat,Long) {

    var address;
    var url = "https://dev.virtualearth.net/REST/v1/Locations/" + Lat + "," + Long + "?key=" + GuardianConfig.MapCredentials + "&o=json&jsonp=?";
    try
    {
            $.ajax({
            url: url,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: function (data) {
                var result = data.resourceSets[0];
                if (result.estimatedTotal > 0)
                    address = result.resources[0].name;
                else
                    address = 'Address Not Found';
                $('#addressdisplay').html(address);
            }
        });
    }

    catch (e) {
        $('#addressdisplay').html('Address Not Found');
    }
}