using System;
using Windows.ApplicationModel.Resources.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace MyerSplashShared.Utils
{
    public static class DeviceUtil
    {
        private static Boolean? _isDesktop;
        public static bool IsDesktop
        {
            get
            {
                if (_isDesktop == null)
                {
                    _isDesktop = HasQualifier("Desktop");
                }
                return _isDesktop.Value;
            }
        }

        private static Boolean? _isMobile;
        public static bool IsMobile
        {
            get
            {
                if (_isMobile == null)
                {
                    _isMobile = HasQualifier("Mobile");
                }
                return _isMobile.Value;
            }
        }

        private static Boolean? _isIoT;
        public static bool IsIoT
        {
            get
            {
                if (_isIoT == null)
                {
                    _isIoT = HasQualifier("IoT");
                }
                return _isIoT.Value;
            }
        }

        private static Boolean? _isXbox;
        public static bool IsXbox
        {
            get
            {
                if (_isXbox == null)
                {
                    _isXbox = HasQualifier("Xbox");
                }
                return _isXbox.Value;
            }
        }

        private static bool HasQualifier(string key)
        {
            try
            {
                var qualifiers = ResourceContext.GetForCurrentView().QualifierValues;
                return qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "key";
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        private static string[] GetDeviceOsVersion()
        {
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            return new string[] { v1.ToString(), v2.ToString(), v3.ToString(), v4.ToString() };
        }

        public static string OSVersion
        {
            get
            {
                string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                ulong v = ulong.Parse(sv);
                ulong v1 = (v & 0xFFFF000000000000L) >> 48;
                ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
                ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
                ulong v4 = (v & 0x000000000000FFFFL);
                return $"{v1}.{v2}.{v3}.{v4}";
            }
        }

        public static bool IsTH1OS
        {
            get
            {
                var versions = GetDeviceOsVersion();
                return versions[2] == "10240";
            }
        }

        public static bool IsTH2OS
        {
            get
            {
                var versions = GetDeviceOsVersion();
                return versions[2] == "10586";
            }
        }

        public static bool IsRS1OS
        {
            get
            {
                var versions = GetDeviceOsVersion();
                return versions[2] == "14393";
            }
        }

        public static bool IsRS2OS
        {
            get
            {
                var versions = GetDeviceOsVersion();
                return versions[2] == "15063";
            }
        }

        public static bool OverRS2OS
        {
            get
            {
                var versions = GetDeviceOsVersion();
                int.TryParse(versions[2], out int versionCode);

                return versionCode >= 15063;
            }
        }

        public static string DeviceModel
        {
            get
            {
                var eas = new EasClientDeviceInformation();
                return eas.SystemProductName;
            }
        }
    }
}