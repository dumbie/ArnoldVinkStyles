using System.Windows.Controls;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkStyles.AVDispatcherInvoke;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="clr-namespace:ArnoldVinkStyles;assembly=ArnoldVinkStyles"
    //Usage:
    //<AVStyles:TextBoxDelay/>

    public class TextBoxDelay : TextBox
    {
        private bool SkipChangedEvent = false;
        private AVHighResTimer TimerDelay = new AVHighResTimer();

        public void TextSkipEvent(dynamic newText)
        {
            SkipChangedEvent = true;
            base.Text = newText;
            SkipChangedEvent = false;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (SkipChangedEvent) { return; }

            //Start delay timer
            TimerDelay.Interval = 1000;
            TimerDelay.Tick = delegate
            {
                DispatcherInvoke(delegate
                {
                    //Stop delay timer
                    TimerDelay.Stop();

                    //Debug.WriteLine("Textbox text change delayed.");
                    base.OnTextChanged(e);
                });
            };
            TimerDelay.Start();
        }
    }
}