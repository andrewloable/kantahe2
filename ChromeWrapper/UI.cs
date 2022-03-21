using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace ChromeWrapper
{
    public class UI
    {
        public static void New(string url, int width, int height, bool kioskMode = false, string tempdir = "chromewrapper", List<string> customArgs = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = "data:text/html,<html></html>";

            List<string> args = Chrome.DefaultChromeArgs();

            args.Add(GenerateLoadScript(url, width, height, kioskMode));

            if (customArgs != null)
                args.AddRange(customArgs);
            string tmppath = Path.Combine(Path.GetTempPath(), tempdir);
            args.Add($"--user-data-dir={tmppath}");
            var chromeLocation = Chrome.LocateChrome();
            if (string.IsNullOrWhiteSpace(chromeLocation))
            {
                Console.WriteLine("Chrome not found. Please install it before running this app.");
                var chromeLink = "https://www.google.com/chrome/";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {chromeLink}") { CreateNoWindow = true });
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Process.Start("open", chromeLink);
                else
                    Process.Start("xdg-open", chromeLink);
            }
            else
                Chrome.NewChromeWithArgs(chromeLocation, args);

            
        }

        private static string GenerateLoadScript(string url, int width, int height, bool kioskMode = false)
        {
            if (kioskMode)
            {
                CloseRunningChromeInstances();
                return $"--kiosk {url}";
            }                

            return $"-app=\"data:text/html,<html><body><script>window.resizeTo({width},{height});window.location='{url}';</script></body></html>\"";
        }

        private static void CloseRunningChromeInstances()
        {
            Process[] chromeInstances = Process.GetProcessesByName("chrome");
            foreach (var p in chromeInstances)
                p.Kill();
        }
    }
}
