using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ArnoldVinkStyles
{
    public partial class AVFocus
    {
        public class AVFocusDetails
        {
            public int FocusIndex { get; set; } = 0;
            public ListView FocusListView { get; set; } = null;
            public FrameworkElement FocusElement { get; set; } = null;

            public void Reset()
            {
                FocusIndex = 0;
                FocusListView = null;
                FocusElement = null;
            }
        }
    }
}