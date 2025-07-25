using System;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        //Variables
        private AVWindowDetails _windowDetails = null;
        private IntPtr _coreWindowHandle = IntPtr.Zero;
        private IntPtr _coreWindowProcedure = IntPtr.Zero;
    }
}