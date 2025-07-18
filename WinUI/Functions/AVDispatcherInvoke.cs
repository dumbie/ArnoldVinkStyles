using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ArnoldVinkStyles
{
    public partial class AVDispatcherInvoke
    {
        ///<param name="actionRun">void DispatchAction() { void(); }</param>
        ///<example>DispatcherInvoke(this, DispatchAction);</example>
        ///<example>DispatcherInvoke(this, delegate { void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static void DispatcherInvoke(FrameworkElement frameworkElement, Action actionRun)
        {
            try
            {
                CoreDispatcher dispatcher = frameworkElement.Dispatcher;
                if (dispatcher.HasThreadAccess)
                {
                    actionRun();
                }
                else
                {
                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate { actionRun(); }).Wait();
                }
            }
            catch { }
        }

        ///<param name="actionRun">async Task DispatchAction() { await void(); }</param>
        ///<example>await DispatcherInvoke(this, DispatchAction);</example>
        ///<example>await DispatcherInvoke(this, async delegate { await void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static async Task DispatcherInvoke(FrameworkElement frameworkElement, Func<Task> actionRun)
        {
            try
            {
                CoreDispatcher dispatcher = frameworkElement.Dispatcher;
                if (dispatcher.HasThreadAccess)
                {
                    await actionRun();
                }
                else
                {
                    TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async delegate
                    {
                        await actionRun();
                        taskCompletionSource.SetResult();
                    });
                    await taskCompletionSource.Task;
                }
            }
            catch { }
        }
    }
}