using MyerSplash.Common;
using MyerSplash.ViewModel;
using MyerSplashShared.Utils;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace MyerSplash.View.Uc
{
    public sealed partial class SettingsControl : NavigableUserControl
    {
        private SettingsViewModel SettingsVM { get; set; }

        public SettingsControl()
        {
            this.InitializeComponent();
            if (!DesignMode.DesignModeEnabled)
            {
                this.DataContext = SettingsVM = new SettingsViewModel();
            }

            if (DeviceUtil.IsXbox)
            {
                AutoChangeSP.Visibility = Visibility.Collapsed;
                CompatModeGrid.Visibility = Visibility.Collapsed;
            }
        }

        public override async void OnPresented()
        {
            base.OnPresented();
            Window.Current.SetTitleBar(DummyTitleBar);
            await SettingsVM.UpdateCacheSizeUIAsync();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Presented = false;
        }
    }
}