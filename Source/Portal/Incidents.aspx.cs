using System;

namespace SOS.Web
{
    public partial class Incidents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }


        private string GetPopUPMessage()
        {

            string message = "Please Select (Start Date And End Date) or Set Incident ID to get the Report";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");
            return sb.ToString();

        }
        protected void submitBtn_Click(object sender, EventArgs e)
        {
            try
            {
             
                string AuthToken = AuthID.Value;
                string UserType = UType.Value;

                ServiceProxy _serviceProxy = new ServiceProxy();
                if (!string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
                {
                    gvIncidents.DataSource = _serviceProxy.IncidentsDataByFilterRpt(AuthToken, UserType, startDate.Text, endDate.Text);
                    gvIncidents.DataBind();
                }

                else if( !string.IsNullOrEmpty(incidentID.Text))
                {
                    gvIncidents.DataSource = _serviceProxy.IncidentsDataByIDRpt(AuthToken, UserType, incidentID.Text);
                    gvIncidents.DataBind();

                }

                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", GetPopUPMessage());

            }
            catch (Exception ex)
            {

            }
        }

               
    }
}