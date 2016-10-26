using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    public class Additional
    {
        #region Delay

        public static async Task Delay(int ms)
        {
            await Task.Run(() => Thread.Sleep(ms));
        }

        public static Task DelayTcs(int ms)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Timer timer = new Timer(_ => tcs.SetResult(null), null, ms,Timeout.Infinite);
            tcs.Task.ContinueWith(delegate { timer.Dispose(); });
            return tcs.Task;
        }

        #endregion

        #region Timeout

        public static async Task<T> WithTimeout<T>(Task<T> task, int ms)
        {
            Task delayTask = Task.Delay(ms);
            Task first = await Task.WhenAny(task, delayTask);
            if (first == delayTask)
            {
                task.ContinueWith(HandleException);
                throw new TimeoutException();
            }

            return await task;
        }

        private static void HandleException<T>(Task<T> task)
        {
            if (task.Exception != null)
            {
                // logging
            }
        }

        #endregion
    }
}