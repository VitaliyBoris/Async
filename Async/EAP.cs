using System;
using System.Net;

namespace Async
{
    public class EAP
    {
        #region Event-based Async Pattern

        public static void DumpWebPage(Uri uri)
        {
            var webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDownloadStringCompleted;
            webClient.DownloadStringAsync(uri);
        }

        private static void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
        }

        #endregion

        #region IAsyncResult

        public static void LookupHostName(string hostName)
        {
            object unrelatedObject = "object";
            Dns.BeginGetHostAddresses(hostName, OnHostNameResolved, unrelatedObject);
        }

        private static void OnHostNameResolved(IAsyncResult ar)
        {
            object unrelatedObject = ar.AsyncState;
            var addresses = Dns.EndGetHostAddresses(ar);
            foreach (var ipAddress in addresses)
            {
                Console.WriteLine(ipAddress);
            }
        }

        #endregion

        #region Callback

        public static void GetHostAddress(string hostName, Action<IPAddress> onHostNameResolvedCallback)
        {
            var addresses = Dns.GetHostAddresses(hostName);
            foreach (var ipAddress in addresses)
            {
                onHostNameResolvedCallback(ipAddress);
            }
        }

        #endregion
    }
}