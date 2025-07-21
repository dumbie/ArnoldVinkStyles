using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkStyles.AVDispatcherInvoke;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="clr-namespace:ArnoldVinkStyles;assembly=ArnoldVinkStyles"
    //Usage:
    //<AVStyles:SliderDelay/>

    public class SliderDelay : Slider
    {
        public int DelayTime { get; set; } = 500;
        public bool DelayIgnoreDrag { get; set; } = false;
        public bool SliderThumbDragging { get; protected set; } = false;
        public bool MouseWheelScrollEnabled { get; set; } = true;
        public DateTime LastValueChange { get; protected set; } = DateTime.Now;
        private AVHighResTimer TimerDelay = new AVHighResTimer();
        private bool SkipChangedEvent = false;
        public bool RecentValueChange()
        {
            double changeDifference = (DateTime.Now - LastValueChange).TotalMilliseconds;
            if (changeDifference > 2500 && !SliderThumbDragging)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.MouseWheel += SliderDelay_MouseWheel;
            base.OnInitialized(e);
        }

        protected void SliderDelay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (MouseWheelScrollEnabled)
            {
                if (e.Delta > 0)
                {
                    base.Value += base.LargeChange;
                }
                else
                {
                    base.Value -= base.LargeChange;
                }
            }
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            SliderThumbDragging = false;
            base.OnThumbDragCompleted(e);
        }

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            SliderThumbDragging = true;
            base.OnThumbDragStarted(e);
        }

        public void MinimumSkipEvent(dynamic newMinimum)
        {
            SkipChangedEvent = true;
            base.Minimum = newMinimum;
            SkipChangedEvent = false;
        }

        public void MaximumSkipEvent(dynamic newMaximum)
        {
            SkipChangedEvent = true;
            base.Maximum = newMaximum;
            SkipChangedEvent = false;
        }

        public void ValueSkipEvent(dynamic newValue, bool checkRecentChange)
        {
            if (checkRecentChange && RecentValueChange()) { return; }
            SkipChangedEvent = true;
            base.Value = newValue;
            SkipChangedEvent = false;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            if (SkipChangedEvent) { return; }
            LastValueChange = DateTime.Now;

            //Start delay timer
            TimerDelay.Interval = (uint)DelayTime;
            TimerDelay.Tick = delegate
            {
                if (DelayIgnoreDrag || !SliderThumbDragging)
                {
                    DispatcherInvoke(delegate
                    {
                        //Stop delay timer
                        TimerDelay.Stop();

                        //Debug.WriteLine("Slider value change delayed.");
                        base.OnValueChanged(oldValue, newValue);
                    });
                }
            };
            TimerDelay.Start();
        }
    }
}