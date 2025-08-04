using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        /// <summary>
        /// Minimize window
        /// </summary>
        public void Minimize()
        {
            try
            {
                ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWMINIMIZED);
            }
            catch { }
        }

        /// <summary>
        /// Returns if window is currently minimized
        /// </summary>
        public bool IsMinimized
        {
            get
            {
                try
                {
                    WindowStyles windowStyle = (WindowStyles)GetWindowLongAuto(_coreWindowHandle, WindowLongFlags.GWL_STYLE).ToInt64();
                    return windowStyle.HasFlag(WindowStyles.WS_MINIMIZE);
                }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// Maximize window
        /// </summary>
        public void Maximize()
        {
            try
            {
                ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWMAXIMIZED);
            }
            catch { }
        }

        /// <summary>
        /// Returns if window is currently maximized
        /// </summary>
        public bool IsMaximized
        {
            get
            {
                try
                {
                    WindowStyles windowStyle = (WindowStyles)GetWindowLongAuto(_coreWindowHandle, WindowLongFlags.GWL_STYLE).ToInt64();
                    return windowStyle.HasFlag(WindowStyles.WS_MAXIMIZE);
                }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// Show window
        /// </summary>
        public void Show()
        {
            try
            {
                ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWNORMAL);
            }
            catch { }
        }

        /// <summary>
        /// Get current window handle
        /// </summary>
        public IntPtr GetHandle()
        {
            try
            {
                return _coreWindowHandle;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Set current Window content
        /// </summary>
        public void SetContent(FrameworkElement content)
        {
            try
            {
                Window.Current.Content = content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Set window content failed: " + ex.Message);
            }
        }

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

        /// <summary>
        /// Get current window location and size
        /// </summary>
        public WindowRectangle GetWindowLocationSize()
        {
            try
            {
                GetWindowRect(_coreWindowHandle, out WindowRectangle windowRectangle);
                return windowRectangle;
            }
            catch
            {
                return new WindowRectangle();
            }
        }

        //Get target window location
        private WindowPoint GetWindowTargetLocation()
        {
            try
            {
                //FixStyle multi monitor bounds
                DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
                if (_windowDetails.Location == AVWindowLocation.TopLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = 0;
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.TopCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = 0;
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.TopRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = 0;
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.BottomRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = (int)(displayInformation.ScreenHeightInRawPixels - _windowDetails.Height);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleLeft)
                {
                    int horizontalLeft = 0;
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleCenter)
                {
                    int horizontalLeft = (int)((displayInformation.ScreenWidthInRawPixels - _windowDetails.Width) / 2);
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
                else if (_windowDetails.Location == AVWindowLocation.MiddleRight)
                {
                    int horizontalLeft = (int)(displayInformation.ScreenWidthInRawPixels - _windowDetails.Width);
                    int verticalTop = (int)((displayInformation.ScreenHeightInRawPixels - _windowDetails.Height) / 2);
                    return new WindowPoint() { X = horizontalLeft, Y = verticalTop };
                }
            }
            catch { }
            return new WindowPoint() { X = 100, Y = 100 };
        }
    }
}