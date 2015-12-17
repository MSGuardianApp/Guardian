using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;

namespace SOS.Phone.Pages
{
    public partial class Help : PhoneApplicationPage
    {
        int CurrentPageNumber = 1;
        int MaxPages = 26;
        double mouseHorizontalPosition = 0;
        double mouseVerticalPosition = 0;
        bool IsMouseCaptured = false;
        public Help()
        {
            InitializeComponent();

            PageCountTextBlock.Text = CurrentPageNumber + "/" + MaxPages;
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            HelpImage.MouseLeftButtonDown += HelpImage_MouseLeftButtonDown;
            HelpImage.MouseMove += HelpImage_MouseMove;
            HelpImage.MouseLeftButtonUp += HelpImage_MouseLeftButtonUp;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            HelpImage.MouseLeftButtonDown -= HelpImage_MouseLeftButtonDown;
            HelpImage.MouseMove -= HelpImage_MouseMove;
            HelpImage.MouseLeftButtonUp -= HelpImage_MouseLeftButtonUp;
        }

        void HelpImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsMouseCaptured = false;
            e.Handled = false;
        }

        void HelpImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                // int deltaV =(int)( e.GetPosition(HelpContentPanel).Y - mouseVerticalPosition);
                int deltaH = (int)(e.GetPosition(HelpContentPanel).X - mouseHorizontalPosition);

                if (deltaH > 20)
                {
                    CurrentPageNumber = CurrentPageNumber == 1 ? MaxPages : CurrentPageNumber - 1;
                    PageCountTextBlock.Text = CurrentPageNumber + "/" + MaxPages;
                    LoadNewHelpPage();
                    IsMouseCaptured = false;
                }
                else if (deltaH < -20)
                {
                    CurrentPageNumber = CurrentPageNumber == MaxPages ? 1 : CurrentPageNumber + 1;
                    PageCountTextBlock.Text = CurrentPageNumber + "/" + MaxPages;
                    LoadNewHelpPage();
                    IsMouseCaptured = false;

                   
                }
            }
        }
        void HelpImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsMouseCaptured = true;
            mouseHorizontalPosition = e.GetPosition(HelpContentPanel).X;
            mouseVerticalPosition = e.GetPosition(HelpContentPanel).Y;
        }

        private void PreviousButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CurrentPageNumber = CurrentPageNumber == 1 ? MaxPages : CurrentPageNumber - 1;
            PageCountTextBlock.Text = CurrentPageNumber + "/" + MaxPages;
            LoadNewHelpPage();
        }

        private void NextButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CurrentPageNumber = CurrentPageNumber == MaxPages ? 1 : CurrentPageNumber + 1;
            PageCountTextBlock.Text = CurrentPageNumber + "/" + MaxPages;
            LoadNewHelpPage();
        }

        private void LoadNewHelpPage()
        {
            try
            {
                HelpImage.Source = new BitmapImage(new Uri("/Assets/HelpImages/" + CurrentPageNumber + ".png", UriKind.Relative));
            }
            catch
            {
                //Absorb exception
            }
        }
    }
}