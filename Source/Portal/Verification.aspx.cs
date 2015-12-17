using System;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Web
{
    /*
        For Marshal Validation
        [WebGet(UriTemplate = "/ValidateGroupMarshal/{ValidationID}/{ProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo ValidateGroupMarshal(string ValidationID, string ProfileID);

        For User Group Validation
        [WebGet(UriTemplate = "/ValidateGroupMemeber/{ValidationID}/{ProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo ValidateGroupMemeber(string ValidationID, string ProfileID);

        Validation ID is nothing but the key you would receive in the URI and Profile ID is nothing but pr in URI. 

        These methods are present in GroupService.cs. 

        Output: 

        If successful 
               ResultType = Success 
               Message = Verified 
        If unsuccessful 
               ResultTupe = Error / Exception
               Message = "Invalid VaidationID" OR "Invalid Input" OR "Failed" depending upon the situation. 

         */
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //http://guardiantest.azurewebsites.net/Verification.aspx?V=2&key=7b024f88-36b1-401c-b7c9-561179853af5&pr=a57dfd6e-7cb2-4d88-bc38-ce8ce3fc561c&et=G
            //

            //Verification.aspx?V=2&key=0b2ce697-0c0e-4090-8cdb-c743bed05950&pr=e2167152-89b1-49c0-8ff9-03e72d15d174&et=M
            if (Page.Request.QueryString["V"] != null)
            {
                string key = Page.Request.QueryString["key"];
                string profileId = Page.Request.QueryString["pr"];
                string entityType = Page.Request.QueryString["et"];
                ResultInfo result = null;

                try
                {
                    switch (entityType)
                    {
                        case "G":
                            result = ValidateGroupAssociation(key, profileId);
                            break;
                        case "M":
                            result = ValidateMarshalAssociation(key, profileId);
                            break;
                    }

                }
                catch
                {
                }

                string displayMessage = "Verification of this account has failed. Please verify that the url is correct and try again.";
                if (result != null)
                {
                    switch (result.ResultType)
                    {
                        case ResultTypeEnum.Success:
                            displayMessage = entityType == "G" ?
                                "Your group association has been verified successfully! Now onwards, this security group will be notified with your tracking/SOS information." :
                                "You are successfully added as Marshal to the group!";
                            break;
                        case ResultTypeEnum.Error:
                        case ResultTypeEnum.Exception:
                            displayMessage = "Verification of this account has failed. Please verify that the url is correct and try again.";
                            break;
                        //case "already validated":
                        //    displayMessage = "Your account is already active";
                        //    break;
                    }
                }
                validationMessage.Text = displayMessage;
            }
            else
            {
                string redirecturl = Page.Request.Url.ToString();
                redirecturl = redirecturl.Replace(redirecturl.Split('?')[0],  SOS.ConfigManager.Config.V1GuardianPortalUri + "/Verification.aspx");
                Response.Redirect(redirecturl);
            }
        }


        private ResultInfo ValidateGroupAssociation(string key, string profileId)
        {
            return (new ServiceProxy()).ValidateGroupMember(key, profileId);
        }

        private ResultInfo ValidateMarshalAssociation(string key, string profileId)
        {
            
            return (new ServiceProxy()).ValidateGroupMarshal(key, profileId);
        }
    }
}