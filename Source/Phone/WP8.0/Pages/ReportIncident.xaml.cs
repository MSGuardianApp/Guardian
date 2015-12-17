using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using SOS.Phone.ServiceWrapper;
using System.Threading.Tasks;
using System.IO;

namespace SOS.Phone.Pages
{
    public partial class ReportIncident : PhoneApplicationPage
    {
        string Category = string.Empty;

        public ReportIncident()
        {
            InitializeComponent();
            PanelReportMessage.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void InitiateReportingIncident()
        {
            try
            {
                CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Completed += new EventHandler<PhotoResult>(ReportTeaseCameraCaptureTask_Completed);
                cameraCaptureTask.Show();
            }
            catch
            {
                //Absorb exception...
            }
        }

        private async void ReportTeaseCameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            //BL: If user cancels capturing photo(backbutton press), report just with Lat/Long
            //In this case, e.ChosenPhoto will be null
            GeoCoordinate gc = await Utility.GetLocationQuick();
            await ReportTeaseToServer(gc, e);
        }

        private async Task ReportTeaseToServer(GeoCoordinate gc, PhotoResult e)
        {
            string serverSyncStatus = string.Empty;
            try
            {

                PanelCategory.Visibility = Visibility.Collapsed;
                ButtonReportIncidentProceed.Visibility = Visibility.Collapsed;
                AddInfoStkPanel.Visibility = Visibility.Collapsed;
                PanelReportMessage.Visibility = Visibility.Visible;
                if (Globals.IsDataNetworkAvailable && Globals.IsRegisteredUser)
                {
                    byte[] b = null;

                    if (e.ChosenPhoto != null)
                    {
                        using (BinaryReader br = new BinaryReader(Phone.Utilites.PhotoResizer.ReduceSize(e.ChosenPhoto)))
                        {
                            b = br.ReadBytes((Int32)e.ChosenPhoto.Length);
                        }
                    }

                    GeoServiceRef.IncidentTag tag = new GeoServiceRef.IncidentTag()
                    {
                        Name = Globals.User.Name,
                        MobileNumber = Globals.CurrentProfile.MobileNumber,
                        Lat = (gc == null) ? string.Empty : gc.Latitude.ToString(),
                        Long = (gc == null) ? string.Empty : gc.Longitude.ToString(),
                        ProfileID = Convert.ToInt64(Globals.CurrentProfile.ProfileId),
                        SessionID = string.Empty,
                        Alt = gc.Altitude.ToString(),
                        GeoDirection = "1",//"TODO"
                        Speed = (gc == null || double.IsNaN(gc.Speed)) ? 0 : Convert.ToInt32(gc.Speed),
                        Accuracy = Math.Round(gc.HorizontalAccuracy),
                        TimeStamp = DateTime.Now.Ticks,
                        Command = Category.ToUpper(),
                        MediaContent = b,
                        AdditionalInfo = AdditionalInfoTB.Text

                    };
                    await LocationServiceWrapper.ReportIncident(tag);
                    PanelReportMessage.Visibility = Visibility.Visible;
                    serverSyncStatus = "Success";
                }
            }
            catch (Exception ex)
            {
                PanelReportMessage.Visibility = Visibility.Collapsed;
                serverSyncStatus = "Fail";
            }
            finally
            {
                if (NavigationService.BackStack != null && NavigationService.BackStack.Count() > 0)
                    NavigationService.RemoveBackEntry();

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (serverSyncStatus == "Success")
                        Globals.DisplayToast(CustomMessage.ReportTeaseToServerSuccessText, "basicWrap", "Report incident successful!");
                    else if (serverSyncStatus == "Fail")
                        Globals.DisplayToast("Reasons could be " + Environment.NewLine + "  1. GPS is turned off  " + Environment.NewLine + "  2. Unable to reach Guardian Server", "basicWrap", "Report incident failed!");
                });

            }
        }

        private void ReportIncidentProceed_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            for (int i = 0; i < this.PanelButtons.Children.Count; i++)
            {
                if (this.PanelButtons.Children[i].GetType().Name == "RadioButton")
                {
                    RadioButton radio = (RadioButton)this.PanelButtons.Children[i];
                    if ((bool)radio.IsChecked)
                    {
                        Category = radio.Content.ToString();
                        break;
                    }
                }
            }
            InitiateReportingIncident();
        }
    }
}