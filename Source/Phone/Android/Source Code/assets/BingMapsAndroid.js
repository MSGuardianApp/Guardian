var MM = Microsoft.Maps;

var BingMapsAndroid = new function () {
    var _mapId,
		_map,
		_bingMapKey,
		_layerManager,
		_infobox,
		_trafficLayer = null,
		_dataLayer,
		_infoboxLayer;
    var directionsManager;

    var directionsErrorEventObj;
    var directionsUpdatedEventObj;
    var FirstLatitude;
    var FirstLongitude;
    var SelectedLat;
    var SelectedLong;
    var Address;
    var serachLatLongs = [];
    var FocusLatstLat;
    var FocusLatstLong;
    var loc;
    var pinInfobox;


    this.AddLayer = function (data, view) {
        //alert("this is BingMapsAndroid.AddLayer");
        if (data != null) {
            var l = _layerManager.GetLayer(data.LayerName);
            if (l == null) {
                _layerManager.AddLayer(data.LayerName);
                l = _layerManager.GetLayer(data.LayerName);
            }

            l.AddEntities(data.Entities);

            if (view != null) {
                _map.setView({ bounds: view });
            }
        }
    };

    this.ClearLayer = function (layerName) {
        //alert("this is BingMapsAndroid.ClearLayer");
        if (layerName != null && layerName != '') {
            var l = _layerManager.GetLayer(layerName);
            if (l != null) {
                l.ClearLayer();
            }
        }
        else {
            _layerManager.DeleteAllLayers();
            _dataLayer.clear();
        }
    };

    this.HideLayer = function (layerName) {
        //alert("this is BingMapsAndroid.HideLayer");
        if (layerName != null && layerName != '') {
            var l = _layerManager.GetLayer(layerName);
            if (l != null) {
                l.HideLayer();
            }
        }
    };

    this.ShowLayer = function (layerName) {
        // alert("this is BingMapsAndroid.ShowLayer");
        if (layerName != null && layerName != '') {
            var l = _layerManager.GetLayer(layerName);
            if (l != null) {
                l.ShowLayer();
            }
        }
    };



    this.LoadMap = function (mapId) {

        _mapId = mapId;

        _bingMapKey = Utilities.GetQuerystring('bingMapKey', '');
        while (window.Microsoft == undefined) { }

        FirstLatitude = parseFloat(Utilities.GetQuerystring('lat', '0'));
        FirstLongitude = parseFloat(Utilities.GetQuerystring('lon', '0'));

        //Add entityId as a property of all shape objects.
        Microsoft.Maps.Pushpin.prototype.entityId = -1;
        Microsoft.Maps.Polygon.prototype.entityId = -1;
        Microsoft.Maps.Polyline.prototype.entityId = -1;

        var options = {
            credentials: _bingMapKey,
            zoom: parseInt(Utilities.GetQuerystring('zoom', '1')),
            center: new MM.Location(parseFloat(Utilities.GetQuerystring('lat', '0')), parseFloat(Utilities.GetQuerystring('lon', '0'))),
            showDashboard: false,
            showScalebar: false,
            showCopyright: false,
            enableSearchLogo: false,
            enableClickableLogo: false,
            fixedMapPosition: true,
        };

        _map = new MM.Map(document.getElementById(_mapId), options);
        _map.entities.clear();
        _dataLayer = new MM.EntityCollection();
        _map.entities.push(_dataLayer);
        _infoboxLayer = new MM.EntityCollection();
        _map.entities.push(_infoboxLayer);



        var location = new Microsoft.Maps.Location(parseFloat(Utilities.GetQuerystring('lat', '0')), parseFloat(Utilities.GetQuerystring('lon', '0')));

        var pin = new Microsoft.Maps.Pushpin(location, { icon: 'trackpin_small.png', width: 30, height: 50, typeName: 'mypin' });
        _map.entities.push(pin);

        _layerManager = new LayerManager(_map, _dataLayer);




        //Add handler for the map click event.
        MM.Events.addHandler(_map, 'dblclick', function (e) {

            if (e.targetType = "map") {
                var p = new MM.Point(e.getX(), e.getY());
                var c = e.target.tryPixelToLocation(p);
                var z = _map.getZoom();
                _map.setView({ zoom: z + 1, center: c });
            }
        });


        //Add handler for the map click event. (added on 21/7/14)
        MM.Events.addHandler(_map, 'rightclick', function (e) {

            if (e.targetType == "map") {
                var point = new Microsoft.Maps.Point(e.getX(), e.getY());

                loc = e.target.tryPixelToLocation(point);
                serachLatLongs[0] = new Microsoft.Maps.Location(FirstLatitude, FirstLongitude);
                serachLatLongs[1] = new Microsoft.Maps.Location(loc.latitude, loc.longitude);

                try {
                    $.ajax({
                        type: "GET",
                        url: 'http://dev.virtualearth.net/REST/v1/Locations/' + loc.latitude + ',' + loc.longitude + '?o=json&key=' + 'AmXVM6msjaWe9ataCTYo5jRtKa6hMKYtaoR0Rquo93OnQxproiZt04PPuAxmLBoG',
                        async: false,
                        dataType: 'jsonp',
                        jsonp: 'jsonp',
                        success: function (data, textStatus) {
                            // alert("success call");
                            // Create the info box for the pushpin
                            if (pinInfobox != null) {
                                _map.entities.clear(pinInfobox);
                            }
                            pinInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(loc.latitude, loc.longitude), {
                                title: 'Location', description: (data.resourceSets[0].resources[0].address.formattedAddress) ? (data.resourceSets[0].resources[0].address.formattedAddress) : (data.resourceSets[0].resources[0].address.adminDistrict), visible: true, width: 200, height: 100
                            });

                            _map.entities.push(pinInfobox);
                        },

                        /*  complete: function(cs1)
                          {
                            if(cs1.readyState == 4 && cs1.status == 200 && cs1.statusText == "load")
                             {
                                alert("HI:"+JSON.stringify());
                             }
                          },*/

                        error: function (e) {
                            alert("caught error" + JSON.stringify(e));
                            console.log("this is error" + e.message);
                        }
                    });

                }
                catch (ex) {
                    alert("caught exception:" + ex);
                    console.log("caught exception" + ex);
                }

            }
        });


        if (window.BingMapsInterlop) {
            MM.Events.addHandler(_map, "viewchangeend", function (e) {
                var c = _map.getCenter();
                var z = _map.getZoom();
                var b = _map.getBounds();
                window.BingMapsInterlop.mapMovedEvent(c.latitude, c.longitude, z,
                    b.getNorth(), b.getEast(), b.getSouth(), b.getWest());
            });

            window.BingMapsInterlop.mapLoaded();
        }

        setMapDimensions();
    };



    this.ProcessData = function (latlongValues, isSOS) {
        try {

            _map.entities.clear();

            var jsonResponse1 = latlongValues.toString();
            var jsonResponse = JSON.parse(jsonResponse1);

            // alert("Hi"+jsonResponse);

            //to get locations and out pushpin at first and last location;
            var loc = [];
            var pin = [];

            for (var i = 0; i < jsonResponse.length; i++) {
                // alert(jsonResponse[i].Lat+","+jsonResponse[i].Long);
                loc[i] = new Microsoft.Maps.Location(jsonResponse[i].Lat, jsonResponse[i].Long);
                if ((i == 0)) {
                    var pin_1 = new Microsoft.Maps.Pushpin(loc[i], { icon: 'trackpin_small.png', width: 30, height: 50, typeName: 'mypin' });
                    _map.entities.push(pin_1);
                }
                if (i == jsonResponse.length - 1) {
                    var pin_1 = new Microsoft.Maps.Pushpin(loc[i], { icon: 'trackpinend_small.png', width: 30, height: 50, typeName: 'mypin' });
                    _map.entities.push(pin_1);


                    //get last location
                    FocusLatstLat = loc[i].latitude;
                    FocusLatstLong = loc[i].longitude;

                }
            }

            if (isSOS == 0)
                var line = new Microsoft.Maps.Polyline(loc, { strokeColor: new Microsoft.Maps.Color(255, 16, 170, 30) });
            else
                var line = new Microsoft.Maps.Polyline(loc, { strokeColor: new Microsoft.Maps.Color(255, 249, 101, 17) });

            // Add the shape to the map
            line.width = 5;
            _map.entities.push(line);
            //below line is to bring focus to pushpin location (current location) at regular calls.
            _map.setView({ center: loc[0], zoom: 15 });


            _layerManager = new LayerManager(_map, _dataLayer);

        }
        catch (err) {
            alert("exception is " + err);
        }
    };

    //call this function from Android, when Directions app bar button is clicked. 21/7/14
    this.CreateDesiredRoute = function () {
        if (serachLatLongs[1] != null) {
            _map.entities.clear();

            if (typeof directionsManager === "undefined") {
                Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: createDrivingRouteNew });
            }
            else {
                createDrivingRouteNew();

            }

        }
        else {
            alert("tap and hold anywhere in the map to show a location address. after that you will be shown a route from your location to the destination");
        }

    };

    //to focus current location
    this.FocusCurrentLocation = function () {

        _map.setView({ center: new Microsoft.Maps.Location(FocusLatstLat, FocusLatstLong), zoom: 15 });

    }


    //this function will be called when user clicks on any of the search result list item.
    this.CreateRoute = function (lat, long, address) {

        // ClearLayer(_dataLayer);
        _map.entities.clear();
        SelectedLat = lat;
        SelectedLong = long;
        Address = address;
        createDirections();

    };

    function createDirections() {

        if (typeof directionsManager === "undefined") {
            Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: createDrivingRoute });
        }
        else {
            createDrivingRoute();

        }
    };

    function createDrivingRoute() {

        if (typeof directionsManager === "undefined") { createDirectionsManager(); }
        directionsManager.resetDirections();
        // Set Route Mode to driving
        directionsManager.setRequestOptions({ routeMode: Microsoft.Maps.Directions.RouteMode.driving });

        var SourceWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: '', location: new Microsoft.Maps.Location(FirstLatitude, FirstLongitude), pushpin: new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(FirstLatitude, FirstLongitude), { icon: 'trackpinend_small.png', width: 30, height: 50, typeName: 'mypin' }) });
        directionsManager.addWaypoint(SourceWaypoint);
        var DestinationWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: '', location: new Microsoft.Maps.Location(SelectedLat, SelectedLong), pushpin: new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(SelectedLat, SelectedLong), { icon: 'destpin_small.png', width: 30, height: 50, typeName: 'mypin' }) });
        directionsManager.addWaypoint(DestinationWaypoint);

        //  var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(FirstLatitude, FirstLongitude), new Microsoft.Maps.Location(SelectedLat, SelectedLong));
        //  _map.setView({ bounds: viewBoundaries});
        _map.setView({ center: new Microsoft.Maps.Location(FirstLatitude, FirstLongitude), zoom: 20 });

        // Set the element in which the itinerary will be rendered
        // directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('directionsItinerary') });
        //alert('Calculating directions...');
        directionsManager.calculateDirections();

    };


    //added this function on 26/8/2014
    function createDrivingRouteNew() {

        if (typeof directionsManager === "undefined") { createDirectionsManager(); }
        directionsManager.resetDirections();
        // Set Route Mode to driving
        directionsManager.setRequestOptions({ routeMode: Microsoft.Maps.Directions.RouteMode.driving });

        var SourceWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: '', location: new Microsoft.Maps.Location(serachLatLongs[0].latitude, serachLatLongs[0].longitude), pushpin: new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(serachLatLongs[0].latitude, serachLatLongs[0].longitude), { icon: 'trackpinend_small.png', width: 30, height: 50, typeName: 'mypin' }) });
        directionsManager.addWaypoint(SourceWaypoint);
        var DestinationWaypoint = new Microsoft.Maps.Directions.Waypoint({ address: '', location: new Microsoft.Maps.Location(serachLatLongs[1].latitude, serachLatLongs[1].longitude), pushpin: new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(serachLatLongs[1].latitude, serachLatLongs[1].longitude), { icon: 'destpin_small.png', width: 30, height: 50, typeName: 'mypin' }) });
        directionsManager.addWaypoint(DestinationWaypoint);

        _map.setView({ center: new Microsoft.Maps.Location(serachLatLongs[0].latitude, serachLatLongs[0].longitude), zoom: 20 });

        // Set the element in which the itinerary will be rendered
        // directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('directionsItinerary') });
        //alert('Calculating directions...');
        directionsManager.calculateDirections();

    };



    function createDirectionsManager() {
        var displayMessage;


        if (typeof directionsManager === "undefined") {
            directionsManager = new Microsoft.Maps.Directions.DirectionsManager(_map);
            displayMessage = 'Directions Module loaded\n';
            displayMessage += 'Directions Manager loaded';


        }
        //alert(displayMessage);
        directionsManager.resetDirections();
        // directionsErrorEventObj = Microsoft.Maps.Events.addHandler(directionsManager, 'directionsError', function (arg) { alert(arg.message) });
        // directionsUpdatedEventObj = Microsoft.Maps.Events.addHandler(directionsManager, 'directionsUpdated', function () { alert('Directions updated') });
    };


    this.Pan = function (dx, dy) {
        _map.setView({ centerOffset: new MM.Point(dx, dy) });
    };

    this.SetCenterAndZoom = function (c, z) {
        _map.setView({ center: c, zoom: z });
    };

    this.SetHeading = function (h) {
        _map.setView({ heading: h });
    };

    this.SetMapView = function (view) {
        _map.setView({ bounds: view });
    };

    this.SetMapStyle = function (style) {
        _map.setMapType(style);
    };

    this.ShowTraffic = function (showTraffic) {
        if (showTraffic) {
            if (_trafficLayer == null) {
                Microsoft.Maps.loadModule('Microsoft.Maps.Traffic', {
                    callback: function () {
                        _trafficLayer = new MM.Traffic.TrafficLayer(_map);
                        _trafficLayer.show();
                    }
                });
            } else {
                _trafficLayer.show();
            }
        } else if (_trafficLayer != null) {
            _trafficLayer.hide();
        }
    };

    this.CloseInfobox = function () {
        _infoboxLayer.clear();
    };

    this.ShowInfobox = function (entity) {

        _infoboxLayer.clear();

        if (entity.title != null && entity.title != undefined && entity.title != '') {
            var center = null;

            if (entity.getLocation != null) {
                center = entity.getLocation();
            } else {//calculate center of polyline or polygon
                center = Utilities.GetCenter(entity.getLocations());
            }

            if (center != null) {
                var title = (entity.title.length > 23) ? Utilities.TruncateText(entity.title, 20) : entity.title;

                // Create an info box 
                var infoboxOptions = {
                    width: 200,
                    height: 30,
                    title: title,
                    showPointer: true,
                    titleClickHandler: function () {
                        if (window.BingMapsInterlop) {
                            window.BingMapsInterlop.entityClicked(entity.layerName, entity.entityId);
                        }
                    },
                    offset: new MM.Point(0, 20),
                    visible: true
                };

                _infobox = new MM.Infobox(center, infoboxOptions);

                // Add the info box to the map
                _infoboxLayer.push(_infobox);
            }
        }
    };

    this.ZoomIn = function (x, y) {
        //  alert("this is BingMapsAndroid.ZoomIn");
        // alert("this is zoom in call");
        var z = _map.getZoom();
        _map.setView({ zoom: z + 1 });
    };

    this.ZoomOut = function (x, y) {
        //alert("this is BingMapsAndroid.ZoomOut");
        // alert("this is zoom out call");
        var z = _map.getZoom();
        _map.setView({ zoom: z - 1 });
    };

    function setMapDimensions() {
        // alert("this is BingMapsAndroid.setMapDimensions");
        _map.setOptions({ width: window.innerWidth, height: window.innerHeight });
    }
};

var Layer = function (name, map, dataLayer) {
    this.Name = name;

    var entities = new MM.EntityCollection(),
	_map = map;

    this.AddEntities = function (data) {
        if (data != null) {
            var i = data.length - 1;
            if (i >= 0) {
                do {
                    data[i].Entity.entityId = data[i].EntityId;
                    data[i].Entity.layerName = name;

                    if (data[i].title != null && data[i].title != undefined && data[i].title != '') {
                        data[i].Entity.title = data[i].title;

                        // Add a handler for the pushpin click event.
                        MM.Events.addHandler(data[i].Entity, 'click', function (e) {
                            BingMapsAndroid.ShowInfobox(e.target);
                        });
                    }

                    entities.push(data[i].Entity);
                } while (i--)
            }
        }
    };

    this.ClearLayer = function () {
        entities.clear();
        BingMapsAndroid.CloseInfobox();
    };

    this.HideLayer = function () {
        entities.setOptions({ visible: false });
        BingMapsAndroid.CloseInfobox();
    };

    this.ShowLayer = function () {
        entities.setOptions({ visible: true });
    };

    function init() {
        alert("this is init");
        dataLayer.push(entities);
    }
    init();
};

var LayerManager = function (map, dataLayer) {
    var _layers = [],
	_map = map;

    this.AddLayer = function (name) {
        var layer = new Layer(name, _map, dataLayer);
        _layers.push(layer);
    };

    this.DeleteLayer = function (name) {
        var i = getLayerIndexByName(name);
        if (i != -1) {
            _dataLayer.removeAt(i);
            _layers.splice(i, 1);
        }

        BingMapsAndroid.CloseInfobox();
    };

    this.DeleteAllLayers = function () {
        dataLayer.clear();
        BingMapsAndroid.CloseInfobox();
        _layers = [];
    };

    this.GetLayer = function (name) {
        var i = getLayerIndexByName(name);
        return (i != -1) ? _layers[i] : null;
    };

    function getLayerIndexByName(name) {
        for (var i = 0; i < _layers.length; i++) {
            if (_layers[i].Name == name) {
                return i;
            }
        }

        return -1;
    }
};

var Utilities = new function () {
    this.GetQuerystring = function (key, _default) {
        if (_default == null) {
            _default = '';
        }

        var regex = new RegExp("[\\?&]" + key + "=([^&#]*)"),
			qs = regex.exec(window.location.href);

        if (qs == null || qs.length < 2) {
            return _default;
        }
        else {
            return qs[1];
        }
    };

    this.TruncateText = function (text, len) {
        if (text.length > len && len > 3) {
            text = text.substring(0, len - 2) + '...';
        }

        return text;
    };

    this.GetCenter = function (locations) {
        if (locations != null) {
            var i = locations.length - 1;
            if (i >= 0) {
                var maxLat = -90, maxLon = -180, minLat = 90, minLon = 180;
                do {
                    if (locations[i].latitude > maxLat) {
                        maxLat = locations[i].latitude;
                    }
                    if (locations[i].longitude > maxLon) {
                        maxLon = locations[i].longitude;
                    }
                    if (locations[i].latitude < minLat) {
                        minLat = locations[i].latitude;
                    }
                    if (locations[i].longitude < minLon) {
                        minLon = locations[i].longitude;
                    }
                } while (i--)

                return new MM.Location((maxLat + minLat) * 0.5, (maxLon + minLon) * 0.5);
            }
        }

        return null;
    };
};

Microsoft.Maps.moduleLoaded('mapModule');