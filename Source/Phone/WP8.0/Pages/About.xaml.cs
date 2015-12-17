using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace SOS.Phone.Pages
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string strItemIndex;
            if (NavigationContext.QueryString.TryGetValue("goto", out strItemIndex))
                PagePivot.SelectedIndex = Convert.ToInt32(strItemIndex);

            base.OnNavigatedTo(e);
        }


        private void SendReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.logger.hasCriticalLogged())
                {
                    App.logger.emailReport();
                    App.logger.clearEventsFromLog();
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.SendReportErrorText, "basicWrap", "Info!"));
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private void MailTo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:guardianapp@outlook.com"));
        }

       	private void SendLinkAppBarItem_OnClick(object sender, EventArgs e)
        {
            Utility.SendEmail(new EmailMessage() { Subject = "Guardian App download", HtmlBody = "Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. Download Guardian App for Windows Phone: http://tinyurl.com/lq63mzq , Android: http://tinyurl.com/nzgcdeu and iOS: http://tinyurl.com/nrxwll2" });
        }



        private void MessageLinkAppBarItem_OnClick(object sender, EventArgs e)
        {
            Utility.SendSMS(new SMSMessage() { PhoneNumbers = "", Message = "Download Guardian App for Windows Phone: http://tinyurl.com/lq63mzq , Android: http://tinyurl.com/nzgcdeu and iOS: http://tinyurl.com/nrxwll2" });
        }

        private void ShareLinkAppBarItem_OnClick(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Guardian App";
            shareLinkTask.LinkUri = new Uri("http://tinyurl.com/lq63mzq", UriKind.Absolute);
            shareLinkTask.Message = "Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. Download Guardian App for Windows Phone: http://tinyurl.com/lq63mzq , Android: http://tinyurl.com/nzgcdeu and iOS: http://tinyurl.com/nrxwll2";

            shareLinkTask.Show();
        }

        private void RatenReviewAppBarItem_OnClick(object sender, EventArgs e)
        {
            MarketplaceReviewTask mrTask = new MarketplaceReviewTask();
            mrTask.Show();
        }
    }
}