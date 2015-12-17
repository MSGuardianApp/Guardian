using Coding4Fun.Toolkit.Controls;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SOS.Phone.Utilites.UtilityClasses
{
    public class Toast
    {
        #region Toast prompt

        public Toast(string msg,string title="Info",string imageUrl="")
        {
            this.message = msg;
           this.title = title;
           this.imageUrl = imageUrl;
        }

        public void Show(string type="basic")
        {
            switch (type)
            {
                case "basic": ToastBasic();
                    break;
                case "basicWrap": ToastWrapBasic();
                    break;
                case "ImgNoTitle": ToastWithImgAndNoTitle();
                    _prompt.Height = 100;
                    break;
            }
            
        }
        private string message;
        private string title;
        private string imageUrl;
        private ToastPrompt _prompt;
        public void ToastWithImgAndNoTitle()
        {
            InitializeToastWithImgAndNoTitle();

            _prompt.Show();
        }

        private void InitializeToastWithImgAndNoTitle(TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Message = message;
            _prompt.ImageSource = new BitmapImage(new Uri("/Assets/Images/add.png", UriKind.RelativeOrAbsolute));
            _prompt.TextWrapping = wrap;
        }

        private void InitializeBasicToast(string title = "Basic", TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Title = this.title;
            _prompt.Message = message;
            _prompt.TextWrapping = wrap;
        }

        private void ToastBasic()
        {
            InitializeBasicToast();

            _prompt.Show();
        }

        private void ToastWrapBasic()
        {
            InitializeBasicToast(wrap: TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializePrompt()
        {
            //var reuseObject = ReuseObject.IsChecked.GetValueOrDefault(false);

            if (_prompt != null)
            {
                _prompt.Completed -= PromptCompleted;
            }

            //if (!reuseObject || _prompt == null)
            {
                _prompt = new ToastPrompt();
            }

            // this is me manually resetting stuff due to the reusability test
            // you don't need to do this.
            // fontsize, foreground, background won't manually be reset

            //_prompt.TextWrapping = TextWrapping.NoWrap;
            //_prompt.ImageSource = null;
            //_prompt.ImageHeight = double.NaN;
            //_prompt.ImageWidth = double.NaN;
            //_prompt.Stretch = Stretch.None;
            //_prompt.IsAppBarVisible = false;
            //_prompt.TextOrientation = System.Windows.Controls.Orientation.Horizontal;

            //_prompt.Message = string.Empty;
            //_prompt.Title = string.Empty;

         //   _prompt.Completed += PromptCompleted;
        }

        private void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
        }

        #endregion

    }
}
