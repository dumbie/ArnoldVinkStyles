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
        private IntPtr _hWnd_MainWindow = IntPtr.Zero;
        private IntPtr _hWnd_XamlWindow = IntPtr.Zero;
        private DesktopWindowXamlSource _desktopWindowXamlSource = null;

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
                //Assembly thisAssembly = Assembly.GetExecutingAssembly();
                //string appName = thisAssembly.GetName().Name;
                //Stream iconStream = thisAssembly.GetManifestResourceStream(appName + ".Resources.AppIcon.ico");
                //Icon iconSource = new Icon(iconStream);

                //Prepare window class
                WindowClassEx windowClassEx = new WindowClassEx();
                windowClassEx.cbSize = (uint)Marshal.SizeOf(typeof(WindowClassEx));
                windowClassEx.style = ClassStyles.CS_HREDRAW | ClassStyles.CS_VREDRAW;
                windowClassEx.lpfnWndProc = WindowProc;
                windowClassEx.hInstance = processHandle;
                //windowClass.hIcon = iconSource.Handle;
                windowClassEx.lpszClassName = szWindowClass;
                if (windowDetails.DisableCloseButton)
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

                //Create main window
                _hWnd_MainWindow = CreateWindowEx(windowStyleEx, szWindowClass, szWindowTitle, windowStyle, CW_USEDEFAULT, CW_USEDEFAULT, windowDetails.Width, windowDetails.Height, IntPtr.Zero, IntPtr.Zero, processHandle, IntPtr.Zero);
                if (_hWnd_MainWindow == IntPtr.Zero)
                {
                    Debug.WriteLine("CreateWindowEx failed.");
                    return false;
                }

                //Create desktop window xaml source
                _desktopWindowXamlSource = new DesktopWindowXamlSource();

                //Get native desktop window xaml source
                IDesktopWindowXamlSourceNative2 desktopWindowXamlSourceNative = _desktopWindowXamlSource.As<IDesktopWindowXamlSourceNative2>();

                //Attach DesktopWindowXamlSource to main window
                int attachResult = desktopWindowXamlSourceNative.AttachToWindow(_hWnd_MainWindow);
                if (attachResult < 0)
                {
                    Debug.WriteLine("AttachToWindow failed: " + attachResult);
                    return false;
                }

                //Get DesktopWindowXamlSource window handle
                _hWnd_XamlWindow = desktopWindowXamlSourceNative.GetWindowHandle();
                if (_hWnd_XamlWindow == IntPtr.Zero)
                {
                    Debug.WriteLine("GetWindowHandle failed.");
                    return false;
                }

                //Update DesktopWindowXamlSource window size
                GetClientRect(_hWnd_MainWindow, out RECT rectClient);
                SetWindowPos(_hWnd_XamlWindow, IntPtr.Zero, 0, 0, rectClient.Right, rectClient.Bottom, SetWindowPositions.SWP_SHOWWINDOW);

                //Set DesktopWindowXamlSource content
                _desktopWindowXamlSource.Content = frameworkElement;

                //Window message loop
                MSG lpMsg = new MSG();
                while (GetMessageW(out lpMsg, IntPtr.Zero, 0, 0))
                {
                    TranslateMessage(ref lpMsg);
                    DispatchMessageW(ref lpMsg);
                }

                //Destroy window
                DestroyWindow(_hWnd_MainWindow);
                DestroyWindow(_hWnd_XamlWindow);

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