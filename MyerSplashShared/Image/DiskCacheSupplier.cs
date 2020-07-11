using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyerSplashShared.Image
{
    public class DiskCacheSupplier : ICacheSupplier<StorageFile>
    {
        private static readonly Lazy<DiskCacheSupplier> lazy = new Lazy<DiskCacheSupplier>(() => new DiskCacheSupplier());

        public static DiskCacheSupplier Instance { get { return lazy.Value; } }

        private Dictionary<string, StorageFile> _cacheMap = new Dictionary<string, StorageFile>();

        private DiskCacheSupplier()
        {
            // private constructor
        }

        public async Task<bool> CheckCacheExistAsync(string key)
        {
            return (await TryGetCacheAsync(key)) != null;
        }

        public async Task<StorageFile> TryGetCacheAsync(string key)
        {
            if (_cacheMap.ContainsKey(key))
            {
                return _cacheMap[key];
            }
            var folder = await OpenCacheFolderAsync();
            var file = await folder.TryGetFileAsync(key);
            if (file != null)
            {
                _cacheMap[key] = file;
            }
            return file;
        }

        public async Task<StorageFile> GetFileToSaveAsync(string key)
        {
            var folder = await OpenCacheFolderAsync();
            return await folder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
        }

        private async Task<StorageFolder> OpenCacheFolderAsync()
        {
            var tempFolder = ApplicationData.Current.TemporaryFolder;
            return await tempFolder.CreateFolderAsync("cache", CreationCollisionOption.OpenIfExists);
        }

        private string GenerateRandomFileName()
        {
            return DateTime.Now.ToFileTime().ToString() + ".jpg";
        }

        public async Task<long> GetSizeAsync()
        {
            ulong size = 0;
            var tempFiles = await (await OpenCacheFolderAsync()).GetItemsAsync();
            foreach (var file in tempFiles)
            {
                var properties = await file.GetBasicPropertiesAsync();
                size += properties.Size;
            }
            return (long)size;
        }

        public async Task ClearAsync()
        {
            _cacheMap.Clear();
            var folder = await OpenCacheFolderAsync();
            await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
