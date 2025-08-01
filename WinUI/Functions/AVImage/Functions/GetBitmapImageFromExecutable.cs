﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get icon from executable file
        private static async Task<BitmapImage> GetBitmapImageFromExecutable(string exeFilePath, int iconIndex, int imageWidth, int imageHeight)
        {
            IntPtr iconHandle = IntPtr.Zero;
            IntPtr libraryHandle = IntPtr.Zero;
            try
            {
                //Load executable file library
                Debug.WriteLine("Loading exe icon: " + exeFilePath);
                libraryHandle = LoadLibraryEx(exeFilePath, IntPtr.Zero, LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
                if (libraryHandle == IntPtr.Zero)
                {
                    Debug.WriteLine("Failed to load icon from exe: " + exeFilePath);
                    return null;
                }

                //Enumerate all icon groups
                List<string> iconGroups = new List<string>();
                EnumResNameProcDelegate EnumResNameProcDelegate = (IntPtr hModule, ResourceTypes lpType, IntPtr lpEnumFunc, IntPtr lParam) =>
                {
                    try
                    {
                        string intPtrString = IntPtrToString(lpEnumFunc);
                        if (!string.IsNullOrWhiteSpace(intPtrString))
                        {
                            iconGroups.Add(intPtrString);
                            return true;
                        }
                    }
                    catch { }
                    return false;
                };
                EnumResourceNames(libraryHandle, ResourceTypes.GROUP_ICON, EnumResNameProcDelegate, IntPtr.Zero);

                //Select target icon group
                string iconGroup = string.Empty;
                int iconGroupsCount = iconGroups.Count;
                //Debug.WriteLine("Total icon groups: " + iconGroupsCount);
                if (iconGroupsCount > 0 && iconGroupsCount >= iconIndex)
                {
                    iconGroup = iconGroups[iconIndex];
                }
                else
                {
                    Debug.WriteLine("No exe icon found to load.");
                    return null;
                }

                //Get all icons from group
                List<MEMICONDIRENTRY> iconDirEntryList = new List<MEMICONDIRENTRY>();
                IntPtr iconDirIntPtr = GetResourceDataIntPtrFromString(libraryHandle, iconGroup, ResourceTypes.GROUP_ICON);
                unsafe
                {
                    MEMICONDIR* iconDir = (MEMICONDIR*)iconDirIntPtr;
                    MEMICONDIRENTRY* iconDirEntryArray = &iconDir->idEntriesArray;
                    //Debug.WriteLine("Total icons in group: " + iconDir->idCount);
                    for (int entryId = 0; entryId < iconDir->idCount; entryId++)
                    {
                        try
                        {
                            iconDirEntryList.Add(iconDirEntryArray[entryId]);
                        }
                        catch { }
                    }
                }

                //Select largest icon
                MEMICONDIRENTRY iconDirEntry = iconDirEntryList.OrderByDescending(x => x.wBitCount).ThenByDescending(x => x.dwBytesInRes).FirstOrDefault();

                //Get icon bitmap data
                byte[] iconBytes = GetResourceDataBytesFromIntPtr(libraryHandle, (IntPtr)iconDirEntry.nIdentifier, ResourceTypes.ICON);

                //Encode icon bitmap frame
                if (iconBytes[0] == 0x28)
                {
                    //Debug.WriteLine("BMP image: " + iconBytes.Length);

                    //Create icon from the resource
                    iconHandle = CreateIconFromResourceEx(iconBytes, (uint)iconBytes.Length, true, IconVersion.Windows3x, iconDirEntry.bWidth, iconDirEntry.bHeight, IconResourceFlags.LR_DEFAULTCOLOR);

                    //Convert to bitmap
                    using (Bitmap bitmap = Icon.FromHandle(iconHandle).ToBitmap())
                    {

                        //Convert to bitmap image
                        return await BitmapToBitmapImage(bitmap, imageWidth, imageHeight);
                    }
                }
                else
                {
                    //Debug.WriteLine("PNG image: " + iconBytes.Length);
                    using (MemoryStream memoryStreamIcon = new MemoryStream(iconBytes))
                    {
                        //Convert image data to bitmap
                        using (Bitmap bitmap = new Bitmap(memoryStreamIcon))
                        {
                            //Convert to bitmap image
                            return await BitmapToBitmapImage(bitmap, imageWidth, imageHeight);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from executable: " + ex.Message);
                return null;
            }
            finally
            {
                SafeCloseLibrary(ref libraryHandle);
                SafeCloseIcon(ref iconHandle);
            }
        }

        private static string IntPtrToString(IntPtr intPtr)
        {
            try
            {
                if (intPtr.ToInt64() > ushort.MaxValue)
                {
                    return Marshal.PtrToStringAnsi(intPtr);
                }
                else
                {
                    return intPtr.ToString();
                }
            }
            catch { }
            return string.Empty;
        }

        private static IntPtr FindResourceString(IntPtr hModule, string lpName, ResourceTypes lpType)
        {
            try
            {
                if (int.TryParse(lpName, out int intResult))
                {
                    return FindResource(hModule, (IntPtr)intResult, lpType);
                }
                else
                {
                    return FindResource(hModule, lpName, lpType);
                }
            }
            catch { }
            return IntPtr.Zero;
        }

        private static byte[] GetResourceDataBytesFromIntPtr(IntPtr hModule, IntPtr lpName, ResourceTypes lpType)
        {
            try
            {
                IntPtr foundResource = FindResource(hModule, lpName, lpType);
                if (foundResource == IntPtr.Zero)
                {
                    return null;
                }

                uint sizeResource = SizeofResource(hModule, foundResource);
                if (sizeResource == 0)
                {
                    return null;
                }

                IntPtr loadResource = LoadResource(hModule, foundResource);
                if (loadResource == IntPtr.Zero)
                {
                    return null;
                }

                IntPtr lockResource = LockResource(loadResource);
                if (lockResource == IntPtr.Zero)
                {
                    return null;
                }

                byte[] bytesResource = new byte[sizeResource];
                Marshal.Copy(lockResource, bytesResource, 0, bytesResource.Length);
                return bytesResource;
            }
            catch { }
            return null;
        }

        private static IntPtr GetResourceDataIntPtrFromString(IntPtr hModule, string lpName, ResourceTypes lpType)
        {
            try
            {
                IntPtr foundResource = FindResourceString(hModule, lpName, lpType);
                if (foundResource == IntPtr.Zero)
                {
                    return IntPtr.Zero;
                }

                uint sizeResource = SizeofResource(hModule, foundResource);
                if (sizeResource == 0)
                {
                    return IntPtr.Zero;
                }

                IntPtr loadResource = LoadResource(hModule, foundResource);
                if (loadResource == IntPtr.Zero)
                {
                    return IntPtr.Zero;
                }

                return LockResource(loadResource);
            }
            catch { }
            return IntPtr.Zero;
        }
    }
}