using Windows.UI.Xaml.Controls;
using static ArnoldVinkCode.AVActions;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="using:ArnoldVinkStyles"
    //Usage:
    //<AVStyles:TextBoxDelay/>
    //textBoxDelay.TextChangedDelay += delegate { ... };
    public partial class TextBoxDelay : TextBox
    {
        public uint DelayTime { get; set; } = 750;
        public event TextChangedEventHandler TextChangedDelay = null;
        private AVHighResTimer TimerDelay = new AVHighResTimer();

        public TextBoxDelay()
        {
            this.TextChanged += TextBoxDelay_TextChanged;
        }

        private void TextBoxDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Start delay timer
                TimerDelay.Interval = DelayTime;
                TimerDelay.Tick = delegate
                {
                    //Stop delay timer
                    TimerDelay.Stop();

                    //Trigger delayed event
                    if (TextChangedDelay != null)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(this.Dispatcher, delegate
                        {
                            TextChangedDelay(sender, e);
                        });
                    }
                };
                TimerDelay.Start();
            }
            catch { }
        }
    }
}