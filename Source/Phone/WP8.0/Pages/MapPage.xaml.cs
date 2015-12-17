using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Navigation;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Text;
using System.Windows.Threading;
using System.Windows.Media;
using SOS.Phone.LocationServiceRef;

namespace SOS.Phone
{
    public partial class LocateMap : PhoneApplicationPage
    {
        ProgressIndicator pi;
        MapLayer PushpinMapLayer = null;
        MapPolyline _line = null;
        MapOverlay prevMapOverlay = null;
        string LastTrackingTime = (new Random()).Next(9999).ToString();
        DispatcherTimer timer = null;
        /// <summary>
        /// Zoom level to be used when showing the user location
        /// </summary>
        private readonly double userLocationMarkerZoomLevel = 16;

        /// <summary>
        /// Gets or sets the current Map Mode used in the page
        /// </summary>
        private MapMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the route displayed in the map
        /// </summary>
        private MapRoute MapRoute { get; set; }

        public LocateMap()
        {
            try
            {
                InitializeComponent();
                pi = new ProgressIndicator();
                pi.IsIndeterminate = true;
                pi.IsVisible = false;
                this.MapExtensionsSetup(this.MyMap);
                this.MyMap.CartographicMode = (MapCartographicMode)Globals.CurrentProfile.MapView;
                var ViewMenu = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
                if (this.MyMap.CartographicMode == MapCartographicMode.Road)
                {
                    ViewMenu.Text = "Aerial View";
                }
                else if (this.MyMap.CartographicMode == MapCartographicMode.Aerial)
                {
                    ViewMenu.Text = "Road View";
                }
                this.MyMap.ResolveCompleted += MapResolveCompleted;
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        /// <summary>
        /// While navigating away, set the idle detection mode to enabled.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if ((PhoneApplicationService.Current.UserIdleDetectionMode == IdleDetectionMode.Disabled) && (!Globals.CurrentProfile.IsSOSOn) && (!Globals.CurrentProfile.IsTrackingOn))
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
            if (timer != null)
                timer.Stop();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);

                // setting Idel detection mode to true while entering the page.
                if (PhoneApplicationService.Current.UserIdleDetectionMode == IdleDetectionMode.Enabled)
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

                string profileId = string.Empty, buddyName;
                if (NavigationContext.QueryString.TryGetValue("ProfileId", out profileId) && Globals.IsRegisteredUser)
                {
                    MyMap.ZoomLevel = 16;
                    NavigationContext.QueryString.TryGetValue("Name", out buddyName);
                    BuddyName.Text = buddyName;
                    LoadBuddyLocationFirstTime(profileId);
                    RunInBackground(profileId, (int)Constants.RefreshInterval);
                }

            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private void LoadBuddyLocationFirstTime(string profileId)
        {
            try
            {
                _line = new MapPolyline();
                _line.StrokeThickness = 5;
                MyMap.MapElements.Add(_line);

                PushpinMapLayer = new MapLayer();
                MyMap.Layers.Add(PushpinMapLayer);

                LoadBuddyLocationAsync(profileId);
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        public void RunInBackground(string profileId, int runEveryNMilSecs)
        {
            timer = new DispatcherTimer();
            timer.Tick +=
                delegate(object s, EventArgs args)
                {
                    LoadBuddyLocationAsync(profileId);
                };

            timer.Start();
            timer.Interval = new TimeSpan(0, 0, 0, 0, runEveryNMilSecs);
        }

        private async void LoadBuddyLocationAsync(string profileId)
        {
            try
            {
                PageProgressBar.Visibility = Visibility.Visible;
                string locationURL = string.Format(Constants.GetUserLocationUrl, profileId, LastTrackingTime);
                WebClient client = new WebClient();
                client.Headers["AuthID"] = Globals.User.LiveAuthId;
                client.DownloadStringCompleted += BuddyLocation_DownloadCompleted;
                client.DownloadStringAsync(new Uri(locationURL));
            }
            catch (Exception exception)
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private void BuddyLocation_DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // Only proceed if there wasn't an error
                if (e.Error == null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoTags));
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Result));
                    var geoTagsArrays = (GeoTags)ser.ReadObject(ms);

                    if (geoTagsArrays == null || geoTagsArrays.Lat == null || geoTagsArrays.Lat.Count <= 0)
                    {
                        StatusTextBlock.Text = "Tracking is off";
                        return;
                    }

                    StatusTextBlock.Text = "Tracking...";

                    var phoneGeoTag = TrackingSession.ConvertServerGeoTags(geoTagsArrays);
                    var geoTag = from a in phoneGeoTag.GeoTags
                                 orderby a.TimeStamp ascending
                                 select a;

                    GeoCoordinate positionCoord = null;
                    GeoCoordinate buddyCurrentCoordinates = new GeoCoordinate() { Latitude = geoTag.Last().Lat, Longitude = geoTag.Last().Long };
                    this.RouteDirectionsPushPin.GeoCoordinate = buddyCurrentCoordinates;
                    PaceBox.Text = geoTag.Last().Speed.ToString();
                    RecentTimeTextBlock.Text = (new DateTime(geoTag.Last().TimeStamp)).ToString("dd/MM/yyyy HH:mm:ss");
                    AccuracyTextBlock.Text = (geoTag.Last().Accuracy != null && geoTag.Last().Accuracy != 0) ? Math.Round(geoTag.Last().Accuracy).ToString(CultureInfo.InvariantCulture) + " Meters" : "N/A";
                    foreach (var geoTagItem in geoTag)
                    {
                        positionCoord = new GeoCoordinate()
                        {
                            Altitude = geoTagItem.Alt,
                            Latitude = Convert.ToDouble(geoTagItem.Lat),
                            Longitude = Convert.ToDouble(geoTagItem.Long)
                        };

                        //TODO: Draw arrow mark in the actual direction of movement
                        Color strokeColor = (geoTagItem.IsSOS) ? Colors.Orange : Colors.Green;
                        _line.StrokeColor = strokeColor;
                        _line.Path.Add(positionCoord);
                        LastTrackingTime = geoTagItem.TimeStamp.ToString();

                        //to retain first pin
                        if (PushpinMapLayer.Count < 1)
                            Utility.DrawPushpin(PushpinMapLayer, positionCoord, false);

                    }

                    if (positionCoord != null && geoTag.Count() > 0)
                    {
                        if (prevMapOverlay != null)
                            PushpinMapLayer.Remove(prevMapOverlay);

                        prevMapOverlay = Utility.DrawPushpin(PushpinMapLayer, positionCoord, true);

                        // Show progress indicator while map resolves to new position
                        pi.IsVisible = true;
                        pi.IsIndeterminate = true;
                        pi.Text = "Resolving...";
                        SystemTray.SetProgressIndicator(this, pi);

                        this.MyMap.Center = positionCoord;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
            finally
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void MapResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
            try
            {
                // Hide progress indicator
                pi.IsVisible = false;
                pi.IsIndeterminate = false;
                SystemTray.SetProgressIndicator(this, null);
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        /// <summary>
        /// Event handler for the directions button
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnShowRouteClick(object sender, EventArgs e)
        {
            try
            {
                OnShowRoute();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }


        /// <summary>
        /// Event handler for the directions button
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnDirectionsClick(object sender, EventArgs e)
        {
            try
            {
                this.UserLocationMarker.GeoCoordinate = Globals.RecentLocation.Coordinate;
                if (this.RouteDirectionsPushPin == null || this.RouteDirectionsPushPin.GeoCoordinate == null)
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
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        /// <summary>
        /// Method to show Route
        /// </summary>
        private async void OnShowRoute()
        {

            this.UserLocationMarker.GeoCoordinate = Globals.RecentLocation.Coordinate;
            this.UserLocationMarker.Visibility = System.Windows.Visibility.Visible;

            this.MyMap.SetView(this.UserLocationMarker.GeoCoordinate, this.userLocationMarkerZoomLevel);

            if (this.RouteDirectionsPushPin == null || this.RouteDirectionsPushPin.GeoCoordinate == null)
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
                    this.MyMap.RemoveRoute(this.MapRoute);
                }

                query = new RouteQuery();
                wayPoints = new List<GeoCoordinate>();

                wayPoints.Add(this.UserLocationMarker.GeoCoordinate);
                wayPoints.Add(this.RouteDirectionsPushPin.GeoCoordinate);

                query.Waypoints = wayPoints;

                route = await query.GetRouteAsync();
                this.MapRoute = new MapRoute(route);

                this.MyMap.SetView(route.BoundingBox);
                this.MyMap.AddRoute(this.MapRoute);

                this.ChangeMode(MapMode.Route);
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
                            this.MyMap.RemoveRoute(this.MapRoute);
                        }

                        break;
                }
            }
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


        private void ViewAppBarItem_OnClick(object sender, EventArgs e)
        {
            var ViewMenu = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            if (ViewMenu.Text == "Road View")
            {
                ViewMenu.Text = "Aerial View";
                MyMap.CartographicMode = MapCartographicMode.Road;
                Globals.CurrentProfile.MapView = MapCartographicMode.Road;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.MapView, MapCartographicMode.Road.ToString());
            }
            else
            {
                ViewMenu.Text = "Road View";
                MyMap.CartographicMode = MapCartographicMode.Aerial;
                Globals.CurrentProfile.MapView = MapCartographicMode.Aerial;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.MapView, MapCartographicMode.Aerial.ToString());
            }
        }
    }
}