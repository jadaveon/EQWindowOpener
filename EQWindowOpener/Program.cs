using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ConsoleHotKey;

namespace EQWindowOpener
{


    class Program
    {
        static string windowTitlePrefix = "Client";
        static int windowTitleNum = 1;
        static IntPtr windowHandle = IntPtr.Zero;

        static void Main(string[] args)
        {
            using (System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                //Set Window Title Based on Current if Value is currently in use or not
                while (WinGetHandle(GetCurrentWindowTitle()) != IntPtr.Zero)
                {
                    windowTitleNum++;
                }

                p.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = @"eqgame.exe",
                    Arguments = @"patchme",
                    WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["EqDirectory"]
                };
                string window_title = GetCurrentWindowTitle();

                //Set HotKey Based on Window Number
                if (GetHotkey() != Keys.None)
                {
                    HotKeyManager.RegisterHotKey(GetHotkey(), GetKeyMod());
                    HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
                }

                p.Start();
                System.Threading.SpinWait.SpinUntil(() => p.HasExited || p.MainWindowHandle != IntPtr.Zero);
                windowHandle = p.MainWindowHandle;
                while (!p.HasExited)
                {
                    p.Refresh();
                    if (p.MainWindowTitle != GetCurrentWindowTitle())
                        SetWindowText(p.MainWindowHandle, window_title);
                    Thread.Sleep(100);
                }
                p.WaitForExit();
            }
        }

        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr hwnd);

        static IntPtr WinGetHandle(string wName)
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
        static string GetCurrentWindowTitle()
        {
            return String.Format("{0}{1}", windowTitlePrefix, windowTitleNum);
        }

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if (windowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(windowHandle);
            }
        }

        static Keys GetHotkey()
        {
            string hotkey = System.Configuration.ConfigurationManager.AppSettings["hotkey" + windowTitleNum.ToString()];
            if (hotkey != null)
                return (Keys)Enum.Parse(typeof(Keys), hotkey);
            else
                return Keys.None;
        }

        static KeyModifiers GetKeyMod()
        {
            string modifier = System.Configuration.ConfigurationManager.AppSettings["keyModifier"];
            if (modifier != null)
                return (KeyModifiers)Enum.Parse(typeof(KeyModifiers), modifier);
            else
                return KeyModifiers.NoRepeat;
        }
    }
}
