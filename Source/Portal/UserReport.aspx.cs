using System;

namespace SOS.Web
{
    public partial class UserReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            try
            {               
                ServiceProxy _serviceProxy = new ServiceProxy();

                string AuthToken = AuthID.Value;
                string UserType = UType.Value;
                grdReport.DataSource = null;
                grdReport.DataSource = _serviceProxy.UserReport(AuthToken, UserType);
                grdReport.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
    }
}