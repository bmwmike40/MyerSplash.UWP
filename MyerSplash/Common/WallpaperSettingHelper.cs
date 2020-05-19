using MyerSplash.View.Uc;
using MyerSplashCustomControl;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;

namespace MyerSplash.Common
{
    public static class WallpaperSettingHelper
    {
        public static async Task SetAsBackgroundAsync(StorageFile savedFile)
        {
            Events.LogSetAsDesktop();

            var uc = new LoadingTextControl() { LoadingText = ResourcesHelper.GetResString("SettingDesktopHint") };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false);

            var file = await PrepareImageFileAsync(savedFile);
            if (file != null)
            {
                var result = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);

                if (!result)
                {
                    ShowFailedToast();
                }
            }

            PopupService.Instance.TryHide(500);
        }

        public static async Task SetAsLockscreenAsync(StorageFile savedFile)
        {
            Events.LogSetAsLockscreen();

            var uc = new LoadingTextControl() { LoadingText = ResourcesHelper.GetResString("SettingLockHint") };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false);

            var file = await PrepareImageFileAsync(savedFile);
            if (file != null)
            {
                var result = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);

                if (!result)
                {
                    ShowFailedToast();
                }
            }

            PopupService.Instance.TryHide(500);
        }

        public static async Task SetBothAsync(StorageFile savedFile)
        {
            Events.LogSetAsBoth();

            var uc = new LoadingTextControl() { LoadingText = ResourcesHelper.GetResString("SettingDesktopAndLockHint") };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false).ConfigureAwait(true);

            var file = await PrepareImageFileAsync(savedFile).ConfigureAwait(true);
            if (file != null)
            {
                var result0 = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);
                var result1 = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);

                if (!result0 || !result1)
                {
                    ShowFailedToast();
                }
            }

            PopupService.Instance.TryHide(500);
        }

        private static async Task<StorageFile> PrepareImageFileAsync(StorageFile resultFile)
        {
            if (!UserProfilePersonalizationSettings.IsSupported())
            {
                ToastService.SendToast(ResourcesHelper.GetResString("SettingWallpaperNotSupported"));
                return null;
            }

            try
            {
                if (resultFile != null)
                {
                    //WTF, the file should be copy to LocalFolder to make the setting wallpaer api work.
                    var folder = ApplicationData.Current.LocalFolder;
                    StorageFile file;
                    if (await folder.TryGetItemAsync(resultFile.Name) is StorageFile oldFile)
                    {
                        await resultFile.CopyAndReplaceAsync(oldFile);
                        file = oldFile;
                    }
                    else
                    {
                        file = await resultFile.CopyAsync(folder);
                    }
                    return file;
                }
            }
            catch (Exception)
            {
                ShowFailedToast();
            }

            return null;
        }

        private static void ShowFailedToast()
        {
            ToastService.SendToast(ResourcesHelper.GetResString("FailedToSetHint"));
        }
    }
}