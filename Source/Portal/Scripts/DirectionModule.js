
var directionsManager;
 
function createDirectionsManager() {
    
    if (!directionsManager) {
        directionsManager = new Microsoft.Maps.Directions.DirectionsManager(PortalMap);
    }
   
}

function loadDirectionsModule() {
    if (!directionsManager) {
        Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: createDirectionsManager });
    }
    else {
        createDirectionsManager();
    }
}

function createDrivingRoute(startLocation,endLocation) {
    if (directionsManager) {

        directionsManager.resetDirections();
        // Set Route Mode to driving 
        directionsManager.setRequestOptions({ routeMode: Microsoft.Maps.Directions.RouteMode.driving });

        //var seattleWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: 'Seattle, WA' });
        //directionsManager.addWaypoint(seattleWaypoint);
        //var tacomaWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: 'Tacoma, WA', location: new Microsoft.Maps.Location(47.255134, -122.441650) });
        //directionsManager.addWaypoint(tacomaWaypoint);
        var source = new Microsoft.Maps.Directions.Waypoint({ location: startLocation });
        var target = new Microsoft.Maps.Directions.Waypoint({ location: endLocation });
        directionsManager.addWaypoint(source);
        //var target = new Microsoft.Maps.Directions.Waypoint({ location: new Microsoft.Maps.Location(47.255134, -122.441650) });
        directionsManager.addWaypoint(target);
        // Set the element in which the itinerary will be rendered
        directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('directionInfo') });
        //alert('Calculating directions...');
        directionsManager.calculateDirections();
    } else {
        loadDirectionsModule();
        createDrivingRoute(startLocation, endLocation);
    }
}

