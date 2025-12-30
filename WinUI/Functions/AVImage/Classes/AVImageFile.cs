using System;
using System.Drawing;
using Windows.UI.Core;
using static ArnoldVinkCode.AVSearch;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        public class AVImageFile
        {
            public CoreDispatcher Dispatcher { get; set; } = null;
            public Uri ImageUri { get; set; } = null;
            public Bitmap ImageBitmap { get; set; } = null;
            public byte[] ImageBytes { get; set; } = null;
            public string FileType { get; set; } = string.Empty;
            public string[] FilePaths { get; set; } = null;
            public SearchSource[] SearchPaths { get; set; } = null;
            public string BackupPath { get; set; } = string.Empty;
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
            public int IconIndex { get; set; } = 0;
            public bool UseThumbnail { get; set; } = false;
        }
    }
}