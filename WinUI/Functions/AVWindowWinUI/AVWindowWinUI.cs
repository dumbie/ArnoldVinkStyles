using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
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
                Thread windowThread = new Thread(delegate ()
                {
                    CreateWindow(windowDetails);
                });
                windowThread.SetApartmentState(ApartmentState.STA);
                windowThread.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Initialize failed: " + ex.Message);
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

        public bool CreateWindow(AVWindowDetails windowDetails)
        {
            try
            {
                Debug.WriteLine("Creating application window: " + windowDetails.Title);

                //Set window details variable
                _windowDetails = windowDetails;

                //Initialize XAML manager for current thread
                WindowsXamlManager.InitializeForCurrentThread();

                //Convert type to framework element
                FrameworkElement frameworkElement = (FrameworkElement)Activator.CreateInstance(windowDetails.Type);

                //Get current process handle
                IntPtr processHandle = Process.GetCurrentProcess().Handle;

                //Set window strings
                string szWindowTitle = windowDetails.Title;
                string szWindowClass = "AVWindow" + Environment.TickCount64.ToString();

                //Load window icon
                IntPtr windowIcon = LoadIcon(windowDetails.IconPath);

                //Prepare window class
                WindowClassEx windowClassEx = new WindowClassEx();
                windowClassEx.cbSize = (uint)Marshal.SizeOf(typeof(WindowClassEx));
                windowClassEx.style = ClassStyles.CS_HREDRAW | ClassStyles.CS_VREDRAW;
                windowClassEx.lpfnWndProc = WindowProc;
                windowClassEx.hInstance = processHandle;
                windowClassEx.hIcon = windowIcon;
                windowClassEx.lpszClassName = szWindowClass;
                if (windowDetails.NoCloseButton)
                {
                    //Fix does not block tray close button
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
                WindowStyles windowStyle = WindowStyles.WS_VISIBLE;
                WindowStylesEx windowStyleEx = WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP;
                if (windowDetails.NoBorder)
                {
                    windowStyle |= WindowStyles.WS_POPUPWINDOW;
                }
                else
                {
                    windowStyle |= WindowStyles.WS_OVERLAPPEDWINDOW;
                }
                if (windowDetails.NoActivation)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_NOACTIVATE;
                }
                if (windowDetails.NoSwitch)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_TOOLWINDOW;
                }
                if (windowDetails.TopMost)
                {
                    windowStyleEx |= WindowStylesEx.WS_EX_TOPMOST;
                }

                //Create main window
                _WindowHandleMain = CreateWindowEx(windowStyleEx, szWindowClass, szWindowTitle, windowStyle, CW_USEDEFAULT, CW_USEDEFAULT, windowDetails.Width, windowDetails.Height, IntPtr.Zero, IntPtr.Zero, processHandle, IntPtr.Zero);
                if (_WindowHandleMain == IntPtr.Zero)
                {
                    Debug.WriteLine("CreateWindowEx failed.");
                    return false;
                }

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

                //Update DesktopWindowXamlSource window size
                GetClientRect(_WindowHandleMain, out RECT rectClient);
                SetWindowPos(_WindowHandleXaml, IntPtr.Zero, 0, 0, rectClient.Right, rectClient.Bottom, SetWindowPositions.SWP_SHOWWINDOW);

                //Set DesktopWindowXamlSource content
                _desktopWindowXamlSource.Content = frameworkElement;

                //Allow background transparency
                if (windowDetails.BackgroundTransparency)
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
                Debug.WriteLine("Closed application window: " + windowDetails.Title);
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