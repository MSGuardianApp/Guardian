using System;

namespace SOS.Web
{
    public partial class AllUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        private void LoadUsersGridData()
        {
            string AuthToken = AuthID.Value;
            string UserType = UType.Value;
            string GroupID = GroupId.Value;
            

            ServiceProxy _serviceProxy = new ServiceProxy();

            ////Case-1
            //if (!string.IsNullOrEmpty(txtSearchKey.Text.Trim()) && !string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
            //    gvAllUsers.DataSource = _serviceProxy.GetAllGroupMembers(AuthToken, UserType, GroupID, txtSearchKey.Text.Trim(), DateTime.UtcNow.Ticks.ToString(), startDate.Text, endDate.Text);

            ////Case-2
            //if (string.IsNullOrEmpty(txtSearchKey.Text.Trim()) && !string.IsNullOrEmpty(startDate.Text) && !string.IsNullOrEmpty(endDate.Text))
            //    gvAllUsers.DataSource = _serviceProxy.GetAllGroupMembers(AuthToken, UserType, GroupID, string.Empty, DateTime.UtcNow.Ticks.ToString(), startDate.Text, endDate.Text);


            ////Case-3
            //if (!string.IsNullOrEmpty(txtSearchKey.Text.Trim()) && string.IsNullOrEmpty(startDate.Text) && string.IsNullOrEmpty(endDate.Text))
            //    gvAllUsers.DataSource = _serviceProxy.GetAllGroupMembers(AuthToken, UserType, GroupID, txtSearchKey.Text.Trim(), DateTime.UtcNow.Ticks.ToString(), string.Empty, string.Empty);

            ////Case-4
            //if (!string.IsNullOrEmpty(txtSearchKey.Text.Trim()) && !string.IsNullOrEmpty(startDate.Text) && string.IsNullOrEmpty(endDate.Text))
            //    gvAllUsers.DataSource = _serviceProxy.GetAllGroupMembers(AuthToken, UserType, GroupID, txtSearchKey.Text.Trim(), DateTime.UtcNow.Ticks.ToString(), startDate.Text, string.Empty);

            gvAllUsers.DataSource = _serviceProxy.GetAllGroupMembers(AuthToken, UserType, GroupID, txtSearchKey.Text.Trim(), DateTime.UtcNow.Ticks.ToString(), startDate.Text, endDate.Text);
            gvAllUsers.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadUsersGridData();
        }
    }
}