// ---------------------------------------------------------------------------

namespace SOS.Phone.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Device.Location;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Maps.Controls;
    using Microsoft.Phone.Maps.Services;
    using Microsoft.Phone.Maps.Toolkit;
    using Microsoft.Phone.Tasks;
    using System.Windows.Navigation;


    /// <summary>
    /// Maps sample page
    /// </summary>
    public partial class ShowRouteMap : PhoneApplicationPage
    {
        /// <summary>
        /// Zoom level to be used when showing the user location
        /// </summary>
        private readonly double userLocationMarkerZoomLevel = 16;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MapsSample"/> class
        /// </summary>
        public ShowRouteMap()
        {
            this.InitializeComponent();
            this.MapExtensionsSetup(this.Map);      

        }

        /// <summary>
        /// Gets or sets the current Map Mode used in the page
        /// </summary>
        private MapMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the route displayed in the map
        /// </summary>
        private MapRoute MapRoute { get; set; }

        #region Event Handlers

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string strCodeTiers = string.Empty;

            GeoCoordinate routeDirections = new GeoCoordinate();
            string parameter;
            if (NavigationContext.QueryString.TryGetValue("lat", out parameter))
            {
                double lat;
                double.TryParse(parameter, out lat);
                routeDirections.Latitude = lat;
            }
            if (NavigationContext.QueryString.TryGetValue("lng", out parameter))
            {
                double lng;
                double.TryParse(parameter, out lng);
                routeDirections.Longitude = lng;
            }
            this.RouteDirectionsPushPin.GeoCoordinate = routeDirections;
            OnShowRoute();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SOS.xaml?parameter=fromLocalHelp", UriKind.Relative));
            e.Cancel = true;
        }


        /// <summary>
        /// Event handler for the Me button. It will show the user location marker and set the view on the map
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void OnMe(object sender, EventArgs e)
        {
            await this.ShowUserLocation();

            this.Map.SetView(this.UserLocationMarker.GeoCoordinate, this.userLocationMarkerZoomLevel);
        }

        /// <summary>
        /// Event handler called when the user tap and hold in the map
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void OnMapHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ReverseGeocodeQuery query;
            List<MapLocation> mapLocations;
            string pushpinContent;
            MapLocation mapLocation;

            query = new ReverseGeocodeQuery();
            query.GeoCoordinate = this.Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.Map));

            mapLocations = (List<MapLocation>)await query.GetMapLocationsAsync();
            mapLocation = mapLocations.FirstOrDefault();

            if (mapLocation != null)
            {
                this.RouteDirectionsPushPin.GeoCoordinate = mapLocation.GeoCoordinate;

                pushpinContent = mapLocation.Information.Name;
                pushpinContent = string.IsNullOrEmpty(pushpinContent) ? mapLocation.Information.Description : null;
                pushpinContent = string.IsNullOrEmpty(pushpinContent) ? string.Format("{0} {1}", mapLocation.Information.Address.Street, mapLocation.Information.Address.City) : null;

                this.RouteDirectionsPushPin.Content = pushpinContent.Trim();
                this.RouteDirectionsPushPin.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Event handler for the directions button
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnDirections(object sender, EventArgs e)
        {
            if (this.RouteDirectionsPushPin.GeoCoordinate == null || this.RouteDirectionsPushPin.Visibility == Visibility.Collapsed)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.MapTapHoldToGetDirections, "basicWrap", "Info!"));
            }
            else
            {
              
                GeoCoordinate spaceNeedleLocation = new GeoCoordinate(this.UserLocationMarker.GeoCoordinate.Latitude, this.UserLocationMarker.GeoCoordinate.Longitude);
                BingMapsDirectionsTask bmDirectionTask = new BingMapsDirectionsTask();
                LabeledMapLocation currentLML = new LabeledMapLocation("Current location", spaceNeedleLocation);
                bmDirectionTask.Start = currentLML;


                GeoCoordinate destinationLocation = new GeoCoordinate(this.RouteDirectionsPushPin.GeoCoordinate.Latitude, this.RouteDirectionsPushPin.GeoCoordinate.Longitude);
                LabeledMapLocation destinationLocationLML = new LabeledMapLocation("Destination location", destinationLocation);
                bmDirectionTask.End = destinationLocationLML;

                bmDirectionTask.Show();
            }
        }

       
        /// <summary>
        /// Method to show Route
        /// </summary>
        private async void OnShowRoute()
        {

            this.UserLocationMarker.GeoCoordinate = Globals.RecentLocation.Coordinate;
            this.UserLocationMarker.Visibility = System.Windows.Visibility.Visible;
            this.RouteDirectionsPushPin.Visibility = Visibility.Visible;

            this.Map.SetView(this.UserLocationMarker.GeoCoordinate, this.userLocationMarkerZoomLevel);

            if (this.RouteDirectionsPushPin.GeoCoordinate == null || this.RouteDirectionsPushPin.Visibility == Visibility.Collapsed)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.MapTapHoldToShowRouteFromLocationToDestination, "basicWrap", "Info!"));
            }
            else
            {
                RouteQuery query;
                List<GeoCoordinate> wayPoints;
                Route route;

                if (this.MapRoute != null)
                {
                    this.Map.RemoveRoute(this.MapRoute);
                }

                //await this.ShowUserLocation();

                query = new RouteQuery();
                wayPoints = new List<GeoCoordinate>();

                wayPoints.Add(this.UserLocationMarker.GeoCoordinate);
                wayPoints.Add(this.RouteDirectionsPushPin.GeoCoordinate);

                query.Waypoints = wayPoints;

                route = await query.GetRouteAsync();
                this.MapRoute = new MapRoute(route);

                this.Map.SetView(route.BoundingBox);
                this.Map.AddRoute(this.MapRoute);

                this.ChangeMode(MapMode.Route);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Show the user location in the map
        /// </summary>
        /// <returns>Task that can used to await</returns>
        private async Task ShowUserLocation()
        {
            this.UserLocationMarker = (UserLocationMarker)this.FindName("UserLocationMarker");

            this.UserLocationMarker.GeoCoordinate = await Utility.GetLocationQuick();
            this.UserLocationMarker.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Setup the map extensions objects.
        /// All named objects inside the map extensions will have its references properly set
        /// </summary>
        /// <param name="map">The map that uses the map extensions</param>
        private void MapExtensionsSetup(Map map)
        {
            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(map);
            var runtimeFields = this.GetType().GetRuntimeFields();

            foreach (DependencyObject i in children)
            {
                var info = i.GetType().GetProperty("Name");

                if (info != null)
                {
                    string name = (string)info.GetValue(i);

                    if (name != null)
                    {
                        foreach (FieldInfo j in runtimeFields)
                        {
                            if (j.Name == name)
                            {
                                j.SetValue(this, i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the effective map mode. Will switch visuals state it changed
        /// </summary>
        /// <param name="mode">New map mode</param>
        private void ChangeMode(MapMode mode)
        {
            if (this.Mode != mode)
            {
                this.Mode = mode;

                switch (this.Mode)
                {
                    case MapMode.Route:
                        break;

                    case MapMode.Directions:
                        if (this.MapRoute != null)
                        {
                            this.Map.RemoveRoute(this.MapRoute);
                        }

                        break;
                }
            }
        }
        #endregion
    }
}