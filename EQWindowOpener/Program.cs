using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQWindowOpener
{
    using System;
    using System.Diagnostics;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;
    using System.Threading;

    class Program
    {
        static string windowTitlePrefix = "Client";
        static int windowTitleNum = 1;

        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }
        public static string getCurrentWindowTitle()
        {
            return String.Format("{0}{1}", windowTitlePrefix, windowTitleNum);
        }

        static void Main(string[] args)
        {
            using (System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                //Set Window Title Based on Current if Value is currently in use or not
                while (WinGetHandle(getCurrentWindowTitle()) != IntPtr.Zero)
                {
                    windowTitleNum++;
                }

                p.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = @"eqgame.exe",
                    Arguments = @"patchme",
                    WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["EqDirectory"]
            };

                string window_title = getCurrentWindowTitle();
                p.Start();
                System.Threading.SpinWait.SpinUntil(() => p.HasExited || p.MainWindowHandle != IntPtr.Zero);
                while (!p.HasExited)
                {
                    p.Refresh();
                    if(p.MainWindowTitle != getCurrentWindowTitle())
                    SetWindowText(p.MainWindowHandle, window_title);
                    Thread.Sleep(100);
                }
                p.WaitForExit();
            }
        }

    }
}
