using ArnoldVinkCode;
using System.Windows.Controls;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="clr-namespace:ArnoldVinkStyles;assembly=ArnoldVinkStyles"
    //Usage:
    //<AVStyles:TextBoxDelay/>

    public class TextBoxDelay : TextBox
    {
        private bool SkipChangedEvent = false;
        private AVTimer DispatcherTimerDelay = new AVTimer();

        public void TextSkipEvent(dynamic newText)
        {
            SkipChangedEvent = true;
            base.Text = newText;
            SkipChangedEvent = false;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (SkipChangedEvent) { return; }
            DispatcherTimerDelay.Renew();
            DispatcherTimerDelay.Interval(1000);
            DispatcherTimerDelay.Action(delegate
            {
                //Debug.WriteLine("Textbox text change delayed.");
                DispatcherTimerDelay.Stop();
                base.OnTextChanged(e);
            });
            DispatcherTimerDelay.Start();
        }
    }
}