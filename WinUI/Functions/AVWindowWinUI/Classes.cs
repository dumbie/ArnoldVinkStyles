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
            public string Title { get; set; } = "Unknown";
            public string IconPath { get; set; } = string.Empty;
            public int Width { get; set; } = 600;
            public int MinWidth { get; set; } = 275;
            public int MaxWidth { get; set; } = 0;
            public int Height { get; set; } = 600;
            public int MinHeight { get; set; } = 250;
            public int MaxHeight { get; set; } = 0;
            /// <summary>
            /// Disable window close button
            /// </summary>
            public bool NoCloseButton { get; set; } = false;
            /// <summary>
            /// Show window without any border only the content
            /// </summary>
            public bool NoBorder { get; set; } = false;
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

            //Fix Style / State / AllowsTransparency / ResizeMode / StartLocation
        }
    }
}