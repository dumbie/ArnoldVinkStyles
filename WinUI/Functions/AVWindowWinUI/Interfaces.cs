using System;
using System.Runtime.InteropServices;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Interfaces
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("E3DCD8C7-3057-4692-99C3-7B7720AFDA31")]
        public interface IDesktopWindowXamlSourceNative2
        {
            int AttachToWindow(IntPtr parenthWnd);
            IntPtr GetWindowHandle();
            bool PreTranslateMessage(ref MSG message);
        }
    }
}