using MyerSplash.Common;
using MyerSplash.Data;
using MyerSplashShared.Data;
using MyerSplashShared.Service;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;

namespace MyerSplashShared.Utils
{
    public static class SimpleWallpaperSetter
    {
        public async static Task ChangeWallpaperAsync()
        {
            var service = new RandomImageService(new UnsplashImageFactory(false), CancellationTokenSourceFactory.CreateDefault());
            var list = await service.GetImagesAsync();

            if (list == null)
            {
                return;
            }

            var enumerator = list.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return;
            }

            var image = enumerator.Current;
            var url = image.Urls.Full;
            var result = await DownloadAndSetAsync(url);
            Debug.WriteLine($"===========result {result}==============");
        }

        public static async Task<bool> DownloadAndSetAsync(string url)
        {
            if (!UserProfilePersonalizationSettings.IsSupported())
            {
                return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var client = new HttpClient();

                    var segments = new Uri(url).Segments;
                    if (segments.Length <= 0)
                    {
                        return false;
                    }

                    var fileName = segments[segments.Length - 1] + ".jpg";

                    var pictureLib = await KnownFolders.PicturesLibrary.CreateFolderAsync("MyerSplash",
                        CreationCollisionOption.OpenIfExists);
                    var targetFolder = await pictureLib.CreateFolderAsync("Auto-change wallpapers",
                        CreationCollisionOption.OpenIfExists);

                    var localFolder = ApplicationData.Current.LocalFolder;

                    Debug.WriteLine($"===========url {url}==============");

                    LiveTileUpdater.UpdateLiveTile();

                    StorageFile file;

                    var imageResp = await client.GetAsync(url);
                    using (var stream = await imageResp.Content.ReadAsStreamAsync())
                    {
                        Debug.WriteLine($"===========download complete==============");

                        file = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                        var bytes = new byte[stream.Length];
                        await stream.ReadAsync(bytes, 0, (int)stream.Length);
                        await FileIO.WriteBytesAsync(file, bytes);

                        // File must be in local folder
                        file = await file.CopyAsync(localFolder, fileName, NameCollisionOption.ReplaceExisting);

                        Debug.WriteLine($"===========save complete==============");
                    }
                    if (file != null)
                    {
                        var setResult = false;
                        var value = (int)ApplicationData.Current.LocalSettings.Values["BackgroundWallpaperSource"];
                        switch (value)
                        {
                            case 0:
                                break;
                            case 1:
                                setResult = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);
                                Events.LogSetAsDesktop();
                                break;
                            case 2:
                                setResult = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);
                                Events.LogSetAsLockscreen();
                                break;
                            case 3:
                                var setDesktopResult = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);
                                var setLockscreenResult = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);
                                setResult = setDesktopResult && setLockscreenResult;
                                Events.LogSetAsBoth();
                                break;
                        }
                        Debug.WriteLine($"===========TrySetWallpaperImageAsync result{setResult}=============");
                        return setResult;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"===========TrySetWallpaperImageAsync failed {e.Message}=============");
                return false;
            }
        }
    }
}