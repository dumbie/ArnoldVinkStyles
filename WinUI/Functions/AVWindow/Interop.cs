using System;
using System.Runtime.InteropServices;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Interop
        [DllImport("Windows.UI.dll", EntryPoint = "#1500", CharSet = CharSet.Unicode)]
        public static extern int PrivateCreateCoreWindow(CORE_WINDOW_TYPE WindowType, string pWindowTitle, int X, int Y, int uWidth, int uHeight, IntPtr dwAttributes, IntPtr hOwnerWindow, Guid riid, out IntPtr ppv);
    }
}