using System;
using System.Runtime.InteropServices;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Callbacks
        private IntPtr WindowProc(IntPtr hWnd, IntPtr messageCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                switch (messageCode)
                {
                    case (int)WindowMessages.WM_CLOSE:
                        DestroyWindow(hWnd);
                        break;
                    case (int)WindowMessages.WM_DESTROY:
                        PostQuitMessage(0);
                        break;
                    case (int)WindowMessages.WM_SIZE:
                    case (int)WindowMessages.WM_DPICHANGED:
                        GetClientRect(hWnd, out RECT rectClient);
                        MoveWindow(_WindowHandleXaml, 0, 0, rectClient.Right, rectClient.Bottom, true);
                        break;
                    case (int)WindowMessages.WM_GETMINMAXINFO:
                        MINMAXINFO minMaxInfo = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                        if (_windowDetails.MinWidth != 0) { minMaxInfo.ptMinTrackSize.X = _windowDetails.MinWidth; }
                        if (_windowDetails.MinHeight != 0) { minMaxInfo.ptMinTrackSize.Y = _windowDetails.MinHeight; }
                        if (_windowDetails.MaxWidth != 0) { minMaxInfo.ptMaxTrackSize.X = _windowDetails.MaxWidth; }
                        if (_windowDetails.MaxHeight != 0) { minMaxInfo.ptMaxTrackSize.Y = _windowDetails.MaxHeight; }
                        Marshal.StructureToPtr(minMaxInfo, lParam, true);
                        break;
                    default:
                        break;
                }
            }
            catch { }
            return DefWindowProcW(hWnd, messageCode, wParam, lParam);
        }
    }
}