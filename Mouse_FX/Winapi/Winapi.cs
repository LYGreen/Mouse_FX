using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_FX
{
    /// <summary>
    /// Windows API 函数
    /// </summary>
    internal class Winapi
    {
        /* Windows Messages */
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;

        /* Hook */
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD = 2;
        public const int WH_MOUSE = 7;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public Point pt;
            public IntPtr hwnd;
            public int wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCTEX
        {
            public Point pt;
            public int mouseData;
            public IntPtr hwnd;
            public int wHitTestCode;
            public IntPtr dwExtraInfo;
            
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        public delegate IntPtr HOOKPROC(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr SetWindowsHookExW(int idHook,HOOKPROC lpfn,IntPtr hmod,int dwThreadId);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /* SetWindowLong */
        public const int GWL_EXSTYLE = (-20);
        public const int WS_EX_TRANSPARENT = 32;

        [DllImport("user32.dll",SetLastError = true)]
        public static extern long SetWindowLongW(IntPtr hWnd, int nIndex, long dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetCursorPos(ref POINT lpPoint);
    }
}
