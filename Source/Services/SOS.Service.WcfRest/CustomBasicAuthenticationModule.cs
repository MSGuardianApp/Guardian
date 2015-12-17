using System;
using System.Web;
using SOS.Service.Security;
using System.Collections.Concurrent;

namespace SOS.RESTService
{
    public class CustomBasicAuthenticationModule : IHttpModule
    {
        private static ConcurrentDictionary<string, string> AuthCache = new ConcurrentDictionary<string, string>(); //Local Cache for AuthID and UserID
        private static DateTime lastCacheRefreshedTime = DateTime.Now;

        public void Dispose() { }


        public delegate void MyEventHandler(Object s, EventArgs e);

        private MyEventHandler _eventHandler = null;

        public event MyEventHandler MyEvent
        {
            add { _eventHandler += value; }
            remove { _eventHandler -= value; }
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            string authID = app.Request.Headers["AuthID"];
            string recievedAuthTokenType = app.Request.Headers["AuthTokenType"];
            if (String.IsNullOrEmpty(authID))
            {
                AccessDenied(app);
                return;
            }

            #region Caching for Auth Token

            if (lastCacheRefreshedTime < DateTime.Now.AddHours(-1))// Local Cache reset interval - 1hr
            {
                AuthCache.Clear();
                lastCacheRefreshedTime = DateTime.Now;
            }

            string userID = string.Empty;
            if (AuthCache.TryGetValue(authID, out userID))
            {
                if (userID == string.Empty)
                    AccessDenied(app);
                else
                    app.Request.Headers.Add("LiveUserID", userID);
                return;
            }

            #endregion
            AuthTokenValidator.AuthTokenType currentAuthTokenType = AuthTokenValidator.AuthTokenType.LiveAuthToken;
            if (!string.IsNullOrEmpty(recievedAuthTokenType))
                currentAuthTokenType = (AuthTokenValidator.AuthTokenType)Convert.ToInt32(recievedAuthTokenType);

            AuthTokenValidator atv = new AuthTokenValidator(authID, currentAuthTokenType);

            AuthTokenValidationResult result = atv.Result;

            if (!result.IsValid)
            {
                AuthCache.TryAdd(authID, string.Empty);
                AccessDenied(app);
            }
            //else if (!app.Request.RawUrl.Contains("PostMyLocation") && !app.Request.RawUrl.Contains("ReportIncident"))
            //{
            //    if (result.IsExpired)
            //    {
            //        AuthCache.TryAdd(authID, string.Empty);
            //        AccessDenied(app);
            //    }
            //}

            AuthCache.TryAdd(authID, result.UserID);
            app.Request.Headers.Add("LiveUserID", result.UserID);
        }


        void context_EndRequest(object sender, EventArgs e)
        {

        }

        private void AccessDenied(HttpApplication app)
        {
            //throw new HttpException(401, "Access Denied");
            app.Response.StatusCode = 401;
            app.Response.StatusDescription = "Access Denied";

            // write to browser  
            app.Response.Write("401 Access Denied");
            app.Response.End();
        }
    }
}