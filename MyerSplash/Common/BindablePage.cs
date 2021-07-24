using JP.Utils.Framework;
using System;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace MyerSplash.Common
{
    public abstract class BindablePage : Page
    {
        public event EventHandler<KeyEventArgs> GlobalPageKeyDown;

        public BindablePage()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                ConstructingInNotDesignMode();
                SetupPageAnimation();
                SetUpNavigationCache();
                IsTextScaleFactorEnabled = false;
                this.Loaded += BindablePage_Loaded;
            }
        }

        protected virtual void ConstructingInNotDesignMode()
        {
        }

        private void BindablePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is INavigable)
            {
                (this.DataContext as INavigable).OnLoaded();
            }
        }

        protected virtual void SetupPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new ContinuumNavigationTransitionInfo()
            };
            collection.Add(theme);
            Transitions = collection;
        }

        protected virtual void SetUpNavigationCache()
        {
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected virtual void SetupTitleBar()
        {
        }

        protected virtual void SetupNavigationBackBtn()
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            GlobalPageKeyDown(sender, args);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (DataContext is INavigable NavigationViewModel)
            {
                if (NavigationViewModel != null)
                {
                    NavigationViewModel.Activate(e.Parameter);
                }
            }

            SetupNavigationBackBtn();
            SetupTitleBar();

            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (DataContext is INavigable NavigationViewModel)
            {
                if (NavigationViewModel != null)
                {
                    NavigationViewModel.Deactivate(null);
                }
            }

            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            }
        }
    }
}