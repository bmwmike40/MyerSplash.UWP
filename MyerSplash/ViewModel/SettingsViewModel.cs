using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MyerSplash.Common;
using MyerSplash.View.Uc;
using MyerSplashCustomControl;
using MyerSplashShared.Image;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.System;

namespace MyerSplash.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly DiskCacheSupplier _cacheSupplier = DiskCacheSupplier.Instance;

        private RelayCommand _diagnoseCommand;
        public RelayCommand DiagnoseCommand
        {
            get
            {
                if (_diagnoseCommand != null) return _diagnoseCommand;
                return _diagnoseCommand = new RelayCommand(async () =>
                {
                    var uc = new NetworkDiagnosisDialog();
                    await PopupService.Instance.ShowAsync(uc);
                });
            }
        }

        private RelayCommand _clearCacheCommand;
        public RelayCommand ClearCacheCommand
        {
            get
            {
                if (_clearCacheCommand != null) return _clearCacheCommand;
                return _clearCacheCommand = new RelayCommand(async () =>
                {
                    await ClearCacheAsync();
                });
            }
        }

        private bool _clearCacheCommandEnabled;
        public bool ClearCacheCommandEnabled
        {
            get
            {
                return _clearCacheCommandEnabled;
            }
            set
            {
                if (_clearCacheCommandEnabled != value)
                {
                    _clearCacheCommandEnabled = value;
                    RaisePropertyChanged(() => ClearCacheCommandEnabled);
                }
            }
        }

        private RelayCommand _clearTempCommand;
        public RelayCommand ClearTempCommand
        {
            get
            {
                if (_clearTempCommand != null) return _clearTempCommand;
                return _clearTempCommand = new RelayCommand(async () =>
                  {
                      await ClearTempFileAsync();
                  });
            }
        }

        private string _cacheHint;
        public string CacheHint
        {
            get
            {
                return _cacheHint;
            }
            set
            {
                if (_cacheHint != value)
                {
                    _cacheHint = value;
                    RaisePropertyChanged(() => CacheHint);
                }
            }
        }

        private RelayCommand _cpenSavingFolderCommand;
        public RelayCommand OpenSavingFolderCommand
        {
            get
            {
                if (_cpenSavingFolderCommand != null) return _cpenSavingFolderCommand;
                return _cpenSavingFolderCommand = new RelayCommand(async () =>
                  {
                      var folder = await AppSettings.GetSavingFolderAsync();
                      if (folder != null)
                      {
                          await Launcher.LaunchFolderAsync(folder);
                      }
                  });
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task UpdateCacheSizeUIAsync()
        {
            CacheHint = ResourcesHelper.GetResString("CalculatingCache");
            ClearCacheCommandEnabled = false;
            var size = await _cacheSupplier.GetSizeAsync();

            var sizeFormatted = (size / (1024 * 1024)).ToString("f0");
            CacheHint = ResourcesHelper.GetFormattedResString("CleanUpContent", sizeFormatted);
            ClearCacheCommandEnabled = true;
        }

        private async Task ClearTempFileAsync()
        {
            var folder = await AppSettings.GetSavingFolderAsync();
            var files = await folder.GetFilesAsync();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var prop = await file.GetBasicPropertiesAsync();
                    if (file.Name.EndsWith(".tmp") || prop.Size == 0)
                    {
                        await file.DeleteAsync();
                    }
                }
            }

            ToastService.SendToast(ResourcesHelper.GetResString("TempFilesCleaned"));
        }

        private async Task ClearCacheAsync()
        {
            ClearCacheCommandEnabled = false;
            CacheHint = ResourceLoader.GetForCurrentView().GetString("Cleaning");
            await _cacheSupplier.ClearAsync();
            ToastService.SendToast(ResourcesHelper.GetResString("TempFilesCleaned"), TimeSpan.FromMilliseconds(1000));
            await UpdateCacheSizeUIAsync();
        }
    }
}