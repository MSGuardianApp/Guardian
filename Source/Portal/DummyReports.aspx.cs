using System;
using SOS.Service.Implementation;

namespace SOS.Web
{
    public partial class DummyReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServiceProxy _serviceProxy = new ServiceProxy();
            

            try
            {
                 //string AuthID = "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM5NDU0NzAxNywidWlkIjoiZTY5MzBkNjUzYTJkNTUxYjFkNzJhZGE0YjQxNmE4OTciLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.S6cEZz8XPpHYG4PkKK6UNu-IqGtiIlbJ8pM75LF9P7k";
                string AuthID = "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM5NDU0NzAxNywidWlkIjoiZTY5MzBkNjUzYTJkNTUxYjFkNzJhZGE0YjQxNmE4OTciLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.S6cEZz8XPpHYG4PkKK6UNu-IqGtiIlbJ8pM75LF9P7k";
                //string AuthID = "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM5NDI3MDkzOSwidWlkIjoiZTY5MzBkNjUzYTJkNTUxYjFkNzJhZGE0YjQxNmE4OTciLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.xo6Ynf_Nb0Q1xUjKR-GwYJOP7wVXhVfRkfeC_UBtdQw";
                //string AuthID = "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM5NDI3MzMyNiwidWlkIjoiM2FmZGY5MWFjZTQ1ZmJlZjUwYmQ3MzZkMDRlYjEzM2YiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.KkRaoXfYhxnW5itvZWdus1rerdhqiIAlUvE8Iop1J7Q";




               // _serviceProxy.AutheriseUsers(AuthID, Convert.ToChar(UType));

               // _serviceProxy.GetAdminProfile("sochyd@live.com");

                //var xyz = _serviceProxy.GetMemberStatusFromLiveInfo("3","aaaaa");

              /*  var grpMembers = _serviceProxy.GetGroupMembersWithStatus("1");
                string tablestring = "";
                tablestring = tablestring + "<table width=\"100%\"><tr  style=\"background:#E2EBFC\"> <td colspan=\"3\" style=\"text-align:center;\"><b> <font color=\"black\" size=\"5\">  Group Members With Status </font></b></td>";
                
             /*   if (grpMembers != null)
                {
                    tablestring = tablestring + "<tr style=\"background:#dcdcdc\"><td><b>Profile Id</b></td><td><b>User Name</b></td><td><b>Status</b></td></tr>";
                    foreach (var member in grpMembers)
                    {
                        tablestring = tablestring + "<tr style=\"background:#f5f5f5\"><td >" + member.ProfileId + "</td><td>" + member.Name + "</td><td>" +member.Status + "</td></tr>";
                    }
                }
                else
                {
                    tablestring = tablestring + "<tr style=\"background:#dcdcdc\"><td colspan=\"3\"> No members for this group </td></tr>";
                } */
              /*  tablestring = tablestring + "</table>";
                lblTable.Text = tablestring; */

                // --------------------------------------------
                
            /*    var LiveGeoDelta = _serviceProxy.GetLiveGeo_Delta("1", DateTime.Now);
                string tableLiveGeo = "";
                tableLiveGeo = tableLiveGeo + "<table width=\"100%\"><tr  style=\"background:#E2EBFC\"> <td colspan=\"2\" style=\"text-align:center;\"><b> <font color=\"black\" size=\"5\"> Live Geo Delta </font></b></td>";

                if (LiveGeoDelta != null)
                {
                    tableLiveGeo = tableLiveGeo + "<tr style=\"background:#dcdcdc\"><td><b>Profile Id</b></td><td><b>Status</b></td></tr>";
                    foreach (var member in LiveGeoDelta)
                    {
                        tableLiveGeo = tableLiveGeo + "<tr style=\"background:#f5f5f5\"><td >" + member.ProfileId + "</td><td>" + member.Status + "</td></tr>";
                    }
                }
                else
                {
                    tableLiveGeo = tableLiveGeo + "<tr style=\"background:#dcdcdc\"><td colspan=\"2\"> No members for this group </td></tr>";
                }
                tableLiveGeo = tableLiveGeo + "</table>";
                lblTable.Text = tableLiveGeo; */

                // --------------------------------------------------

                //var Incidents = _serviceProxy.GetIncidentsbyDates(DateTime.Now.ToString(), DateTime.Parse("01/01/2014"), DateTime.Now);

            }
            catch (Exception ex)
            {

            }
        }


        protected void btnUpdateUserName_Click(object sender, EventArgs e)
        {
            string AuthToken= AuthID.Value;
            string UserType = UType.Value;
           
            ReportService _rptService = new ReportService();
            //_rptService.UpdateNameInGroupMembership();
        }

        protected void btnEncryptPhoneNumber_Click(object sender, EventArgs e)
        {
            string AuthToken = AuthID.Value;
            string UserType = UType.Value;

            ReportService _rptService = new ReportService();
            _rptService.encryptPhoneNumber();
        }
    }
}