using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Procedure
        private IntPtr WindowProcedure(IntPtr hWnd, IntPtr message, IntPtr wParam, IntPtr lParam)
        {
            bool messageHandled = false;
            try
            {
                switch (message)
                {
                    case (int)WindowMessages.WM_CLOSE:
                        Handle_CloseRequest(ref messageHandled);
                        break;
                    case (int)WindowMessages.WM_GETMINMAXINFO:
                        Handle_WindowMinMaxInfo(lParam);
                        break;
                    case (int)WindowMessages.WM_DROPFILES:
                        Handle_DropFiles(wParam, lParam);
                        break;
                }
                Handle_ForwardMessage(hWnd, message, wParam, lParam, ref messageHandled);
            }
            catch { }
            if (messageHandled)
            {
                return IntPtr.Zero;
            }
            else
            {
                return CallWindowProc(_coreWindowProcedure, hWnd, message, wParam, lParam);
            }
        }

        private void Handle_ForwardMessage(IntPtr hWnd, IntPtr message, IntPtr wParam, IntPtr lParam, ref bool messageHandled)
        {
            try
            {
                if (forwardMessage != null)
                {
                    WindowMessage windowMessage = new WindowMessage()
                    {
                        hWnd = hWnd,
                        message = message,
                        wParam = wParam,
                        lParam = lParam
                    };
                    forwardMessage(ref windowMessage, ref messageHandled);
                }
            }
            catch { }
        }

        private void Handle_CloseRequest(ref bool messageHandled)
        {
            try
            {
                if (closeRequested != null)
                {
                    closeRequested();
                    messageHandled = true;
                }
            }
            catch { }
        }

        private void Handle_WindowMinMaxInfo(IntPtr lParam)
        {
            try
            {
                WindowMinMaxInfo minMaxInfo = (WindowMinMaxInfo)Marshal.PtrToStructure(lParam, typeof(WindowMinMaxInfo));
                if (_windowDetails.MinWidth != 0) { minMaxInfo.ptMinTrackSize.X = _windowDetails.MinWidth; }
                if (_windowDetails.MinHeight != 0) { minMaxInfo.ptMinTrackSize.Y = _windowDetails.MinHeight; }
                if (_windowDetails.MaxWidth != 0) { minMaxInfo.ptMaxTrackSize.X = _windowDetails.MaxWidth; }
                if (_windowDetails.MaxHeight != 0) { minMaxInfo.ptMaxTrackSize.Y = _windowDetails.MaxHeight; }
                Marshal.StructureToPtr(minMaxInfo, lParam, true);
            }
            catch { }
        }

        private void Handle_DropFiles(IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (filesDropped != null)
                {
                    //Get file count
                    int fileNameCount = DragQueryFile(wParam, 0xFFFFFFFF, null, 0);
                    if (fileNameCount > 0)
                    {
                        //Get file names
                        List<string> fileNameList = new List<string>();
                        for (uint index = 0; index < fileNameCount; index++)
                        {
                            //Get file size
                            int fileNameSize = DragQueryFile(wParam, index, null, 0) + 1;
                            if (fileNameSize > 1)
                            {
                                StringBuilder stringBuilder = new StringBuilder(fileNameSize);
                                DragQueryFile(wParam, index, stringBuilder, fileNameSize);
                                fileNameList.Add(stringBuilder.ToString());
                            }
                        }

                        //Finish drag
                        DragFinish(wParam);

                        //Return file names
                        filesDropped(fileNameList);
                    }
                }
            }
            catch { }
        }
    }
}