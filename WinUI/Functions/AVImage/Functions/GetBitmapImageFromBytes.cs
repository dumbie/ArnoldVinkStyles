﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from Bytes
        private static async Task<BitmapImage> GetBitmapImageFromBytes(byte[] sourceBytes, int imageWidth, int imageHeight)
        {
            try
            {
                return await BytesToBitmapImage(sourceBytes, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from bytes: " + ex.Message);
                return null;
            }
        }
    }
}