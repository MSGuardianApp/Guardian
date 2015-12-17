using System;
using System.Web.UI.WebControls;

namespace SOS.Web
{
    public partial class ActiveUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                gvSOSReport.DataSource = _serviceProxy.HistorySOSAndTrackData(AuthToken, UserType, Convert.ToDateTime(startDate.Text).Ticks.ToString(), Convert.ToDateTime(endDate.Text).Ticks.ToString());
                ViewState["ActiveUsers"] = gvSOSReport.DataSource;
                gvSOSReport.DataBind();
            }

        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
            {
                //btnReports.Visible = false;
                LoadGridData();

                //gvSOSReport.DataSource = _serviceProxy.HistorySOSData(AuthToken, UserType, startDateCalender.SelectedDate.Ticks.ToString(), endDateCalender.SelectedDate.Ticks.ToString());
                //gvSOSReport.DataBind();
            }

            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", GetPopUPMessage());

           
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            //if (IsPostBack)
            //    LoadGridData();
            gvSOSReport.DataSource =(object) ViewState["ActiveUsers"];
            gvSOSReport.PageIndex = e.NewPageIndex;
            gvSOSReport.DataBind();
        }
    }
}