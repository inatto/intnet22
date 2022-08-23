using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.general
{
    public static class GeneralModule
    {
        /**
         * MySql para tipo data c#
         */
        public static string RemoveMasks(string? str)
        {
            //
            str ??= "";
            const string matches = " ()-_./\\";
            foreach (var charr in matches)
                str = str.Replace(charr.ToString(), "");

            //
            return str;
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }


        public static void BucketIconFc(Image image, string icon)
        {
            //
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"https://dmo8uuv30ja5t.cloudfront.net/fcicons/{icon}.png", UriKind.Absolute);
            bitmap.EndInit();

            //
            image.Source = bitmap;
        }

        public static void UrlIcon(Image image, string url)
        {
            //
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();

            //
            image.Width = 16;
            image.Source = bitmap;
        }




    }
}