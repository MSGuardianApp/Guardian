using System;

namespace SOS.Web
{
    public partial class TestPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLiveID_Click(object sender, EventArgs e)
        {
            if (txtLiveAuthID.Value != "")
            {
                var secValidator = new SOS.Service.Security.AuthTokenValidator(txtLiveAuthID.Value);
                txtLiveUserID.Value = secValidator.Result.UserID;
            }
        }
    }
}