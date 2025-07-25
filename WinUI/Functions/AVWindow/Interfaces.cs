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

        [GeneratedComInterface, Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
        public partial interface ICoreWindowInterop
        {
            int GetWindowHandle(out IntPtr hWnd);
            int SetMessageHandled([MarshalAs(UnmanagedType.Bool)] bool value);
        }

        [GeneratedComInterface, Guid("6090202D-2843-4BA5-9B0D-FC88EECD9CE5")]
        public partial interface ICoreApplicationPrivate2 : IInspectable
        {
            int InitializeForAttach();
            int WaitForActivate(out IntPtr coreWindow);
            int CreateNonImmersiveView(out IntPtr coreApplicationView);
        }
    }
}