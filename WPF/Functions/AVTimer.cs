using System.Windows.Threading;

namespace ArnoldVinkStyles
{
    public partial class AVTimer
    {

        //Reset dispatch timer tick estimate
        public static void TimerReset(DispatcherTimer dispatchTimer)
        {
            try
            {
                if (dispatchTimer != null)
                {
                    dispatchTimer.Stop();
                    dispatchTimer.Start();
                }
            }
            catch { }
        }

        //Stop dispatch timer safely
        public static void TimerStop(DispatcherTimer dispatchTimer)
        {
            try
            {
                if (dispatchTimer != null)
                {
                    dispatchTimer.Stop();
                }
            }
            catch { }
        }

        //Renew dispatch timer
        public static void TimerRenew(ref DispatcherTimer dispatchTimer)
        {
            try
            {
                if (dispatchTimer != null) { dispatchTimer.Stop(); }
                dispatchTimer = new DispatcherTimer();
            }
            catch { }
        }
    }
}