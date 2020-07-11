using MyerSplash.Model;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class DownloadItemAdatper : AnimatedAdapter
    {
        public async override void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args != null && args.Item is DownloadItem item)
            {
                await item?.ImageItem?.TryLoadBitmapAsync();
            }
        }
    }
}
