using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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
                            closeRequested();
                            return IntPtr.Zero;
                        }
                        break;
                    case (int)WindowMessages.WM_GETMINMAXINFO:
                        Handle_WindowMinMaxInfo(lParam);
                        break;
                    case (int)WindowMessages.WM_DROPFILES:
                        Handle_DropFiles(wParam, lParam);
                        break;
                }
            }
            catch { }
            return CallWindowProcW(_coreWindowProcedure, hWnd, messageCode, wParam, lParam);
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