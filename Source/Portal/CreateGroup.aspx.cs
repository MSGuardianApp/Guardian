using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SOS.Service.Implementation;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.IO;
using SOS.AzureStorageAccessLayer;
using SOS.Service.Utility;


namespace SOS.Web
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        private string AuthToken = "";
        private string UserType = "";

        static List<GroupDTO> groupList = new List<GroupDTO>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["AuthID"] != null)
                AuthToken = Convert.ToString(Request.QueryString["AuthID"]);
            if (Request.QueryString["Utype"] != null)
                UserType = Convert.ToString(Request.QueryString["Utype"]);
            if (!Page.IsPostBack)
            {
                BindAllGroups();
            }

        }

        public void BindAllGroups()
        {
            groupList = GetGroups();
            if (groupList != null)
            {
                lvwgroup.DataSource = groupList;
                lvwgroup.DataBind();
                HdnFileName.Value = (groupList.Max(x => x.GroupID) + 1).ToString();
            }
            else
                lblSuccess.Text = "You are not authorized to view this page.";
        }
        private List<GroupDTO> GetGroups()
        {
            ServiceProxy _serviceProxy = new ServiceProxy();
            return _serviceProxy.GetAllGroupWithAdmins(AuthToken, UserType);
        }


        protected void lvwgroup_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            CloseInsert();
            lvwgroup.EditIndex = e.NewEditIndex;
            BindAllGroups();
            List<GroupDTO> parentgroups = new List<GroupDTO>();
            parentgroups.Add(new GroupDTO() { GroupName = "Select", GroupID = 0 });

            var grpDTO = (List<GroupDTO>)lvwgroup.DataSource;
            var parentList = grpDTO.Where(x => (!x.ParentGroupID.HasValue || x.ParentGroupID.Value == 0));

            var rGrp1 = (RadioButton)lvwgroup.EditItem.FindControl("rGroupType1");
            var rGrp2 = (RadioButton)lvwgroup.EditItem.FindControl("rGroupType2");
            var rGrp3 = (RadioButton)lvwgroup.EditItem.FindControl("rGroupType3");

            int grpId = (int)lvwgroup.DataKeys[e.NewEditIndex]["GroupID"];
            GroupDTO GrpObj = GetGroup(grpDTO, grpId);

            if (GrpObj != null)
            {
                if (GrpObj.GroupType == GroupType.Private)
                    rGrp1.Checked = true;
                else if (GrpObj.GroupType == GroupType.Public)
                    rGrp2.Checked = true;
                else if (GrpObj.GroupType == GroupType.Social)
                    rGrp3.Checked = true;

            }
            BindListViewDropDownGroupNames(lvwgroup.EditItem);
            var ddl = (DropDownList)lvwgroup.EditItem.FindControl("ddlParentGroupNames");
            if (ddl != null)
            {
                if (GrpObj.ParentGroupID != null && ddl.Items.FindByValue(GrpObj.ParentGroupID.Value.ToString()) != null)
                {
                    ddl.Items.FindByValue(GrpObj.ParentGroupID.Value.ToString()).Selected = true;
                }

            }


        }
        private GroupDTO GetGroup(List<GroupDTO> groups, int grpId)
        {
            return groups.FirstOrDefault(x => x.GroupID == grpId);
        }

        private void CloseInsert()
        {
            lvwgroup.InsertItemPosition = InsertItemPosition.None;
            ((LinkButton)lvwgroup.FindControl("NewButton")).Visible = true;
            lblSuccess.Text = string.Empty;

        }

        protected void lvwgroup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }


        protected void lvwgroup_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            int grpId = (int)lvwgroup.DataKeys[e.ItemIndex]["GroupID"];
            GroupService _grpService = new GroupService();
            _grpService.DeleteGroup(grpId);
            BindAllGroups();
        }


        protected void lvwgroup_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            if (e.CancelMode == ListViewCancelMode.CancelingInsert)
            {
                CloseInsert();
            }
            else
            {
                lvwgroup.EditIndex = -1;
            }

            BindAllGroups();
        }

        protected void lvwgroup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Insert":
                    {
                        InsertNewGroup(e.Item);
                        break;
                    }
                case "Update":
                    {
                        UpdateGroup(e.CommandArgument as string, e.Item);
                        break;
                    }
                case "New":
                    {
                        BindListViewDropDownGroupNames(lvwgroup.InsertItem);
                        ((TextBox)lvwgroup.InsertItem.FindControl("GroupKeyTextBox")).Enabled = true;
                        ((DropDownList)lvwgroup.InsertItem.FindControl("CircleKeyDropDownList")).Visible = false;
                        break;
                    }

            }
        }


        private void BindListViewDropDownGroupNames(ListViewItem newListViewItem)
        {

            var grpDTO = (List<GroupDTO>)lvwgroup.DataSource;

            BindGroupListParentCombobox(newListViewItem, grpDTO);

        }
        private void BindGroupListParentCombobox(ListViewItem newListViewItem, List<GroupDTO> grpDTO)
        {
            List<GroupDTO> parentgroups = new List<GroupDTO>();
            if (newListViewItem != null)
            {
                var ddl = (DropDownList)newListViewItem.FindControl("ddlParentGroupNames");
                parentgroups.Add(new GroupDTO() { GroupName = "Select", GroupID = 0 });
                var parentList = grpDTO.Where(x => (!x.ParentGroupID.HasValue || x.ParentGroupID.Value == 0));
                if (ddl != null)
                {
                    parentgroups.AddRange(parentList);
                    ddl.DataSource = parentgroups;
                    ddl.DataBind();
                }
            }


        }


        private void UpdateGroup(string GrpID, ListViewItem editItem)
        {
            Group grp = new Group();
            grp.GroupID = GrpID;
            if (((HiddenField)editItem.FindControl("hdnLocation")) != null)
                grp.GroupLocation = ((HiddenField)editItem.FindControl("hdnLocation")).Value;
            UpsertGroup(grp, editItem);
            lvwgroup.EditIndex = -1;
            lblSuccess.Text = "Successfully updated!";
            BindAllGroups();


        }


        public List<string> WardNames(string ShapeFileID, string SubGroupIdentificationKey)
        {
            GroupStorageAccess _GroupStorageAccess = new GroupStorageAccess();
            var groupListwithGroupNames = _GroupStorageAccess.GetAllGroupsWithGroupNames().OrderBy(x => x.Key);

            ShapeFileGISutility service = new ShapeFileGISutility();
            var wardnames = service.GetAllWardNames(ShapeFileID, SubGroupIdentificationKey);

            return wardnames;
        }

        private void UpsertGroup(Group grp, ListViewItem editItem, bool isCreate = false)
        {
            grp.GroupName = ((TextBox)editItem.FindControl("GroupNameTextBox")).Text;
            grp.Email = ((TextBox)editItem.FindControl("EmailTextBox")).Text;
            grp.PhoneNumber = ((TextBox)editItem.FindControl("PhoneNumberTextBox")).Text;
            grp.EnrollmentKey = ((TextBox)editItem.FindControl("EnrollmentKeyTextBox")).Text;


            if (((TextBox)editItem.FindControl("LocationTextBox")) != null)
                grp.GroupLocation = ((TextBox)editItem.FindControl("LocationTextBox")).Text;

            string ShapeFileName = Path.GetFileNameWithoutExtension(((TextBox)editItem.FindControl("ShapeFileIDTextBox")).Text);
            grp.ShapeFileID = ShapeFileName;


            grp.LiveInfo = new LiveCred();
            grp.LiveInfo.LiveID = ((TextBox)editItem.FindControl("LiveIDTextBox")).Text;
            grp.GeoLocation = ((TextBox)editItem.FindControl("GeoLocationTextBox")).Text;
            grp.ParentGroupID = Convert.ToInt32(((DropDownList)editItem.FindControl("ddlParentGroupNames")).SelectedValue);


            grp.SubGroupIdentificationKey = (grp.ParentGroupID.Value == 0) ? ((TextBox)editItem.FindControl("GroupKeyTextBox")).Text : ((DropDownList)editItem.FindControl("CircleKeyDropDownList")).SelectedValue;

            var rGrp1 = (RadioButton)editItem.FindControl("rGroupType1");
            var rGrp2 = (RadioButton)editItem.FindControl("rGroupType2");
            var rGrp3 = (RadioButton)editItem.FindControl("rGroupType3");

            if (rGrp1.Checked)
            {
                grp.Type = GroupType.Private;
                grp.EnrollmentType = Enrollment.AutoOrgMail;
            }
            if (rGrp2.Checked)
            {
                grp.Type = GroupType.Public;
                grp.EnrollmentType = Enrollment.None;
            }
            if (rGrp3.Checked)
            {
                grp.Type = GroupType.Social;
                grp.EnrollmentType = Enrollment.Moderator;
            }

            if (((CheckBox)editItem.FindControl("IsActiveCheckBox")).Checked)
                grp.IsActive = true;
            else
                grp.IsActive = false;

            if (((CheckBox)editItem.FindControl("NotifySubgroupsCheckBox")).Checked)
                grp.NotifySubgroups = true;
            else
                grp.NotifySubgroups = false;

            if (((CheckBox)editItem.FindControl("AllowGroupManagementCheckBox")).Checked)
                grp.AllowGroupManagement = true;
            else
                grp.AllowGroupManagement = false;

            if (((CheckBox)editItem.FindControl("ShowIncidentsCheckBox")).Checked)
                grp.ShowIncidents = true;
            else
                grp.ShowIncidents = false;

            ServiceProxy _serviceProxy = new ServiceProxy();
            if (!isCreate)
            {
                string partitionKey = ((HiddenField)editItem.FindControl("PartitionKey")).Value;
                string rowKey = ((HiddenField)editItem.FindControl("RowKey")).Value;
                _serviceProxy.EditGroup(AuthToken, UserType, grp, partitionKey, rowKey);
            }
            else
            {
                _serviceProxy.EditGroup(AuthToken, UserType, grp, isCreate: true);
            }
        }

        private void InsertNewGroup(ListViewItem insertItem)
        {

            string grpID = HdnFileName.Value;
            Group grp = new Group();
            grp.GroupID = grpID;
            UpsertGroup(grp, insertItem, true);
            lvwgroup.InsertItemPosition = InsertItemPosition.None;
            BindAllGroups();
            lblSuccess.Text = "Successfully Inserted!";

        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            lvwgroup.EditIndex = -1;
            lvwgroup.InsertItemPosition = InsertItemPosition.FirstItem;
            ((LinkButton)sender).Visible = false;
            BindAllGroups();
        }

        protected void ddlParentGroupNames_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlParentGroups = (DropDownList)sender;
            ListViewItem newItem = (ListViewItem)ddlParentGroups.NamingContainer;

            DropDownList CircleKeyDropDownList = (DropDownList)newItem.FindControl("CircleKeyDropDownList");

            CheckBox NotifySubgroupsCheckBoxCheckBox = (CheckBox)newItem.FindControl("NotifySubgroupsCheckBox");
            LinkButton uploadButton = (LinkButton)newItem.FindControl("uploadButton");
            TextBox ShapeFileIDTextBox = (TextBox)newItem.FindControl("ShapeFileIDTextBox");
            TextBox txtGroupKey = (TextBox)newItem.FindControl("GroupKeyTextBox");

            txtGroupKey.Visible = false; CircleKeyDropDownList.Visible = false;

            if (((DropDownList)sender).SelectedValue.Equals("0"))
            {

                txtGroupKey.Visible = true;
                NotifySubgroupsCheckBoxCheckBox.Visible = true;
                uploadButton.Visible = true;
                ShapeFileIDTextBox.Enabled = true;

            }
            else
            {

                CircleKeyDropDownList.Visible = true;
                NotifySubgroupsCheckBoxCheckBox.Visible = false;
                uploadButton.Visible = false;
                ShapeFileIDTextBox.Enabled = false;
            }

            if (CircleKeyDropDownList.Visible)
            {
                try
                {
                    CircleKeyDropDownList.Items.Clear();

                    string ShapeFileIDs = groupList.Where(x => x.GroupID == Convert.ToInt32(ddlParentGroups.SelectedValue)).Select(y => y.ShapeFileID).FirstOrDefault();
                    string SubGroupIdentificationKey = groupList.Where(x => x.GroupID == Convert.ToInt32(ddlParentGroups.SelectedValue)).Select(y => y.GroupKey).FirstOrDefault();

                    List<string> WardNamesList = WardNames(ShapeFileIDs, SubGroupIdentificationKey);
                    CircleKeyDropDownList.DataSource = WardNamesList;
                    CircleKeyDropDownList.DataBind();
                }

                catch (Exception)
                {
                    Response.Write("<script type=text/javascript>alert('Parent Group Does not have shape file to find Wards!');</script>");
                }


            }

        }

        protected void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btn_Upload_Click(object sender, EventArgs e)
        {

            try
            {
                if (CheckAllRequiredShapeFilesUploaded())
                {

                    BlobAccess blobAccess = new BlobAccess();
                    var container = blobAccess.UploadShapeFiles(ShapeFileIDUpload.PostedFile.InputStream, ShapeFileIDUpload.FileName);
                    container = blobAccess.UploadShapeFiles(FileUploadDescribe.PostedFile.InputStream, FileUploadDescribe.FileName);
                    container = blobAccess.UploadShapeFiles(FileUploadProjection.PostedFile.InputStream, FileUploadProjection.FileName);
                    container = blobAccess.UploadShapeFiles(FileUploadShapeIndex.PostedFile.InputStream, FileUploadShapeIndex.FileName);
                    //saving the file

                    ((TextBox)lvwgroup.InsertItem.FindControl("ShapeFileIDTextBox")).Text = Path.GetFileNameWithoutExtension(ShapeFileIDUpload.FileName);

                    var k = ((TextBox)lvwgroup.InsertItem.FindControl("GroupNameTextBox")).Text;


                    BindGroupListParentCombobox(lvwgroup.InsertItem, GetGroups());
                    lblSuccess.Text = String.Empty;


                }
                else
                {
                    lblSuccess.Text = "Please upload Shape Files Correctly.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private bool CheckAllRequiredShapeFilesUploaded()
        {
            bool flag = ShapeFileIDUpload.HasFile && FileUploadDescribe.HasFile && FileUploadProjection.HasFile && FileUploadShapeIndex.HasFile;
            return flag && Path.GetExtension(ShapeFileIDUpload.FileName) == ".shp" && Path.GetExtension(FileUploadShapeIndex.FileName) == ".shx" &&
                Path.GetExtension(FileUploadDescribe.FileName) == ".dbf" && Path.GetExtension(FileUploadProjection.FileName) == ".prj";



        }


        protected void lvwgroup_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            //set current page startindex, max rows and rebind to false
            (lvwgroup.FindControl("dpGroups") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            //rebind List View
            BindAllGroups();
            BindGroupListParentCombobox(lvwgroup.InsertItem, GetGroups());
        }

        protected void lvwgroup_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            //   lvwgroup.EditIndex = -1;
            e.Cancel = true;
            //CloseInsert();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            lblSuccess.Text = string.Empty;
            BindAllGroups();
        }
    }
}