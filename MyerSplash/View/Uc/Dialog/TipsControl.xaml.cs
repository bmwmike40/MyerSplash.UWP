using MyerSplashCustomControl;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.View.Uc
{
    public sealed partial class TipsControl : UserControl
    {
        public TipsControl()
        {
            this.InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupService.Instance.TryHide();
        }

        private async void GoToWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://juniperphoton.dev/myersplash/"));
        }
    }
}