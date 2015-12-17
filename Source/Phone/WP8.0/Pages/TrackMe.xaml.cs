using System.Globalization;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using SOS.Phone.MVVM.Model;
using SOS.Phone.ServiceWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;

namespace SOS.Phone.Pages
{
    public partial class TrackMe : PhoneApplicationPage
    {
        MapLayer _pushpinMapLayer;
        MapPolyline _line;
        MapOverlay _prevMapOverlay;
        MapOverlay _searchBarMapOverlay;

        private double _previousHeading;
        int _drawPosCount;

        bool _hideSearchDestinationPopup = false, _positionCenter = true;
        private ObservableCollection<MapAddressModel> _locationModel;

        /// <summary>
        /// Gets or sets the current Map Mode used in the page
        /// </summary>
        private MapMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the route displayed in the map
        /// </summary>
        private MapRoute MapRoute { get; set; }

        /// <summary>
        /// Zoom level to be used when showing the user location
        /// </summary>
        private const double MapZoomLevel = 16;

        private bool _isTrackResultsLoaded;

        public TrackMe()
        {
            try
            {
                InitializeComponent();

                //Creating a MapLayer and adding the MapOverlay to it
                _pushpinMapLayer = new MapLayer();
                MyMap.Layers.Add(_pushpinMapLayer);

                _line = new MapPolyline { StrokeThickness = 5 };
                MyMap.MapElements.Add(_line);

                MapExtensionsSetup(MyMap);
                if (Globals.TagList == null || Globals.TagList.Count <= 0)
                    GetMyLocation_OnClick(null, EventArgs.Empty);

                TrackMePageOnLoad();

            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }


        private void TrackMePageOnLoad()
        {
            StateUtility.InTrackMePage = true;
            PageProgressBar.Visibility = Visibility.Visible;

            if (!Globals.CurrentProfile.IsTrackingOn)
                StartTracking(true);//BL: Local Page Tracking will be enabled in OnNavigationTo
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StateUtility.InTrackMePage = true;
            PageProgressBar.Visibility = Visibility.Visible;

            base.OnNavigatedTo(e);

            if (Globals.CurrentProfile.IsTrackingOn)
            {
                //If Tracking is already ON, get the tracked data from static object and show in Map
                if (Globals.TagList != null && Globals.TagList.Count > 0)
                    ShowCurrentSessionFullTracking();

                if (App.Geolocator != null)
                    EnableLocalTracking();
                else
                    StartTracking();
            }
            RenderUIBasedOnStatus();

            PageProgressBar.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (App.Geolocator != null)
            {
                if (Config.UseGeoLocator)
                {
                    App.Geolocator.PositionChanged -= Geolocator_PositionChanged;
                    App.Geolocator.StatusChanged -= Geolocator_StatusChanged;
                }
                else
                {
                    App.geoCoordinateWatcher.PositionChanged -= GeoCoordinateWatcher_PositionChanged;
                    App.geoCoordinateWatcher.StatusChanged -= GeoCoordinateWatcher_StatusChanged;
                }
            }
            base.OnNavigatedFrom(e);
        }

        void TrackMeApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StateUtility.InTrackMePage = false;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ToggleSosAppBarItem_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Globals.CurrentProfile.IsSOSOn)
                {
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SOSStatus, "false");

                    var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (appBarButton != null)
                    {
                        appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                        appBarButton.Text = "start sos";
                    }

                    Globals.InitiateStopSOSEventsAsync(false);
                }
                else
                {
                    Globals.RetainSOSState = false;
                    NavigationService.Navigate(new Uri("/Pages/SOS.xaml", UriKind.Relative));
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private void ToggleTrackAppBarItem_OnClick(object sender, EventArgs e)
        {
            if (!Globals.CurrentProfile.IsTrackingOn)
                StartTracking();
            else
                StopTracking();

            RenderUIBasedOnStatus();
        }

        void EnableLocalTracking()
        {
            _pushpinMapLayer.Clear();
            MyMap.MapElements.Clear();
            _line = new MapPolyline { StrokeThickness = 5 };
            MyMap.MapElements.Add(_line);

            if (Config.UseGeoLocator)
            {
                App.Geolocator.PositionChanged += Geolocator_PositionChanged;
                App.Geolocator.StatusChanged += Geolocator_StatusChanged;
            }
            else
            {
                App.geoCoordinateWatcher.PositionChanged += GeoCoordinateWatcher_PositionChanged;
                App.geoCoordinateWatcher.StatusChanged += GeoCoordinateWatcher_StatusChanged;
            }
        }

        void StartTracking(bool isPageLoad = false)
        {
            Globals.InitiateTracking(true);
            if (!isPageLoad)
                EnableLocalTracking();
            Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TurnOffTrackingText, "basicWrap", "Guardian tracking is active!"));
        }

        void StopTracking()
        {
            try
            {
                if (Globals.CurrentProfile.IsSOSOn) return;

                if (Config.UseGeoLocator)
                {
                    if (App.Geolocator != null)
                    {
                        App.Geolocator.PositionChanged -= Geolocator_PositionChanged;
                        App.Geolocator.StatusChanged -= Geolocator_StatusChanged;
                    }
                }
                else
                {
                    App.geoCoordinateWatcher.PositionChanged -= GeoCoordinateWatcher_PositionChanged;
                    App.geoCoordinateWatcher.StatusChanged -= GeoCoordinateWatcher_StatusChanged;
                }

                Globals.StopTracking();

                //Clear the Location details from Local Storage
                ClearTrackData();

                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                if (appBarButton != null)
                {
                    appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                    appBarButton.Text = "start tracking";
                }
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TrackingIsOffText, "basicWrap", "Info"));
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private void ClearTrackData()
        {
            Globals.TagList.Clear();
            Globals.PostedLocationIndex = 0;
            _drawPosCount = 0;

            PaceBox.Text = "0";
        }

        public void ShowCurrentSessionFullTracking()
        {
            try
            {
                _pushpinMapLayer.Clear();
                MyMap.MapElements.Clear();
                _prevMapOverlay = null;

                _line = new MapPolyline { StrokeThickness = 5 };
                MyMap.MapElements.Add(_line);

                _drawPosCount = Globals.TagList.Count;
                if (Globals.TagList.Count > 0)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        GeoCoordinate positionCoord = null;
                        var tagList = from GeoLocation loc in Globals.TagList
                                      orderby loc.TimeStamp ascending
                                      select loc;
                        foreach (var tag in tagList)
                        {
                            positionCoord = new GeoCoordinate(Convert.ToDouble(tag.Lat), Convert.ToDouble(tag.Long));
                            Color prevColor = _line.StrokeColor;
                            Color currentColor = (tag.IsSOS) ? Colors.Orange : Colors.Green;
                            if (prevColor != currentColor && _line != null && _line.Path != null && _line.Path.Count > 0)
                            {
                                GeoCoordinate newStartGeoCoordinate = _line.Path[_line.Path.Count - 1];
                                _line = new MapPolyline { StrokeThickness = 5 };
                                _line.Path.Add(newStartGeoCoordinate);
                                MyMap.MapElements.Add(_line);
                                _line.Path.Add(positionCoord);
                                _line.StrokeColor = currentColor;
                            }
                            else
                            {
                                if (_line.Path != null) _line.Path.Add(positionCoord);
                            }
                        }

                        //Draw start pushpin
                        Utility.DrawPushpin(_pushpinMapLayer, new GeoCoordinate(Convert.ToDouble(tagList.First().Lat), Convert.ToDouble(tagList.First().Long)), false);
                        if (Globals.TagList.Count > tagList.Count())// Draw line btw last point to next new point in the tag list, if any
                        {
                            var latestPoint = new GeoCoordinate(Convert.ToDouble(Globals.TagList[tagList.Count()].Lat), Convert.ToDouble(Globals.TagList[tagList.Count()].Long));
                            _line.StrokeColor = tagList.Last().IsSOS ? Colors.Orange : Colors.Green;
                            if (_line.Path != null) _line.Path.Add(latestPoint);
                            MyMap.Center = latestPoint;
                        }
                        else
                        {
                            _prevMapOverlay = Utility.DrawPushpin(_pushpinMapLayer, positionCoord, true);
                            MyMap.Center = positionCoord;
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        void Geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            if (StateUtility.IsRunningInBackground)
            {
                return;
            }

            string status = "";

            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "Location Service is disabled";
                    break;
                case PositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "Initializing...";
                    break;
                case PositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "No GPS data found";
                    break;
                case PositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "ON";
                    break;
                case PositionStatus.NotAvailable:
                    status = "GPS not available";
                    // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                    break;
                case PositionStatus.NotInitialized:
                    // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state
                    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                StatusTextBlock.Text = status;
            });
        }

        void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            try
            {
                if (StateUtility.IsRunningInBackground || !StateUtility.InTrackMePage) return;
#if DEBUG
                Dispatcher.BeginInvoke(() =>
                {
                    DebugTimeTextBlock.Text = "Poll - " + DateTime.Now.ToLongTimeString() + " Acy: " + Math.Round(args.Position.Coordinate.Accuracy);
                });
#endif
                Thread.Sleep(500);//500ms - Just to make sure, globals PositionChangeEvent executes first so that it captures valid RecentLocation

                Dispatcher.BeginInvoke(() =>
                {
                    if (args.Position.Coordinate.Accuracy < Globals.AcceptanceAccuracy) PaceBox.Text = "0"; else PaceBox.Text = "--";
                });

                if (Globals.TagList.Count <= _drawPosCount) return;
                _drawPosCount = Globals.TagList.Count;

                GeoCoordinate pos = args.Position.Coordinate.ToGeoCoordinate();

                #region Dynamically adjusting the ReportInterval based on Speed //Commented
                //App.geolocator.PositionChanged -= Geolocator_PositionChanged;
                ////App.geolocator.StatusChanged -= Geolocator_StatusChanged;
                //App.geolocator.ReportInterval = Algorithms.ReportIntervalCalculator.NextReportInterval(args.Position.Coordinate.ToGeoCoordinate().Speed);
                ////App.geolocator.StatusChanged += Geolocator_StatusChanged;
                //App.geolocator.PositionChanged += Geolocator_PositionChanged;
                #endregion

                //If IsTracking is switched off from MainPage
                if (Globals.CurrentProfile.IsTrackingOn)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
#if DEBUG
                        DebugRecordTextBlock.Text = "Recorded - " + DateTime.Now.ToLongTimeString();
#endif
                        PaceBox.Text = Globals.TagList[_drawPosCount - 1].Speed.ToString(CultureInfo.InvariantCulture);

                        if (_positionCenter) MyMap.Center = pos;
                        Color prevColor = _line.StrokeColor;
                        Color currentColor = Globals.CurrentProfile.IsSOSOn ? Colors.Orange : Colors.Green;

                        if (_line.Path.Count > 0)
                        {
                            try
                            {
                                var previousPoint = _line.Path.Last();
                                if (previousPoint.GetDistanceTo(pos) > 5)
                                {
                                    PositionHandler handler = new PositionHandler();
                                    var heading = handler.CalculateBearing(new Position(previousPoint), new Position(pos));
                                    //Introduce 10 degree change acceptence to reset the view.
                                    if (_positionCenter && (_previousHeading + 10) < heading || (_previousHeading - 10) > heading)
                                        MyMap.SetView(pos, MyMap.ZoomLevel, heading, MapAnimationKind.Parabolic);
                                    _previousHeading = heading;
                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            MyMap.Center = pos;
                        }

                        if (prevColor != currentColor && _line != null && _line.Path != null && _line.Path.Count > 0)
                        {
                            GeoCoordinate newStartGeoCoordinate = _line.Path[_line.Path.Count - 1];
                            _line = new MapPolyline { StrokeThickness = 5 };
                            _line.Path.Add(newStartGeoCoordinate);
                            MyMap.MapElements.Add(_line);
                            _line.Path.Add(pos);
                            _line.StrokeColor = currentColor;
                        }
                        else
                        {
                            _line.Path.Add(pos);
                        }

                        if (_pushpinMapLayer.Count > 1 && _prevMapOverlay != null)
                            _pushpinMapLayer.Remove(_prevMapOverlay);
                        else
                            Utility.DrawPushpin(_pushpinMapLayer, pos, false);

                        _prevMapOverlay = Utility.DrawPushpin(_pushpinMapLayer, pos, true);
                    }
                    );
                }
            }
            catch (Exception exception)
            {
                //Supress any ex
            }

        }

        #region GeoCoordinate Watcher Code

        async void GeoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            try
            {
#if DEBUG
                if (!StateUtility.IsRunningInBackground && StateUtility.InTrackMePage)
                    Dispatcher.BeginInvoke(() =>
                    {
                        DebugTimeTextBlock.Text = DateTime.Now.ToLongTimeString();
                    });
#endif
                //If IsTracking is switched off from MainPage
                if (Globals.CurrentProfile.IsTrackingOn && !StateUtility.IsRunningInBackground && StateUtility.InTrackMePage)
                {
                    //this.Dispatcher.CheckAccess();

                    //BUG 003: Commenting Dispatcher is throwing Unauthorized Access exception or not working
                    Dispatcher.BeginInvoke(() =>
                    {
#if DEBUG
                        DebugRecordTextBlock.Text = "Recorded - " + DateTime.Now.ToLongTimeString();
#endif
                        GeoCoordinate pos = new GeoCoordinate(args.Position.Location.Latitude, args.Position.Location.Longitude);
                        if (_positionCenter) MyMap.Center = pos;
                        Color prevColor = _line.StrokeColor;
                        Color currentColor = Globals.CurrentProfile.IsSOSOn ? Colors.Orange : Colors.Green;

                        if (_line.Path.Count > 0)
                        {
                            try
                            {
                                var previousPoint = _line.Path.Last();
                                PaceBox.Text = Globals.TagList[_drawPosCount - 1].Speed.ToString(CultureInfo.InvariantCulture);

                                if (previousPoint.GetDistanceTo(pos) > 5)
                                {
                                    PositionHandler handler = new PositionHandler();
                                    var heading = handler.CalculateBearing(new Position(previousPoint), new Position(pos));
                                    //Introduce 10 degree change acceptence to reset the view.
                                    if ((_previousHeading + 10) < heading || (_previousHeading - 10) > heading)
                                        MyMap.SetView(pos, MyMap.ZoomLevel, heading, MapAnimationKind.Parabolic);
                                    _previousHeading = heading;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            MyMap.Center = pos;
                        }

                        if (prevColor != currentColor && _line != null && _line.Path != null && _line.Path.Count > 0)
                        {
                            GeoCoordinate newStartGeoCoordinate = _line.Path[_line.Path.Count - 1];
                            _line = new MapPolyline { StrokeThickness = 5 };
                            _line.Path.Add(newStartGeoCoordinate);
                            MyMap.MapElements.Add(_line);
                            _line.Path.Add(pos);
                            _line.StrokeColor = currentColor;
                        }
                        else
                        {
                            _line.Path.Add(pos);
                        }

                        if (_pushpinMapLayer.Count > 1)
                        {
                            if (_prevMapOverlay != null)
                                _pushpinMapLayer.Remove(_prevMapOverlay);
                        }
                        else
                            Utility.DrawPushpin(_pushpinMapLayer, pos, false);

                        _prevMapOverlay = Utility.DrawPushpin(_pushpinMapLayer, pos, true);
                    }
                    );
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        void GeoCoordinateWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
        {
            if (StateUtility.IsRunningInBackground)
            {
                return;
            }

            string status = "";

            switch (args.Status)
            {
                case GeoPositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "Location Service is disabled";
                    break;
                case GeoPositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "Initializing...";
                    break;
                case GeoPositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "No GPS data found";
                    break;
                case GeoPositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "ON";
                    break;
                //case PositionStatus.NotAvailable:
                //    status = "GPS not available";
                //    // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                //    break;
                //case PositionStatus.NotInitialized:
                //    // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state
                //    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                StatusTextBlock.Text = status;
            });
        }

        #endregion

        private void RenderUIBasedOnStatus()
        {
            try
            {
                if (Globals.CurrentProfile.IsSOSOn)
                {
                    var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    appBarButton.IconUri = new Uri("/Assets/Images/stopsos.png", UriKind.Relative);
                    appBarButton.Text = "stop sos";
                }
                else
                {
                    var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                    appBarButton.Text = "start sos";
                }

                if (Globals.CurrentProfile.IsTrackingOn)
                {
                    var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                    appBarButton.IconUri = new Uri("/Assets/Images/stoptrack.png", UriKind.Relative);
                    appBarButton.Text = "stop tracking";
                    StatusTextBlock.Text = "ON";
                }
                else
                {
                    var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                    appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                    appBarButton.Text = "start tracking";
                    StatusTextBlock.Text = "OFF";
                }
                ChangeFocusMeIcons();

                this.MyMap.CartographicMode = (MapCartographicMode)Globals.CurrentProfile.MapView;
                var ViewMenu = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;
                if (this.MyMap.CartographicMode == MapCartographicMode.Road)
                {
                    ViewMenu.Text = "Aerial View";
                }
                else if (this.MyMap.CartographicMode == MapCartographicMode.Aerial)
                {
                    ViewMenu.Text = "Road View";
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }


        #region Events

        /// <summary>
        /// Event handler for the Me button. It will show the user location marker and set the view on the map
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void GetMyLocation_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    _positionCenter = !_positionCenter;
                }
                if (_positionCenter)
                {
                    PageProgressBar.Visibility = Visibility.Visible;
                    await this.ShowUserLocation();
                    PageProgressBar.Visibility = Visibility.Collapsed;
                }
                ChangeFocusMeIcons();

            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        void ChangeFocusMeIcons()
        {
            if (_positionCenter)
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "Stop Focusing";
            else
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "Focus Me";
        }

        /// <summary>
        /// Event handler called when the user tap and hold in the map
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void OnMapHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                _positionCenter = false;
                string pushpinContent;

                ReverseGeocodeQuery query = new ReverseGeocodeQuery();
                query.GeoCoordinate = MyMap.ConvertViewportPointToGeoCoordinate(e.GetPosition(MyMap));

                List<MapLocation> mapLocations = (List<MapLocation>)await query.GetMapLocationsAsync();
                MapLocation mapLocation = mapLocations.FirstOrDefault();

                if (mapLocation != null)
                {
                    RouteDirectionsPushPin.GeoCoordinate = mapLocation.GeoCoordinate;
                    pushpinContent = mapLocation.Information.Name;
                    pushpinContent = string.IsNullOrEmpty(pushpinContent) ? mapLocation.Information.Description : null;
                    pushpinContent = string.IsNullOrEmpty(pushpinContent) ? string.Format("{0} {1}", mapLocation.Information.Address.Street, mapLocation.Information.Address.City) : null;

                    this.RouteDirectionsPushPin.Content = pushpinContent.Trim();
                    this.RouteDirectionsPushPin.Visibility = Visibility.Visible;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        /// <summary>
        /// Event handler for the show route button
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void OnShowRoute(object sender, EventArgs e)
        {
            try
            {
                if (this.RouteDirectionsPushPin.GeoCoordinate == null || this.RouteDirectionsPushPin.Visibility == Visibility.Collapsed)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.MapTapAndHoldText, "basicWrap", "Info!"));
                }
                else
                {
                    RouteQuery query;
                    List<GeoCoordinate> wayPoints;
                    Route route;

                    if (this.MapRoute != null)
                    {
                        _pushpinMapLayer.Remove(_searchBarMapOverlay);
                        this.MyMap.RemoveRoute(this.MapRoute);
                    }

                    var gc = await this.ShowUserLocation();
                    if (gc != null)
                    {
                        query = new RouteQuery();
                        wayPoints = new List<GeoCoordinate>();

                        wayPoints.Add(gc);
                        wayPoints.Add(this.RouteDirectionsPushPin.GeoCoordinate);

                        query.Waypoints = wayPoints;

                        route = await query.GetRouteAsync();
                        this.MapRoute = new MapRoute(route);

                        this.MyMap.SetView(route.BoundingBox);
                        this.MyMap.AddRoute(this.MapRoute);

                        this.ChangeMode(MapMode.Route);
                        Globals.RouteDirectionsPushPinGeoCoordinate = this.RouteDirectionsPushPin.GeoCoordinate;
                        Globals.TrackMeDestinationName = (string)this.RouteDirectionsPushPin.Content;
                    }
                }
            }
            catch (Exception exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.RanOutOfDestination, "basicWrap", "Oops!"));
            }
            finally
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Event handler for the show route on search destination button
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private async void DestinationSearchClick(object sender, EventArgs e)
        {
            try
            {
                GeocodeQuery geocodequery = new GeocodeQuery();
                destinationSearchResult.DataContext = null;
                if (SearchTextBox.Text == "")
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.EnterDestinationForSearchText, "basicWrap", "Oops!"));
                }
                else
                {
                    PageProgressBar.Visibility = Visibility.Visible;

                    geocodequery.SearchTerm = SearchTextBox.Text;
                    geocodequery.GeoCoordinate = await Utility.GetLocationQuick();
                    geocodequery.QueryCompleted += geocodequery_QueryCompleted;
                    geocodequery.QueryAsync();
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringAsync(new Uri(Utility.BingSearchUrl(SearchTextBox.Text, "", Constants.MaxTrackMeSearchResults, true)));
                    webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
                    _locationModel = new ObservableCollection<MapAddressModel>();
                    _isTrackResultsLoaded = false;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private void geocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            try
            {
                if (e.Result.Count != 0)
                {
                    SearchDestinationPopup.IsOpen = true;
                    _hideSearchDestinationPopup = false;
                    MyMap.Opacity = 0.3;
                    foreach (var item in e.Result)
                    {
                        var temp = item.Information.Address;
                        MapAddressModel mapobj = new MapAddressModel();

                        mapobj.Address = temp.BuildingRoom + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.BuildingFloor))
                            mapobj.Address += temp.BuildingFloor + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.BuildingName))
                            mapobj.Address += temp.BuildingName + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.BuildingZone))
                            mapobj.Address += temp.BuildingZone + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.HouseNumber))
                            mapobj.Address += temp.HouseNumber + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.Street))
                            mapobj.Address += temp.Street + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.Neighborhood))
                            mapobj.Address += temp.Neighborhood + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.Township))
                            mapobj.Address += temp.Township + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.District))
                            mapobj.Address += temp.District + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.County))
                            mapobj.Address += temp.County + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.City))
                            mapobj.Address += temp.City + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.State))
                            mapobj.Address += temp.State + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.Province))
                            mapobj.Address += temp.Province + ", ";
                        if (!string.IsNullOrWhiteSpace(temp.County))
                            mapobj.Address += temp.County;

                        mapobj.Address = mapobj.Address.Trim(new char[] { ',', ' ' });
                        mapobj.GeoCoordinate = item.GeoCoordinate;
                        _locationModel.Add(mapobj);
                    }

                    destinationSearchResult.DataContext = _locationModel;
                    PageProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (_isTrackResultsLoaded && _locationModel.Count == 0)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.RefineSearchText, "basic", "Oops!"));
                    }
                }
                _isTrackResultsLoaded = false;
            }
            catch (Exception exc)
            {
            }
            finally
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void GeoQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            try
            {
                Route route = e.Result;
                this.MapRoute = new MapRoute(route);
                SearchTextBox.Text = "";
                if (SearchDestinationPopup.IsOpen && !_hideSearchDestinationPopup)
                {
                    SearchDestinationPopup.IsOpen = false;
                    MyMap.Opacity = 1.0;
                }
                _hideSearchDestinationPopup = false;
                this.MyMap.SetView(route.BoundingBox);
                this.MyMap.AddRoute(this.MapRoute);
                this.ChangeMode(MapMode.Route);
            }
            catch (Exception exc)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.RanOutOfDestination, "basicWrap", "Oops!"));
            }
            finally
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
            }
        }



        #endregion


        #region Helpers

        /// <summary>
        /// Show the user location in the map
        /// </summary>
        /// <returns>Task that can used to await</returns>
        private async Task<GeoCoordinate> ShowUserLocation()
        {
            GeoCoordinate gc = await Utility.GetLocationQuick();
            if (gc != null)
            {
                if (_pushpinMapLayer.Count > 1)
                {
                    if (_prevMapOverlay != null)
                        _pushpinMapLayer.Remove(_prevMapOverlay);
                    _prevMapOverlay = Utility.DrawPushpin(_pushpinMapLayer, gc, true);
                }
                else
                    Utility.DrawPushpin(_pushpinMapLayer, gc, false);

                MyMap.ZoomLevel = MapZoomLevel;
                this.MyMap.Center = gc;
            }
            return gc;
        }

        /// <summary>
        /// Setup the map extensions objects.
        /// All named objects inside the map extensions will have its references properly set
        /// </summary>
        /// <param name="map">The map that uses the map extensions</param>
        private void MapExtensionsSetup(Map map)
        {
            try
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
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        /// <summary>
        /// Changes the effective map mode. Will switch visuals state it changed
        /// </summary>
        /// <param name="mode">New map mode</param>
        private void ChangeMode(MapMode mode)
        {
            try
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
                                _pushpinMapLayer.Remove(_searchBarMapOverlay);
                                this.MyMap.RemoveRoute(this.MapRoute);
                            }

                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        #endregion

        #region AppBar Events
        private void ViewAppBarItem_OnClick(object sender, EventArgs e)
        {
            var ViewMenu = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;
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


        private void ClearRoute_OnClick(object sender, EventArgs e)
        {
            if (this.MapRoute != null)
            {
                _pushpinMapLayer.Remove(_searchBarMapOverlay);
                Globals.RouteDirectionsPushPinGeoCoordinate = null;
                Globals.TrackMeDestinationName = null;
                this.RouteDirectionsPushPin.Visibility = Visibility.Collapsed;
                this.MyMap.RemoveRoute(this.MapRoute);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NoRouteToClear, "basic", "Info!"));
            }
        }

        private async void SMSBuddiesTrackUrl_OnClick(object sender, EventArgs e)
        {
            if (Globals.CurrentProfile.IsTrackingOn)
            {
                await SendTrackLinkAsync();
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TrackingNotEnabledText, "basic", "Info!"));
            }
        }
        private async Task SendTrackLinkAsync()
        {
            try
            {
                string message = await Utility.GetTrackUrlMessage();

                SMSMessage smsMgs = new SMSMessage();
                smsMgs.PhoneNumbers = Utility.GetBuddyNumbers();
                smsMgs.Message = message;

                Utility.SendSMS(smsMgs);
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }
        #endregion

        #region Tap Event Handlers
        private async void DestinationSearchResult_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _positionCenter = false;
            ChangeFocusMeIcons();
            PageProgressBar.Visibility = Visibility.Visible;
            List<GeoCoordinate> wayPoints;
            RouteQuery query;
            wayPoints = new List<GeoCoordinate>();
            GeoCoordinate gc = await Utility.GetLocationQuick();
            wayPoints.Add(gc);

            if (this.MapRoute != null)
            {
                _pushpinMapLayer.Remove(_searchBarMapOverlay);
                this.MyMap.RemoveRoute(this.MapRoute);
            }

            try
            {
                MapAddressModel destinationSelection = ((System.Windows.FrameworkElement)(sender)).DataContext as MapAddressModel;
                wayPoints.Add(destinationSelection.GeoCoordinate);
                query = new RouteQuery();
                query.Waypoints = wayPoints;
                _searchBarMapOverlay = Utility.DrawPushpin(_pushpinMapLayer, destinationSelection.GeoCoordinate, true, true);
                Globals.RouteDirectionsPushPinGeoCoordinate = destinationSelection.GeoCoordinate;
                Globals.TrackMeDestinationName = destinationSelection.Address;
                query.QueryCompleted += GeoQuery_QueryCompleted;
                query.QueryAsync();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void SearchTextBoxTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {

                SearchTextBox.Focus();
                destinationSearchResult.DataContext = null;
                if (SearchDestinationPopup.IsOpen && !_hideSearchDestinationPopup)
                {
                    SearchDestinationPopup.IsOpen = false;
                    MyMap.Opacity = 1.0;
                }
                _hideSearchDestinationPopup = false;

            }
            catch (Exception ex)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(ex, true);
            }
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                parseLocalHelpData(e.Result);
                _isTrackResultsLoaded = true;
            }
            catch { }
        }

        void parseLocalHelpData(string jsonString)
        {
            try
            {

                JObject parsedData = JObject.Parse(jsonString.Replace("microsoftMapsNetworkCallback(", "").Replace(".d},'r229');", "}"));
                foreach (var data in parsedData)
                {
                    if (data.Key.Equals("response"))
                    {
                        if (_locationModel == null)
                            _locationModel = new ObservableCollection<MapAddressModel>();
                        SearchDestinationPopup.IsOpen = true;
                        _hideSearchDestinationPopup = false;
                        MyMap.Opacity = 0.3;

                        foreach (var result in data.Value)
                        {
                            if (result.First["SearchResults"].Count() > 0)
                            {
                                foreach (var child in result.First["SearchResults"])
                                {
                                    MapAddressModel mapobj = new MapAddressModel();
                                    if (!string.IsNullOrWhiteSpace(child["Name"].ToString()))
                                        mapobj.Address += child["Name"].ToString() + ", ";
                                    if (!string.IsNullOrWhiteSpace(child["Address"].ToString()))
                                        mapobj.Address += child["Address"].ToString() + ", ";
                                    if (!string.IsNullOrWhiteSpace(child["City"].ToString()))
                                        mapobj.Address += child["City"].ToString() + ", ";
                                    if (!string.IsNullOrWhiteSpace(child["State"].ToString()))
                                        mapobj.Address += child["State"].ToString() + ", ";
                                    if (!string.IsNullOrWhiteSpace(child["Country"].ToString()))
                                        mapobj.Address += child["Country"].ToString();

                                    mapobj.Address = mapobj.Address.Trim(new char[] { ',', ' ' });
                                    mapobj.GeoCoordinate = new GeoCoordinate(double.Parse(child["Location"]["Latitude"].ToString()), double.Parse(child["Location"]["Longitude"].ToString()));
                                    _locationModel.Add(mapobj);
                                }
                            }
                            else
                            {
                                if (_isTrackResultsLoaded && _locationModel.Count == 0)
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.RefineSearchText, "basic", "Oops!"));
                                }
                            }
                        }
                        destinationSearchResult.DataContext = _locationModel;
                        PageProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
                _isTrackResultsLoaded = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                PageProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        private void PhotoButton_OnClick(object sender, EventArgs e)
        {
            //1. Async capture Geo Loc. 2. Capture Cam 3. Await for 1st - Send loc and pic to service async
            try
            {
                CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Completed += new EventHandler<PhotoResult>(EvidenceCameraCaptureTask_Completed);
                cameraCaptureTask.Show();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private async void EvidenceCameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                try
                {
                    if (Globals.CurrentProfile.IsSOSOn || Globals.CurrentProfile.IsTrackingOn)
                    {
                        GeoCoordinate gc = await Utility.GetLocationQuick();
                        await LocationServiceWrapper.PostMyLocationAsync(gc, e.ChosenPhoto);
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidPhotoCapture, "basicWrap", "Evidence upload failed!"));
                    }
                }
                catch (Exception ex)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.EvidenceUploadFailText, "basicWrap", "Evidence upload failed!"));
                }

            }
        }


    }

}
