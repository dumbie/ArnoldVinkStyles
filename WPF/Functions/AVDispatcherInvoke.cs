using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ArnoldVinkStyles
{
    public partial class AVDispatcherInvoke
    {
        ///<param name="actionRun">void DispatchAction() { void(); }</param>
        ///<example>DispatcherInvoke(DispatchAction);</example>
        ///<example>DispatcherInvoke(delegate { void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static void DispatcherInvoke(Action actionRun)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(actionRun, DispatcherPriority.Normal);
            }
            catch { }
        }

        ///<param name="actionRun">async Task DispatchAction() { await void(); }</param>
        ///<example>await DispatcherInvoke(DispatchAction);</example>
        ///<example>await DispatcherInvoke(async delegate { await void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static async Task DispatcherInvoke(Func<Task> actionRun)
        {
            try
            {
                TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
                await Application.Current.Dispatcher.InvokeAsync(async delegate
                {
                    await actionRun();
                    taskCompletionSource.SetResult();
                },
                DispatcherPriority.Normal);
                await taskCompletionSource.Task;
            }
            catch { }
        }
    }
}