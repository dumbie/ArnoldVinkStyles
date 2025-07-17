using System;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Classes
        public class AVWindowDetails()
        {
            /// <summary>
            /// FrameworkElement type that gets set as content
            /// </summary>
            public Type Type { get; set; }
            /// <summary>
            /// Set window title
            /// </summary>
            public string Title { get; set; } = "Unknown";
            /// <summary>
            /// Path to window icon file or assembly resource
            /// </summary>
            public string IconPath { get; set; } = string.Empty;
            /// <summary>
            /// Window start location
            /// </summary>
            public AVWindowLocation Location { get; set; } = AVWindowLocation.MiddleCenter;
            /// <summary>
            /// Set window width
            /// </summary>
            public int Width { get; set; } = 600;
            /// <summary>
            /// Set window minimum width
            /// </summary>
            public int MinWidth { get; set; } = 275;
            /// <summary>
            /// Set window maximum width
            /// </summary>
            public int MaxWidth { get; set; } = 0;
            /// <summary>
            /// Set window height
            /// </summary>
            public int Height { get; set; } = 600;
            /// <summary>
            /// Set window minimum height
            /// </summary>
            public int MinHeight { get; set; } = 250;
            /// <summary>
            /// Set window maximum height
            /// </summary>
            public int MaxHeight { get; set; } = 0;
            /// <summary>
            /// Disable window close button
            /// </summary>
            public bool NoCloseButton { get; set; } = false;
            /// <summary>
            /// Disable window maximize button
            /// </summary>
            public bool NoMaximizeButton { get; set; } = false;
            /// <summary>
            /// Disable window minimize button
            /// </summary>
            public bool NoMinimizeButton { get; set; } = false;
            /// <summary>
            /// Show window without any border only the content
            /// </summary>
            public bool NoBorder { get; set; } = false;
            /// <summary>
            /// Prevent window from getting resized
            /// </summary>
            public bool NoResize { get; set; } = false;
            /// <summary>
            /// Prevent window from focus and activation
            /// </summary>
            public bool NoActivation { get; set; } = false;
            /// <summary>
            /// Prevent window from showing up on Alt+Tab and taskbar
            /// </summary>
            public bool NoSwitch { get; set; } = false;
            /// <summary>
            /// Show window above all other non topmost windows
            /// </summary>
            public bool TopMost { get; set; } = false;
            /// <summary>
            /// Allows window background to be transparent
            /// </summary>
            public bool BackgroundTransparency { get; set; } = false;
        }
    }
}