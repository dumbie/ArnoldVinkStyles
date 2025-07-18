using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Interfaces
        [GeneratedComInterface, Guid("AF86E2E0-B12D-4C6A-9C5A-D7AA65101E90")]
        public partial interface IInspectable
        {
            int GetIids(out ulong iidCount, out IntPtr iids);
            int GetRuntimeClassName(out IntPtr className);
            int GetTrustLevel(out IntPtr trustLevel);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("E3DCD8C7-3057-4692-99C3-7B7720AFDA31")]
        public interface IDesktopWindowXamlSourceNative2
        {
            int AttachToWindow(IntPtr parenthWnd);
            IntPtr GetWindowHandle();
            int PreTranslateMessage(ref MSG message, out bool result);
        }

        [GeneratedComInterface, Guid("06636C29-5A17-458D-8EA2-2422D997A922")]
        public partial interface IXamlSourceTransparency : IInspectable
        {
            [return: MarshalAs(UnmanagedType.Bool)] bool IsBackgroundTransparent();
            void IsBackgroundTransparent([MarshalAs(UnmanagedType.Bool)] bool value);
        }
    }
}