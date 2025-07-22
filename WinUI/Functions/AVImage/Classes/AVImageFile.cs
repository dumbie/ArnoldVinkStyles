using System;
using Windows.UI.Core;
using static ArnoldVinkCode.AVSearch;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        public class AVImageFile
        {
            public CoreDispatcher Dispatcher { get; set; } = null;
            public string[] FilePaths { get; set; } = [];
            public string BackupPath { get; set; } = string.Empty;
            public SearchSource[] SearchPaths { get; set; } = [];
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
            public int IconIndex { get; set; } = 0;
            public bool UseThumbnail { get; set; } = false;
        }
    }
}