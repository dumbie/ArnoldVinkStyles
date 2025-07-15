using System;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Classes
        public class AVWindowDetails()
        {
            public Type Type { get; set; }
            public string Title { get; set; } = "Unknown";
            public int Width { get; set; } = 600;
            public int MinWidth { get; set; } = 275;
            public int MaxWidth { get; set; } = 0;
            public int Height { get; set; } = 600;
            public int MinHeight { get; set; } = 250;
            public int MaxHeight { get; set; } = 0;
            public bool DisableCloseButton { get; set; } = false;
            public bool NoBorder { get; set; } = false;

            //Fix MinWidth / MaxWidth / TopMost / Style / State / AllowsTransparency / ResizeMode / ShowInTaskbar / Icon / StartLocation / 
        }
    }
}