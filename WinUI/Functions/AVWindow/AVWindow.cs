using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using WinRT;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVWindow
    {
        /// <summary>
        /// Create application window using provided details
        /// </summary>
        public AVWindow(AVWindowDetails windowDetails)
        {
            try
            {
                //Set window details variable
                _windowDetails = windowDetails;

                //Create window in new thread
                Thread windowThread = new Thread(delegate ()
                {
                    CreateWindow();
                });
                windowThread.SetApartmentState(ApartmentState.STA);
                windowThread.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AVWindow failed: " + ex.Message);
            }
        }

        //Create window
        private bool CreateWindow()
        {
            try
            {
                Debug.WriteLine("Creating application window: " + _windowDetails.Title);

                //Create core window
                Guid coreWindowGuid = new Guid("79B9D5F2-879E-4B89-B798-79E47598030C");
                PrivateCreateCoreWindow(CORE_WINDOW_TYPE.NOT_IMMERSIVE, _windowDetails.Title, 0, 0, 0, 0, 0, IntPtr.Zero, coreWindowGuid, out IntPtr coreWindowPointer);
                CoreWindow coreWindow = CoreWindow.FromAbi(coreWindowPointer);

                //Create core application view
                CoreApplication.As<ICoreApplicationPrivate2>().CreateNonImmersiveView(out IntPtr coreApplicationViewPointer);
                CoreApplicationView coreApplicationView = CoreApplicationView.FromAbi(coreApplicationViewPointer);

                //Create framework view
                FrameworkView frameworkView = new FrameworkView();
                frameworkView.Initialize(coreApplicationView);
                frameworkView.SetWindow(coreWindow);

                //Get core window handle
                ICoreWindowInterop coreWindowInterop = coreWindow.As<ICoreWindowInterop>();
                coreWindowInterop.GetWindowHandle(out _coreWindowHandle);

                //Set synchronization context
                SynchronizationContext.SetSynchronizationContext(new DispatcherQueueSynchronizationContext(coreWindow.DispatcherQueue));
                SynchronizationContext.SetSynchronizationContext(new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread()));

                //Set custom window procedure
                IntPtr windowProcPointer = Marshal.GetFunctionPointerForDelegate((WindowProcedureDelegate)WindowProcedure);
                _coreWindowProcedure = SetWindowLongAuto(_coreWindowHandle, WindowLongFlags.GWL_WNDPROC, windowProcPointer);

                //Convert type to framework element
                FrameworkElement frameworkElement = (FrameworkElement)Activator.CreateInstance(_windowDetails.Type);

                //Set framework element as content
                Window.Current.Content = frameworkElement;

                //Prepare window styles
                WindowStyles windowStyle = WindowStyles.WS_NONE;
                WindowStylesEx windowStyleEx = WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP;
                if (_windowDetails.NoBorder)
                {
                    windowStyle |= WindowStyles.WS_POPUPWINDOW;
                }
                else
                {
                    windowStyle |= WindowStyles.WS_OVERLAPPEDWINDOW;
                }
                if (_windowDetails.NoMaximizeButton)
                {
                    windowStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
                }
                if (_windowDetails.NoMinimizeButton)
                {
                    windowStyle &= ~WindowStyles.WS_MINIMIZEBOX;
                }
                if (_windowDetails.NoResize)
                {
                    windowStyle &= ~WindowStyles.WS_THICKFRAME;
                }
                if (_windowDetails.NoActivation)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_NOACTIVATE;
                }
                if (_windowDetails.NoSwitch)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_TOOLWINDOW;
                }
                if (_windowDetails.TopMost)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_TOPMOST;
                }

                //Allow window drag and drop files
                if (_windowDetails.AllowDragDropFiles)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_ACCEPTFILES;
                    ChangeWindowMessageFilterEx(_coreWindowHandle, WindowMessages.WM_DROPFILES, ChangeWindowMessageFilterAction.MSGFLT_ALLOW, IntPtr.Zero);
                    ChangeWindowMessageFilterEx(_coreWindowHandle, WindowMessages.WM_COPYGLOBALDATA, ChangeWindowMessageFilterAction.MSGFLT_ALLOW, IntPtr.Zero);
                }

                //Set window styles
                SetWindowLongAuto(_coreWindowHandle, WindowLongFlags.GWL_STYLE, new IntPtr((uint)windowStyle));
                SetWindowLongAuto(_coreWindowHandle, WindowLongFlags.GWL_EXSTYLE, new IntPtr((uint)windowStyleEx));

                //Set window icon
                IntPtr windowIcon = LoadIcon(_windowDetails.IconPath);
                if (windowIcon != IntPtr.Zero)
                {
                    SetClassLongAuto(_coreWindowHandle, ClassLongFlags.GCL_HICON, windowIcon);
                    SetClassLongAuto(_coreWindowHandle, ClassLongFlags.GCL_HICONSM, windowIcon);
                }

                //Check minimum and maximum window size
                if (_windowDetails.MinWidth != 0 && _windowDetails.Width < _windowDetails.MinWidth) { _windowDetails.Width = _windowDetails.MinWidth; }
                if (_windowDetails.MaxWidth != 0 && _windowDetails.Width > _windowDetails.MaxWidth) { _windowDetails.Width = _windowDetails.MaxWidth; }
                if (_windowDetails.MinHeight != 0 && _windowDetails.Height < _windowDetails.MinHeight) { _windowDetails.Height = _windowDetails.MinHeight; }
                if (_windowDetails.MaxHeight != 0 && _windowDetails.Height > _windowDetails.MaxHeight) { _windowDetails.Height = _windowDetails.MaxHeight; }

                //Get window location coordinates
                WindowPoint windowLocation = GetWindowTargetLocation();

                //Set window size and location
                MoveWindow(_coreWindowHandle, windowLocation.X, windowLocation.Y, _windowDetails.Width, _windowDetails.Height, true);

                //Show window in set state
                if (_windowDetails.State == AVWindowState.Normal)
                {
                    ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWNORMAL);
                }
                else if (_windowDetails.State == AVWindowState.Minimized)
                {
                    ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWMINIMIZED);
                }
                else if (_windowDetails.State == AVWindowState.Maximized)
                {
                    ShowWindow(_coreWindowHandle, ShowWindowFlags.SW_SHOWMAXIMIZED);
                }

                //Run framework view
                frameworkView.Run();

                //Return result
                Debug.WriteLine("Closed application window: " + _windowDetails.Title);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateWindow failed: " + ex.Message);
                return false;
            }
        }
    }
}