using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;

namespace MyerSplash.Common
{
    public static class Events
    {
        public static void LogSelected(string name)
        {
            Analytics.TrackEvent($"Tab Selected: {name}");
        }

        public static void LogRefreshList(int selectedIndex)
        {
            Analytics.TrackEvent("RefreshList", new Dictionary<string, string> { { "Index", selectedIndex.ToString() } });
        }

        public static void LogEnterDownloads()
        {
            Analytics.TrackEvent("Enter downloads");
        }

        public static void LogEnterSearch()
        {
            Analytics.TrackEvent("Enter search");
        }

        public static void LogEnterSettings()
        {
            Analytics.TrackEvent("Enter settings");
        }

        public static void LogEnterAbout()
        {
            Analytics.TrackEvent("Enter about");
        }

        public static void LogDownloadButtonOnList()
        {
            Analytics.TrackEvent("Click download button on list");
        }
        public static void LogDownloadButtonOnDetails()
        {
            Analytics.TrackEvent("Click download button on details");
        }

        public static void LogSetAsInDetails()
        {
            Analytics.TrackEvent("Click set-as button on details");
        }

        public static void LogCancelInDetails()
        {
            Analytics.TrackEvent("Click cancel button on details");
        }

        public static void LogDragToDismiss()
        {
            Analytics.TrackEvent("Drag to dismiss");
        }

        public static void LogToggleImageDetails()
        {
            Analytics.TrackEvent("Toggle image details");
        }

        public static void LogSetAsPreview()
        {
            Analytics.TrackEvent("Toggle set-as preview");
        }

        public static void LogPhotoInfoDetails()
        {
            Analytics.TrackEvent("Toggle photo info details");
        }

        public static void LogCompatMode(bool on)
        {
            LogSwitch("Swtich compat mode", on);
        }

        public static void LogTile(bool on)
        {
            LogSwitch("Swtich live tile", on);
        }

        public static void LogScaleAnimation(bool on)
        {
            LogSwitch("Swtich scale animation", on);
        }

        public static void LogBackgroundWallpapersSource(int source)
        {
            Analytics.TrackEvent("Change background wallpapers source", new Dictionary<string, string> { { "Source", source.ToString() } });
        }

        public static void LogBackgroundWallpapersInterval(int intervalOption)
        {
            Analytics.TrackEvent("Change background wallpapers interval", new Dictionary<string, string> { { "Interval", intervalOption.ToString() } });
        }

        public static void LogSwitchLanguage(int itemIndex)
        {
            Analytics.TrackEvent("Change language", new Dictionary<string, string> { { "ItemIndex", itemIndex.ToString() } });
        }

        public static void LogSwitchTheme(int itemIndex)
        {
            Analytics.TrackEvent("Change theme", new Dictionary<string, string> { { "ItemIndex", itemIndex.ToString() } });
        }

        public static void LogDownloadError(Exception e, string url, long durationMs)
        {
            Analytics.TrackEvent("Download exception", new Dictionary<string, string> {
                { "Error", e.ToString() },
                { "Url", url },
                { "DurationMillis", durationMs.ToString()}
            });
        }

        public static void LogDownloadSuccess(long durationMs)
        {
            Analytics.TrackEvent("Download success", new Dictionary<string, string> {
                { "DurationMillis", durationMs.ToString()}
            });
        }

        public static void LogSetAsBoth()
        {
            LogSetAsTarget(0);
        }

        public static void LogSetAsDesktop()
        {
            LogSetAsTarget(1);
        }

        public static void LogSetAsLockscreen()
        {
            LogSetAsTarget(2);
        }

        private static void LogSetAsTarget(int target)
        {
            var targetName = "";
            switch (target)
            {
                case 0: targetName = "Both"; break;
                case 1: targetName = "Desktop"; break;
                default: targetName = "Lockscreen"; break;
            }
            Analytics.TrackEvent("Set as", new Dictionary<string, string> { { "Target", targetName } });
        }

        private static void LogSwitch(string name, bool on)
        {
            Analytics.TrackEvent(name, new Dictionary<string, string> { { "IsOn", on.ToString() } });
        }
    }
}
