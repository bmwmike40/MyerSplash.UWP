using MyerSplash.Data;
using MyerSplashShared.Utils;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class WallpaperAutoChangeTask : IBackgroundTask
    {
        private const string KEY = "BackgroundWallpaperSource";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("===========background task run==============");
            var defer = taskInstance.GetDeferral();
            await SimpleWallpaperSetter.ChangeWallpaperAsync();
            defer.Complete();
        }
    }
}