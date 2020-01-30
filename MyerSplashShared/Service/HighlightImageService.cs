using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyerSplash.Data;
using MyerSplashShared.Data;

namespace MyerSplashShared.Service
{
    public class HighlightImageService : ImageServiceBase
    {
        private static DateTime END_TIME => DateTime.Parse("2017/03/20");
        private static int COUNT => 20;

        public HighlightImageService(UnsplashImageFactory factory,
            CancellationTokenSourceFactory ctsFactory) : base(factory, ctsFactory)
        {
        }

        public async override Task<IEnumerable<UnsplashImage>> GetImagesAsync()
        {
            return await Task.FromResult(GetImages(Page, COUNT));
        }

        public IEnumerable<UnsplashImage> GetImages(int page, int count)
        {
            var list = new List<UnsplashImage>();

            var start = DateTime.Now.AddDays(-(page - 1) * count);

            for (var i = 0; i < count; i++)
            {
                var next = start.AddDays(-i);
                if (next < END_TIME)
                {
                    break;
                }
                list.Add(UnsplashImageFactory.CreateHighlightImage(next, true));
            }

            return list;
        }
    }
}
