using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Magic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;


namespace WorkWork.Bot
{

    class KeyboardSim
    {
        public KeyboardSim()
        {
            handle = FindWindow(null, "World of Warcraft");
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, int wParam, int lParam);



        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_MOUSEMOVE = 0x0200;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;

        private IntPtr handle;

        public void KeyDown(int key)
        {
            

            SendMessage(handle, WM_KEYDOWN, key, 0);
        }
        public void KeyUp(int key)
        {

            SendMessage(handle, WM_KEYUP, key, 0);
        }
        public void MouseButtonDown()
        {

            SendMessage(handle, WM_RBUTTONDOWN, (int)Keys.RButton, 0);
        }
        public void MouseButtonUp()
        {

            SendMessage(handle, WM_RBUTTONUP, (int)Keys.RButton, 0);
        }
        public void SendToChat(string value, int channel)
        {
            IntPtr currentWindow = GetForegroundWindow();
            SetForegroundWindow(handle);
            SendMessage(handle, WM_KEYDOWN, (int)Keys.Enter, 0);
            SendMessage(handle, WM_KEYUP, (int)Keys.Enter, 0);
            Thread.Sleep(100);
            SendKeys.SendWait("{/}");
            if (channel == 0)
            {
                value = "s " + value;
            }
            else if (channel == 1)
            {
                value = "p " + value;
            }
            else if (channel == 2)
            {
                value = "g " + value;
            }
            else if (channel == 3)
            {
                value = "w " + value;
            }
            else if (channel == 4)
            {
                value = "r " + value;
            }
            SendKeys.SendWait(value);
            Thread.Sleep(500);
            SendMessage(handle, WM_KEYDOWN, (int)Keys.Enter, 0);
            SendMessage(handle, WM_KEYUP, (int)Keys.Enter, 0);
            SetForegroundWindow(currentWindow);
            
        }
    }
}
