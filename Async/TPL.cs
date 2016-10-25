using System;
using System.Net;
using System.Threading.Tasks;

namespace Async
{
    class TPL
    {
        #region Task

        public static void LookupHostNameTask(string hostName)
        {
            Task<IPAddress[]> ipAddressesPromise = Dns.GetHostAddressesAsync(hostName);
            ipAddressesPromise.ContinueWith(_ =>
            {
                IPAddress[] ipAddresses = ipAddressesPromise.Result;
                foreach (var ipAddress in ipAddresses)
                {
                    Console.WriteLine(ipAddress);
                }
            });
        }

        #endregion

        public static async void DumpWebPageAsync(Uri uri)
        {
            var webClient = new WebClient();
            var task = webClient.DownloadStringTaskAsync(uri);
            var page = await task;
            Console.WriteLine(page);
        }

        private static async Task<int> GetPageSizeAsync(Uri uri)
        {
            var webClient = new WebClient();
            string page = await webClient.DownloadStringTaskAsync(uri);
            return page.Length;
        }

        public static async Task<Uri> FindLargestWebPage(Uri[] uris)
        {
            Uri largest = null;
            int largestSize = 0;
            foreach (var uri in uris)
            {
                int size = await GetPageSizeAsync(uri);
                if (size > largestSize)
                {
                    size = largestSize;
                    largest = uri;
                }
            }
            return largest;
        }
    }
}
