using System;
using System.Web.UI.WebControls;


namespace SOS.Web
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        protected void btnReports_Click(object sender, EventArgs e)
        {
            //btnReports.Visible = false;
            try
            {
                string AuthToken = AuthID.Value;
                string UserType = UType.Value;

                ServiceProxy _serviceProxy = new ServiceProxy();
                lblUserCount.Text = "0";

                lblMissed.Text = _serviceProxy.MissedActivationCount(AuthToken, UserType).ToString();
                lblUserCount.Text = _serviceProxy.UserCount(AuthToken, UserType).ToString();

                gvGroups.DataSource = _serviceProxy.GroupUsers(AuthToken, UserType);
                gvGroups.DataBind();

                grdActiveSOS.DataSource = _serviceProxy.ActiveSOSData(AuthToken, UserType);
                grdActiveSOS.DataBind();
            }

            catch (Exception ex)
            {

            }
        }

        private string GetPopUPMessage()
        {

            string message = "Please Select Start Date And End Date before get the Report";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");
            return sb.ToString();

        }

        private void LoadGridData()
        {
            string AuthToken = AuthID.Value;
            string UserType = UType.Value;

            ServiceProxy _serviceProxy = new ServiceProxy();
            if (!string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
            {
                gvSOSReport.DataSource = _serviceProxy.HistorySOSData(AuthToken, UserType, Convert.ToDateTime(startDate.Text).Ticks.ToString(), Convert.ToDateTime(endDate.Text).Ticks.ToString());
                ViewState["Incidents"] = gvSOSReport.DataSource;
                gvSOSReport.DataBind();
            }

        }
        protected void btnHistorySOS_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
            {
                //btnHistorySOS.Visible = false;
                LoadGridData();
            }
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", GetPopUPMessage());

        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            //if (IsPostBack)
            //    LoadGridData();
            gvSOSReport.DataSource = (object)ViewState["Incidents"];
            gvSOSReport.PageIndex = e.NewPageIndex;
            gvSOSReport.DataBind();
        }

    }
}