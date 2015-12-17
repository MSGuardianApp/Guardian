using System;
using System.Collections.Generic;
using Facebook;
using System.Diagnostics;

namespace SOS.Service.Utility
{
    public static class FaceBookPosting
    {
        static int MaxRetryCount = 2;
        public static void FBPosting(string accessToken, string message, string caption, string link, string fbGroupID, int retryCount = 0)
        {
            if (retryCount > MaxRetryCount)
                return;
            try
            {
                var fb = new FacebookClient(accessToken);
                var data = new Dictionary<string, object>();
                data.Add("message", message);
                if (link != null) data.Add("link", link);
                data.Add("caption", caption);
                fb.Post(string.Format("/{0}/feed", fbGroupID), data);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error while posting (retry: {0} ) to Facebook: {1}", retryCount.ToString(), ex.Message));
                FBPosting(accessToken, message, caption, link, fbGroupID, retryCount + 1);
            }

        }
    }
}