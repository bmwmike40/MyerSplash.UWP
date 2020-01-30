using JP.Utils.Debug;
using MyerSplashShared.Service;
using NotificationsExtensions.Tiles;
using System;
using Windows.UI.Notifications;

namespace MyerSplashShared.Utils
{
    public static class LiveTileUpdater
    {
        public static void UpdateLiveTile()
        {
            try
            {
                var tile = new TileBinding();
                var photosContent = new TileBindingContentPhotos();

                var list = new HighlightImageService(null, null).GetImages(1, 5);
                foreach(var e in list)
                {
                    photosContent.Images.Add(new TileImageSource(e.Urls.Tile));
                }

                tile.Content = photosContent;

                var tileContent = new TileContent
                {
                    Visual = new TileVisual()
                };
                tileContent.Visual.Branding = TileBranding.NameAndLogo;
                tileContent.Visual.TileMedium = tile;
                tileContent.Visual.TileWide = tile;
                tileContent.Visual.TileLarge = tile;

                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileContent.GetXml()));
            }
            catch (Exception e)
            {
                _ = Logger.LogAsync(e);
            }
        }

        public static void CleanUpTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }
    }
}