using System;

namespace SOS.Web
{
    public partial class ActiveSOSReport : System.Web.UI.Page
    {
        internal static string AuthToken; 
        internal static  string UserType;
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }


        [System.Web.Services.WebMethod]
        public static bool SOSStop(string profileId)
        {
            
                ServiceProxy _serviceProxy = new ServiceProxy();
                bool updateSOSFlag = _serviceProxy.StopAllPostingsRpt(AuthToken, UserType,profileId);                
                return updateSOSFlag;
        }
       
        public void GetReport()
        {
            ServiceProxy _serviceProxy = new ServiceProxy();

            try
            {

                // btnReports.Visible = false;

                 AuthToken = AuthID.Value;                
                 UserType = UType.Value;

                var sosList = _serviceProxy.ActiveSOSData(AuthToken, UserType);
                string tablestring = "";
               // tablestring = tablestring + "<table width=\"100%\"><tr  style=\"background:#E2EBFC\"> <td colspan=\"5\" style=\"text-align:center;\"><b> <font color=\"black\" size=\"5\">  Active SOS Sessions Report </font></b></td>";

                if (sosList.Count != 0)
                {
                    tablestring = tablestring + "<table width=\"100%\"><tr style=\"background:#dcdcdc\"><td><b>SNo</b></td><td><b>UserName</b></td><td><b>MobileNumber</b></td><td><b>StartTime</b></td><td><b>GroupName</b></td><td><b>Action</b></td></tr>";
                    foreach (var sos in sosList)
                    {
                        tablestring = tablestring + "<tr style=\"background:#f5f5f5\"><td >" + sos.SNo + "</td><td>" + sos.UserName + "</td><td>" + sos.MobileNumber + "</td><td>" + sos.StartTime + "</td><td>" + sos.GroupName + "</td><td>" + "<button id='Stop SOS' onclick=\"sosJSFunction('" + sos.ProfileId + "')\">Stop SOS</button>" + "</td></tr>";
                    }
                }
                else
                {
                    tablestring = tablestring + "<tr style=\"background:#dcdcdc\"><td colspan=\"5\"> No Active SOS alerts available at Present </td></tr>";
                }
                tablestring = tablestring + "</table>";
                lblTable.Text = tablestring;

               
            }
                

            catch (Exception ex)
            {

            }
        }
  
        protected void btnReports_Click(object sender, EventArgs e)
        {
            GetReport();
        }
    }

}