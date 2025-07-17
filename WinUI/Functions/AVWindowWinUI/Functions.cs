using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using Windows.Graphics.Display;

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

        private POINT GetWindowLocationCoordinates()
        {
            try
            {
                //FixStyle multi monitor bounds
                DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
                if (_windowDetails.Location == AVWindowLocation.TopLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = 0;
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.TopCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = 0;
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.TopRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = 0;
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new POINT() { X = horizontalLeft, Y = verticalTop };
                }
            }
            catch { }
            return new POINT() { X = 100, Y = 100 };
        }
    }
}