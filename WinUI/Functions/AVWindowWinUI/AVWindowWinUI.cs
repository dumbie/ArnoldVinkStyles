using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using WinRT;

namespace ArnoldVinkStyles
{
    public partial class AVWindowWinUI
    {
        //Variables
        private AVWindowDetails _windowDetails = null;
        private DesktopWindowXamlSource _desktopWindowXamlSource = null;
        private IntPtr _WindowHandleMain = IntPtr.Zero;
        private IntPtr _WindowHandleXaml = IntPtr.Zero;

        //Initialize
        public AVWindowWinUI(AVWindowDetails windowDetails)
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
                Debug.WriteLine("AVWindowWinUI failed: " + ex.Message);
            }
        }

        public IntPtr GetHandle()
        {
            return _WindowHandleMain;
        }

        public void SetContent(FrameworkElement content)
        {
            try
            {
                //Check DesktopWindowXamlSource
                if (_desktopWindowXamlSource == null)
                {
                    return;
                }

                //Set DesktopWindowXamlSource content
                _desktopWindowXamlSource.Content = content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetContent failed: " + ex.Message);
            }
        }

        private bool CreateWindow()
        {
            try
            {
                Debug.WriteLine("Creating application window: " + _windowDetails.Title);

                //Get current process handle
                IntPtr processHandle = Process.GetCurrentProcess().Handle;

                //Set window strings
                string szWindowTitle = _windowDetails.Title;
                string szWindowClass = "AVWindow" + new Random().NextInt64();

                //Load window icon
                IntPtr windowIcon = LoadIcon(_windowDetails.IconPath);

                //Prepare window class
                WindowClassEx windowClassEx = new WindowClassEx();
                windowClassEx.cbSize = (uint)Marshal.SizeOf(typeof(WindowClassEx));
                windowClassEx.style = ClassStyles.CS_HREDRAW | ClassStyles.CS_VREDRAW;
                windowClassEx.lpfnWndProc = WindowProc;
                windowClassEx.hInstance = processHandle;
                windowClassEx.hIcon = windowIcon;
                windowClassEx.lpszClassName = szWindowClass;
                if (_windowDetails.NoCloseButton)
                {
                    //Fix does not disable tray close button
                    windowClassEx.style |= ClassStyles.CS_NOCLOSE;
                }

                //Register window class
                IntPtr regResult = RegisterClassEx(ref windowClassEx);
                if (regResult == IntPtr.Zero)
                {
                    Debug.WriteLine("RegisterClassEx failed.");
                    return false;
                }

                //Prepare window creation
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

                //Create main window
                _WindowHandleMain = CreateWindowEx(windowStyleEx, szWindowClass, szWindowTitle, windowStyle, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, processHandle, IntPtr.Zero);
                if (_WindowHandleMain == IntPtr.Zero)
                {
                    Debug.WriteLine("CreateWindowEx failed.");
                    return false;
                }

                //Initialize XAML manager for current thread
                WindowsXamlManager.InitializeForCurrentThread();

                //Set synchronization context
                SynchronizationContext.SetSynchronizationContext(new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread()));

                //Create desktop window xaml source
                _desktopWindowXamlSource = new DesktopWindowXamlSource();

                //Get native desktop window xaml source
                IDesktopWindowXamlSourceNative2 desktopWindowXamlSourceNative = _desktopWindowXamlSource.As<IDesktopWindowXamlSourceNative2>();

                //Attach DesktopWindowXamlSource to main window
                int attachResult = desktopWindowXamlSourceNative.AttachToWindow(_WindowHandleMain);
                if (attachResult < 0)
                {
                    Debug.WriteLine("AttachToWindow failed: " + attachResult);
                    return false;
                }

                //Get DesktopWindowXamlSource window handle
                _WindowHandleXaml = desktopWindowXamlSourceNative.GetWindowHandle();
                if (_WindowHandleXaml == IntPtr.Zero)
                {
                    Debug.WriteLine("GetWindowHandle failed.");
                    return false;
                }

                //Check minimum and maximum window size
                if (_windowDetails.MinWidth != 0 && _windowDetails.Width < _windowDetails.MinWidth) { _windowDetails.Width = _windowDetails.MinWidth; }
                if (_windowDetails.MaxWidth != 0 && _windowDetails.Width > _windowDetails.MaxWidth) { _windowDetails.Width = _windowDetails.MaxWidth; }
                if (_windowDetails.MinHeight != 0 && _windowDetails.Height < _windowDetails.MinHeight) { _windowDetails.Height = _windowDetails.MinHeight; }
                if (_windowDetails.MaxHeight != 0 && _windowDetails.Height > _windowDetails.MaxHeight) { _windowDetails.Height = _windowDetails.MaxHeight; }

                //Get window location coordinates
                POINT windowLocation = GetWindowLocationCoordinates();

                //Show window and update size and location
                MoveWindow(_WindowHandleMain, windowLocation.X, windowLocation.Y, _windowDetails.Width, _windowDetails.Height, true);
                ShowWindow(_WindowHandleXaml, ShowWindowCommands.SW_SHOWNORMAL);
                if (_windowDetails.State == AVWindowState.Normal)
                {
                    ShowWindow(_WindowHandleMain, ShowWindowCommands.SW_SHOWNORMAL);
                }
                else if (_windowDetails.State == AVWindowState.Minimized)
                {
                    ShowWindow(_WindowHandleMain, ShowWindowCommands.SW_SHOWMINIMIZED);
                }
                else if (_windowDetails.State == AVWindowState.Maximized)
                {
                    ShowWindow(_WindowHandleMain, ShowWindowCommands.SW_SHOWMAXIMIZED);
                }

                //Convert type to framework element
                FrameworkElement frameworkElement = (FrameworkElement)Activator.CreateInstance(_windowDetails.Type);

                //Set DesktopWindowXamlSource content
                _desktopWindowXamlSource.Content = frameworkElement;

                //Allow background transparency
                if (_windowDetails.BackgroundTransparency)
                {
                    IXamlSourceTransparency windowTransparency = Window.Current.As<IXamlSourceTransparency>();
                    windowTransparency.IsBackgroundTransparent(true);
                }

                //Window message loop
                MSG lpMsg = new MSG();
                while (GetMessageW(out lpMsg, IntPtr.Zero, 0, 0))
                {
                    TranslateMessage(ref lpMsg);
                    DispatchMessageW(ref lpMsg);
                }

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