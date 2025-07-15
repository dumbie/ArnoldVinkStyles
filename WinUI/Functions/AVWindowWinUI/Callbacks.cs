using System;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Callbacks
        private IntPtr WindowProc(IntPtr hWnd, IntPtr messageCode, IntPtr wParam, IntPtr lParam)
        {
            switch (messageCode)
            {
                case (int)WindowMessages.WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                case (int)WindowMessages.WM_SIZE:
                    GetClientRect(hWnd, out RECT rectClient);
                    MoveWindow(_hWnd_XamlWindow, 0, 0, rectClient.Right, rectClient.Bottom, true);
                    break;
                default:
                    break;
            }
            return DefWindowProcW(hWnd, messageCode, wParam, lParam);
        }
    }
}