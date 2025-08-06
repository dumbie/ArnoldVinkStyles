using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using static ArnoldVinkCode.AVActions;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="using:ArnoldVinkStyles"
    //Usage:
    //<AVStyles:SliderDelay/>
    //sliderDelay.ValueChangedDelay += delegate { ... };
    public partial class SliderDelay : Slider
    {
        public uint DelayTime { get; set; } = 500;
        public bool PointerWheelScrollEnabled { get; set; } = true;
        public event RangeBaseValueChangedEventHandler ValueChangedDelay = null;
        private AVHighResTimer TimerDelay = new AVHighResTimer();

        public SliderDelay()
        {
            this.ValueChanged += SliderDelay_ValueChanged;
            this.PointerWheelChanged += SliderDelay_PointerWheelChanged;
        }

        private void SliderDelay_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (this.PointerWheelScrollEnabled)
                {
                    int delta = e.GetCurrentPoint((FrameworkElement)sender).Properties.MouseWheelDelta;
                    if (delta > 0)
                    {
                        this.Value += this.LargeChange;
                    }
                    else
                    {
                        this.Value -= this.LargeChange;
                    }
                }
            }
            catch { }
        }

        private void SliderDelay_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            try
            {
                //Start delay timer
                TimerDelay.Interval = DelayTime;
                TimerDelay.TickSet = delegate
                {
                    //Stop delay timer
                    TimerDelay.Stop();

                    //Trigger delayed event
                    if (ValueChangedDelay != null)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(this.Dispatcher, delegate
                        {
                            ValueChangedDelay(sender, e);
                        });
                    }
                };
                TimerDelay.Start();
            }
            catch { }
        }
    }
}