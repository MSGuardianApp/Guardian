using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SOS.Test.ServiceClient
{
    public static class WebClientExtensions
    {
        public static Task<string> DownloadStringTask(this WebClient webClient, Uri uri)
        {
            var tcs = new TaskCompletionSource<string>();

            webClient.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            webClient.DownloadStringAsync(uri);

            return tcs.Task;
        }

        public static Task<Stream> OpenReadTask(this WebClient webClient, Uri uri)
        {
            var tcs = new TaskCompletionSource<Stream>();

            webClient.OpenReadCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            webClient.OpenReadAsync(uri);

            return tcs.Task;
        }

        public static Task<string> UploadStringTask(this WebClient webClient, Uri uri, string data)
        {
            var tcs = new TaskCompletionSource<string>();

            webClient.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else if (e.Cancelled)
                {
                    tcs.SetCanceled();
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            webClient.UploadStringAsync(uri, "POST", data);

            return tcs.Task;
        }
    }
}
