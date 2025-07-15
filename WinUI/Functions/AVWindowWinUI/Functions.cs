using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Functions
        private IntPtr LoadIcon(string iconPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iconPath)) { return IntPtr.Zero; }
                if (iconPath.Contains(":"))
                {
                    //Load icon from file
                    Icon iconSource = new Icon(iconPath);
                    return iconSource.Handle;
                }
                else
                {
                    //Load icon from resource
                    Assembly assembly = Assembly.GetEntryAssembly();
                    using (Stream iconStream = assembly.GetManifestResourceStream(iconPath))
                    {
                        Icon iconSource = new Icon(iconStream);
                        return iconSource.Handle;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed loading window icon: " + ex.Message);
                return IntPtr.Zero;
            }
        }
    }
}