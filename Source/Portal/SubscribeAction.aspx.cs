//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace SOS.Web
{
    using System;
    using SOS.Service.Interfaces.DataContracts.OutBound;
    using SOS.Service.Interfaces.DataContracts;

    // 
    public partial class SubscribeAction : System.Web.UI.Page
    {
        enum SubscribeActionType
        {
            Subscribe = 1,
            Unsubscribe
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //http://guardiandemo.azurewebsites.net/SubscribeAction.aspx?V=2&pr=ce22d5a4-50e1-43d2-9a4f-6b0c5f38f16a&uid=e3d665f2-e023-4eb2-b8b2-645b02060427&at=u

            if (Page.Request.QueryString["V"] != null)
            {
                // SubscribeAction.aspx?V=2&{0}={1}&{2}={3}&{4}={5}", "pr", profileId, "uid", buddyUserId, "at", actionCode);
                string displayMesage = "The link has expired or the buddy request is no more valid.";
                try
                {
                    //Verification.aspx?V=2&key=0b2ce697-0c0e-4090-8cdb-c743bed05950&pr=e2167152-89b1-49c0-8ff9-03e72d15d174&et=M

                    string userId = Page.Request.QueryString["uid"];
                    string profileId = Page.Request.QueryString["pr"];

                    //Action u:Unsubscribe s:subscribe
                    string actionType = Page.Request.QueryString["at"].ToLowerInvariant();
                    string subscribtionId = Page.Request.QueryString["s"];


                    switch (actionType)
                    {
                        case "s":
                            SubscribeFromProfileAction(profileId, userId, SubscribeActionType.Subscribe, subscribtionId);
                            break;
                        case "u":
                            SubscribeFromProfileAction(profileId, userId, SubscribeActionType.Unsubscribe, subscribtionId);
                            break;
                        default:
                            validationMessage.Text = displayMesage;
                            break;
                    }

                }
                catch
                {
                    validationMessage.Text = displayMesage;
                }
            }
            else
            {
                string redirecturl = Page.Request.Url.ToString();
                redirecturl = redirecturl.Replace(redirecturl.Split('?')[0], Guardian.Common.Config.V1GuardianPortalUri + "/SubscribeAction.aspx");
                Response.Redirect(redirecturl);
            }
        }

        // SubscribeAction Given Buddy from Profile with appropriate action
        private void SubscribeFromProfileAction(string profileId, string userId, SubscribeActionType action, string SubscribtionID)
        {
            string displayMessage = string.Empty;

            ServiceProxy proxy = new ServiceProxy();

            User user = proxy.SubscribeBuddyAction(profileId, userId, Convert.ToString(((int)action)), SubscribtionID);
            string link = string.Empty;
            if (user.DataInfo[0].ResultType == ResultTypeEnum.Success)
            {

                switch (action)
                {
                    case SubscribeActionType.Subscribe:
                        displayMessage = String.Format("You have been subscribed to the buddy list of {0}({1}).", user.Name, user.MobileNumber);
                        link = Page.Request.Url.OriginalString;
                        link = link.Substring(0, link.Length - 1) + "u";
                        displayMessage += String.Format(@"<br/> If you have subscribed in error, click <a href='{0}'>here</a> to unsubscribe again.", link);
                        break;

                    case SubscribeActionType.Unsubscribe:
                        displayMessage = String.Format("You have been unsubscribed from the buddy list of {0}({1}).", user.Name, user.MobileNumber);
                        link = Page.Request.Url.OriginalString;
                        link = link.Substring(0, link.Length - 1) + "s";
                        displayMessage += String.Format(@"<br/> If you have unsubscribed in error, click <a href='{0}'>here</a> to subscribe  again.", link);
                        break;
                }
            }
            else
            {
                displayMessage = "The link has expired or the buddy request is no more valid.";
            }

            validationMessage.Text = displayMessage;
        }
    }
}