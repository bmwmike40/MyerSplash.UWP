using System;
using System.Collections.Generic;
using System.Web;

namespace MyerSplashShared.API
{
    public static class Request
    {
        public static string HOST => "api.unsplash.com";

        public static string JP_HOST => "juniperphoton.dev";

        public static string GetNewImages => $"https://{HOST}/photos?";

        public static string SearchImages => $"https://{HOST}/search/photos?";

        public static string GetRandomImages => $"https://{HOST}/photos/random?";

        public static string GetCategories => $"https://{HOST}/categories?";

        public static string GetDeveloperPhotos => $"https://{HOST}/users/juniperphoton/photos";

        public static string GetImageDetail => $"https://{HOST}/photos/";

        public static string GetTodayWallpaper => $"https://{JP_HOST}/myersplash/wallpapers";

        public static string GetTodayThumbWallpaper => $"https://{JP_HOST}/myersplash/wallpapers/thumbs";

        public static string AppendParamsToUrl(string baseUrl, List<KeyValuePair<string, string>> paramList)
        {
            var builder = new UriBuilder(baseUrl);

            var query = HttpUtility.ParseQueryString(builder.Query);
            paramList.ForEach(e => query.Add(e.Key, e.Value));
            builder.Query = query.ToString();

            return builder.Uri.ToString();
        }
    }
}