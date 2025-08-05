using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ArnoldVinkStyles
{
    public partial class AVDispatcherInvoke
    {
        ///<param name="actionRun">void DispatchAction() { void(); }</param>
        ///<example>DispatcherInvoke(this.Dispatcher, DispatchAction);</example>
        ///<example>DispatcherInvoke(this.Dispatcher, delegate { void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static void DispatcherInvoke(CoreDispatcher dispatcher, Action actionRun)
        {
            try
            {
                if (dispatcher == null)
                {
                    dispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
                }
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
        ///<example>await DispatcherInvoke(this.Dispatcher, DispatchAction);</example>
        ///<example>await DispatcherInvoke(this.Dispatcher, async delegate { await void(); });</example>
        ///<summary>Don't forget to use try and catch to improve stability</summary>
        public static async Task DispatcherInvoke(CoreDispatcher dispatcher, Func<Task> actionRun)
        {
            try
            {
                if (dispatcher == null)
                {
                    dispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
                }
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