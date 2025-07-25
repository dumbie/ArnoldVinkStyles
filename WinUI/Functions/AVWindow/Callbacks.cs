using System;
using System.Runtime.InteropServices;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Callbacks
        private IntPtr WindowProcedure(IntPtr hWnd, IntPtr messageCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                switch (messageCode)
                {
                    case (int)WindowMessages.WM_CLOSE:
                        if (closeRequested != null)
                        {
                            closeRequested(this, null);
                            return IntPtr.Zero;
                        }
                        break;
                    case (int)WindowMessages.WM_GETMINMAXINFO:
                        WindowMinMaxInfo minMaxInfo = (WindowMinMaxInfo)Marshal.PtrToStructure(lParam, typeof(WindowMinMaxInfo));
                        if (_windowDetails.MinWidth != 0) { minMaxInfo.ptMinTrackSize.X = _windowDetails.MinWidth; }
                        if (_windowDetails.MinHeight != 0) { minMaxInfo.ptMinTrackSize.Y = _windowDetails.MinHeight; }
                        if (_windowDetails.MaxWidth != 0) { minMaxInfo.ptMaxTrackSize.X = _windowDetails.MaxWidth; }
                        if (_windowDetails.MaxHeight != 0) { minMaxInfo.ptMaxTrackSize.Y = _windowDetails.MaxHeight; }
                        Marshal.StructureToPtr(minMaxInfo, lParam, true);
                        break;
                }
            }
            catch { }
            return CallWindowProcW(_coreWindowProcedure, hWnd, messageCode, wParam, lParam);
        }
    }
}