using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ChromeWrapper
{
    public class Chrome
    {
        /// <summary>
        /// Run google chrome with command line arguments
        /// </summary>
        /// <param name="chomePath"></param>
        /// <param name="args"></param>
        public static void NewChromeWithArgs(string chromePath, List<string> args, bool waitExit = false)
        {
            string argString = string.Join(" ", args);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = chromePath;
            startInfo.Arguments = argString;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            process.StartInfo = startInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Start();
            if (waitExit)
                process.WaitForExit();
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"** Error {e.Data}");
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"{e.Data}");
        }

        /// <summary>
        /// Returns all possible common locations of google chrome or chromium
        /// </summary>
        /// <returns></returns>
        public static string LocateChrome()
        {
            List<string> paths;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                paths = darwinLocations();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                paths = windowsLocations();
            else
                paths = linuxUnixLocations();

            foreach(var path in paths)
            {
                if (File.Exists(path))
                    return path;
            }
            return string.Empty;
        }
        /// <summary>
        /// All possible chrome/chromium locations in macos
        /// </summary>
        /// <returns></returns>
        private static List<string> darwinLocations()
        {
            var retval = new List<string>()
            {
                "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
                "/Applications/Google Chrome Canary.app/Contents/MacOS/Google Chrome Canary",
                "/Applications/Chromium.app/Contents/MacOS/Chromium",
                "/usr/bin/google-chrome-stable",
                "/usr/bin/google-chrome",
                "/usr/bin/chromium",
                "/usr/bin/chromium-browser"
            };

            return retval;
        }
        /// <summary>
        /// All possible chrome/chromium locations in windows
        /// </summary>
        /// <returns></returns>
        private static List<string> windowsLocations()
        {
            var retval = new List<string>()
            {
                $"C:/Users/{Environment.UserName}/AppData/Local/Google/Chrome/Application/chrome.exe",
                "C:/Program Files (x86)/Google/Chrome/Application/chrome.exe",
                "C:/Program Files/Google/Chrome/Application/chrome.exe",
                $"C:/Users/{Environment.UserName}/AppData/Local/Chromium/Application/chrome.exe"
            };

            return retval;
        }
        /// <summary>
        /// All possible chrome/chromium locations in linux/unix
        /// </summary>
        /// <returns></returns>
        private static List<string> linuxUnixLocations()
        {
            var retval = new List<string>()
            {
                "/usr/bin/google-chrome-stable",
                "/usr/bin/google-chrome",
                "/usr/bin/chromium",
                "/usr/bin/chromium-browser",
                "/snap/bin/chromium"
            };

            return retval;
        }
        public static List<string> DefaultChromeArgs()
        {
            var retval = new List<string>()
            {
                "--autoplay-policy=no-user-gesture-required",
                "--disable-background-networking",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-breakpad",
                "--disable-client-side-phishing-detection",
                "--disable-default-apps",
                "--disable-dev-shm-usage",
                "--disable-extensions",
                "--disable-features=site-per-process",
                "--disable-hang-monitor",
                "--disable-ipc-flooding-protection",
                "--disable-popup-blocking",
                "--disable-prompt-on-repost",
                "--disable-renderer-backgrounding",
                "--disable-sync",
                "--disable-translate",
                "--metrics-recording-only",
                "--no-first-run",
                "--safebrowsing-disable-auto-update",
                "--password-store=basic",
                "--use-mock-keychain",
                "--remote-debuggin-port=0"
            };

            return retval;
        }
    }
}
