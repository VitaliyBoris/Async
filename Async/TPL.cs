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

        public static void LookupHostNames(string[] hostNames)
        {
            LookupHostNamesHelper(hostNames, 0);
        }

        private static void LookupHostNamesHelper(string[] hostNames, int i)
        {
            Task<IPAddress[]> ipAddressesPromise = Dns.GetHostAddressesAsync(hostNames[i]);
            ipAddressesPromise.ContinueWith(_ =>
            {
                IPAddress[] ipAddresses = ipAddressesPromise.Result;
                foreach (var ipAddress in ipAddresses)
                {
                    Console.WriteLine(ipAddress);
                }

                if (i + 1 < hostNames.Length)
                {
                    LookupHostNamesHelper(hostNames, i + 1);
                }
            });
        }

        #endregion

        #region Await

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

        #endregion
    }
}
