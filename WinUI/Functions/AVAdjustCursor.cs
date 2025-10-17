using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace ArnoldVinkStyles
{
    public partial class AVAdjustCursor
    {
        //Cursors
        private static CoreCursor cursorHand = new CoreCursor(CoreCursorType.Hand, 0);
        private static CoreCursor cursorArrow = new CoreCursor(CoreCursorType.Arrow, 0);
        private static CoreCursor cursorBeam = new CoreCursor(CoreCursorType.IBeam, 0);
        private static CoreCursor cursorSizeNorthSouth = new CoreCursor(CoreCursorType.SizeNorthSouth, 0);
        private static CoreCursor cursorSizeWestEast = new CoreCursor(CoreCursorType.SizeWestEast, 0);

        /// <summary>
        /// Adjust mouse cursor to hovered framework element
        /// Usage: this.PointerMoved += PointerEvent_AdjustCursor; 
        /// </summary>
        public static void PointerEvent_AdjustCursor(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                //Set Slider cursor
                Slider parentSlider = AVVisualTree.FindVisualParent<Slider>((FrameworkElement)e.OriginalSource);
                if (parentSlider != null)
                {
                    Window.Current.CoreWindow.PointerCursor = cursorSizeWestEast;
                    return;
                }

                //Set ScrollBar cursor
                ScrollBar parentScrollBar = AVVisualTree.FindVisualParent<ScrollBar>((FrameworkElement)e.OriginalSource);
                if (parentScrollBar != null)
                {
                    Window.Current.CoreWindow.PointerCursor = cursorSizeNorthSouth;
                    return;
                }

                //Set Button cursor
                Button parentButton = AVVisualTree.FindVisualParent<Button>((FrameworkElement)e.OriginalSource);
                if (parentButton != null)
                {
                    Window.Current.CoreWindow.PointerCursor = cursorHand;
                    return;
                }

                //Set TextBox cursor
                TextBox parentTextBox = AVVisualTree.FindVisualParent<TextBox>((FrameworkElement)e.OriginalSource);
                if (parentTextBox != null)
                {
                    Window.Current.CoreWindow.PointerCursor = cursorBeam;
                    return;
                }

                //Set default cursor
                Window.Current.CoreWindow.PointerCursor = cursorArrow;
            }
            catch { }
        }
    }
}