using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Lia.Utils
{
    public static class ThreadUtil
    {
        public static async Task TimeoutAfter(Task task, int millisecondsTimeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
            {
                await task;
                return;
            }

            throw new TimeoutException();
        }

        public static async Task OnUI(Action action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (action == null) { return; }

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(priority, delegate { action(); });
        }
    }
}