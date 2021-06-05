using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace MyerSplashShared.Utils
{
    public class Keys
    {
        public string AppCenterKey { get; private set; }
        public string ClientKey { get; private set; }

        private Keys()
        {
            // private contructor
        }

        public static readonly Keys Instance = new Keys();

        public async Task InitializeAsync()
        {
            var uri = new Uri("ms-appx:///Assets/Json/keys.json");

            StorageFile file;
            try
            {
                file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentNullException("Please create a file named keys.json in assets folder");
            }

            var jsonString = await FileIO.ReadTextAsync(file);

            var o = JsonObject.Parse(jsonString);
            AppCenterKey = o.GetNamedString("app_center_key");
            ClientKey = o.GetNamedString("client_key");

            if (ClientKey == null)
            {
                throw new ArgumentNullException("ClientKey is null");
            }
        }
    }
}
