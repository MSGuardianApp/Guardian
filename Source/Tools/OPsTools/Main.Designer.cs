namespace SOS.OPsTools
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainTab = new System.Windows.Forms.TabControl();
            this.GroupCreationTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rGroupType1 = new System.Windows.Forms.RadioButton();
            this.rGroupType2 = new System.Windows.Forms.RadioButton();
            this.rGroupType3 = new System.Windows.Forms.RadioButton();
            this.btnShapeFile = new System.Windows.Forms.Button();
            this.btnCreateGroup = new System.Windows.Forms.Button();
            this.cmbParentGroupName = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtGroupKey = new System.Windows.Forms.TextBox();
            this.chkAllowGroupManagemen = new System.Windows.Forms.CheckBox();
            this.chkShowIncidents = new System.Windows.Forms.CheckBox();
            this.chkNotifySubgroups = new System.Windows.Forms.CheckBox();
            this.txtShapeFile = new System.Windows.Forms.TextBox();
            this.txtLiveAuthID = new System.Windows.Forms.TextBox();
            this.txtliveUserID = new System.Windows.Forms.TextBox();
            this.txtEnrollmentKey = new System.Windows.Forms.TextBox();
            this.rEnrollmentType3 = new System.Windows.Forms.RadioButton();
            this.rEnrollmentType2 = new System.Windows.Forms.RadioButton();
            this.rEnrollmentType1 = new System.Windows.Forms.RadioButton();
            this.txtGeoLocation = new System.Windows.Forms.TextBox();
            this.txtGroupLocation = new System.Windows.Forms.TextBox();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblSetting = new System.Windows.Forms.Label();
            this.lblCircleKey = new System.Windows.Forms.Label();
            this.lblShapeFiles = new System.Windows.Forms.Label();
            this.lblLiveAuthID = new System.Windows.Forms.Label();
            this.lblLiveUserID = new System.Windows.Forms.Label();
            this.lblDomainKey = new System.Windows.Forms.Label();
            this.lblGroupType = new System.Windows.Forms.Label();
            this.lblAuthenticationType = new System.Windows.Forms.Label();
            this.lblFocusLatLong = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblParentGroupName = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.CryptoTab = new System.Windows.Forms.TabPage();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CryptoTextBox = new System.Windows.Forms.TextBox();
            this.EnterTextLabel = new System.Windows.Forms.Label();
            this.EncryptButton = new System.Windows.Forms.Button();
            this.ResendVerificationSMSTab = new System.Windows.Forms.TabPage();
            this.securityCodePhoneTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.GetSecurityCodeButton = new System.Windows.Forms.Button();
            this.DaysTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GetPendingVerificationsButton = new System.Windows.Forms.Button();
            this.ResendSMSToAllButton = new System.Windows.Forms.Button();
            this.PendingVerificationDataGridView = new System.Windows.Forms.DataGridView();
            this.SendEmailTab = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.SendEmail = new System.Windows.Forms.Button();
            this.ProfileSearchTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.RegionCodeTextBox = new System.Windows.Forms.TextBox();
            this.GetUsersByRegionButton = new System.Windows.Forms.Button();
            this.GetAllUsersOutsideOfIndiaButton = new System.Windows.Forms.Button();
            this.ProfileGridView = new System.Windows.Forms.DataGridView();
            this.BuddySearchTab = new System.Windows.Forms.TabPage();
            this.BuddyOutsideIndiaButton = new System.Windows.Forms.Button();
            this.BuddyGridView = new System.Windows.Forms.DataGridView();
            this.PackageDownloaderTab = new System.Windows.Forms.TabPage();
            this.slotDropdown = new System.Windows.Forms.DomainUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.containerURITextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.subIDTextBox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.EmailContentTextBox = new System.Windows.Forms.RichTextBox();
            this.MainTab.SuspendLayout();
            this.GroupCreationTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.CryptoTab.SuspendLayout();
            this.ResendVerificationSMSTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PendingVerificationDataGridView)).BeginInit();
            this.SendEmailTab.SuspendLayout();
            this.ProfileSearchTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileGridView)).BeginInit();
            this.BuddySearchTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BuddyGridView)).BeginInit();
            this.PackageDownloaderTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.GroupCreationTab);
            this.MainTab.Controls.Add(this.CryptoTab);
            this.MainTab.Controls.Add(this.ResendVerificationSMSTab);
            this.MainTab.Controls.Add(this.SendEmailTab);
            this.MainTab.Controls.Add(this.ProfileSearchTab);
            this.MainTab.Controls.Add(this.BuddySearchTab);
            this.MainTab.Location = new System.Drawing.Point(7, 9);
            this.MainTab.Margin = new System.Windows.Forms.Padding(2);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(713, 562);
            this.MainTab.TabIndex = 4;
            // 
            // GroupCreationTab
            // 
            this.GroupCreationTab.Controls.Add(this.groupBox1);
            this.GroupCreationTab.Controls.Add(this.btnShapeFile);
            this.GroupCreationTab.Controls.Add(this.btnCreateGroup);
            this.GroupCreationTab.Controls.Add(this.cmbParentGroupName);
            this.GroupCreationTab.Controls.Add(this.chkIsActive);
            this.GroupCreationTab.Controls.Add(this.txtGroupKey);
            this.GroupCreationTab.Controls.Add(this.chkAllowGroupManagemen);
            this.GroupCreationTab.Controls.Add(this.chkShowIncidents);
            this.GroupCreationTab.Controls.Add(this.chkNotifySubgroups);
            this.GroupCreationTab.Controls.Add(this.txtShapeFile);
            this.GroupCreationTab.Controls.Add(this.txtLiveAuthID);
            this.GroupCreationTab.Controls.Add(this.txtliveUserID);
            this.GroupCreationTab.Controls.Add(this.txtEnrollmentKey);
            this.GroupCreationTab.Controls.Add(this.rEnrollmentType3);
            this.GroupCreationTab.Controls.Add(this.rEnrollmentType2);
            this.GroupCreationTab.Controls.Add(this.rEnrollmentType1);
            this.GroupCreationTab.Controls.Add(this.txtGeoLocation);
            this.GroupCreationTab.Controls.Add(this.txtGroupLocation);
            this.GroupCreationTab.Controls.Add(this.txtPhoneNumber);
            this.GroupCreationTab.Controls.Add(this.txtEmail);
            this.GroupCreationTab.Controls.Add(this.lblSetting);
            this.GroupCreationTab.Controls.Add(this.lblCircleKey);
            this.GroupCreationTab.Controls.Add(this.lblShapeFiles);
            this.GroupCreationTab.Controls.Add(this.lblLiveAuthID);
            this.GroupCreationTab.Controls.Add(this.lblLiveUserID);
            this.GroupCreationTab.Controls.Add(this.lblDomainKey);
            this.GroupCreationTab.Controls.Add(this.lblGroupType);
            this.GroupCreationTab.Controls.Add(this.lblAuthenticationType);
            this.GroupCreationTab.Controls.Add(this.lblFocusLatLong);
            this.GroupCreationTab.Controls.Add(this.lblLocation);
            this.GroupCreationTab.Controls.Add(this.lblPhoneNumber);
            this.GroupCreationTab.Controls.Add(this.lblEmail);
            this.GroupCreationTab.Controls.Add(this.lblParentGroupName);
            this.GroupCreationTab.Controls.Add(this.txtGroupName);
            this.GroupCreationTab.Controls.Add(this.lblGroupName);
            this.GroupCreationTab.Location = new System.Drawing.Point(4, 22);
            this.GroupCreationTab.Name = "GroupCreationTab";
            this.GroupCreationTab.Size = new System.Drawing.Size(705, 536);
            this.GroupCreationTab.TabIndex = 5;
            this.GroupCreationTab.Text = "GroupCreation";
            this.GroupCreationTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rGroupType1);
            this.groupBox1.Controls.Add(this.rGroupType2);
            this.groupBox1.Controls.Add(this.rGroupType3);
            this.groupBox1.Location = new System.Drawing.Point(176, 202);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 46);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GroupType";
            // 
            // rGroupType1
            // 
            this.rGroupType1.AutoSize = true;
            this.rGroupType1.Location = new System.Drawing.Point(6, 25);
            this.rGroupType1.Name = "rGroupType1";
            this.rGroupType1.Size = new System.Drawing.Size(58, 17);
            this.rGroupType1.TabIndex = 18;
            this.rGroupType1.Text = "Private";
            this.rGroupType1.UseVisualStyleBackColor = true;
            // 
            // rGroupType2
            // 
            this.rGroupType2.AutoSize = true;
            this.rGroupType2.Location = new System.Drawing.Point(71, 27);
            this.rGroupType2.Name = "rGroupType2";
            this.rGroupType2.Size = new System.Drawing.Size(54, 17);
            this.rGroupType2.TabIndex = 19;
            this.rGroupType2.Text = "Public";
            this.rGroupType2.UseVisualStyleBackColor = true;
            // 
            // rGroupType3
            // 
            this.rGroupType3.AutoSize = true;
            this.rGroupType3.Location = new System.Drawing.Point(160, 26);
            this.rGroupType3.Name = "rGroupType3";
            this.rGroupType3.Size = new System.Drawing.Size(54, 17);
            this.rGroupType3.TabIndex = 20;
            this.rGroupType3.Text = "Social";
            this.rGroupType3.UseVisualStyleBackColor = true;
            // 
            // btnShapeFile
            // 
            this.btnShapeFile.Location = new System.Drawing.Point(312, 382);
            this.btnShapeFile.Name = "btnShapeFile";
            this.btnShapeFile.Size = new System.Drawing.Size(120, 23);
            this.btnShapeFile.TabIndex = 35;
            this.btnShapeFile.Text = "Browse";
            this.btnShapeFile.UseVisualStyleBackColor = true;
            this.btnShapeFile.Click += new System.EventHandler(this.btnShapeFile_Click);
            // 
            // btnCreateGroup
            // 
            this.btnCreateGroup.Location = new System.Drawing.Point(175, 494);
            this.btnCreateGroup.Name = "btnCreateGroup";
            this.btnCreateGroup.Size = new System.Drawing.Size(170, 23);
            this.btnCreateGroup.TabIndex = 34;
            this.btnCreateGroup.Text = "CreateGroup";
            this.btnCreateGroup.UseVisualStyleBackColor = true;
            this.btnCreateGroup.Click += new System.EventHandler(this.btnCreateGroup_Click);
            // 
            // cmbParentGroupName
            // 
            this.cmbParentGroupName.FormattingEnabled = true;
            this.cmbParentGroupName.Location = new System.Drawing.Point(181, 62);
            this.cmbParentGroupName.Name = "cmbParentGroupName";
            this.cmbParentGroupName.Size = new System.Drawing.Size(121, 21);
            this.cmbParentGroupName.TabIndex = 33;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(557, 456);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(64, 17);
            this.chkIsActive.TabIndex = 32;
            this.chkIsActive.Text = "IsActive";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtGroupKey
            // 
            this.txtGroupKey.Location = new System.Drawing.Point(181, 418);
            this.txtGroupKey.Name = "txtGroupKey";
            this.txtGroupKey.Size = new System.Drawing.Size(100, 20);
            this.txtGroupKey.TabIndex = 31;
            // 
            // chkAllowGroupManagemen
            // 
            this.chkAllowGroupManagemen.AutoSize = true;
            this.chkAllowGroupManagemen.Location = new System.Drawing.Point(295, 456);
            this.chkAllowGroupManagemen.Name = "chkAllowGroupManagemen";
            this.chkAllowGroupManagemen.Size = new System.Drawing.Size(142, 17);
            this.chkAllowGroupManagemen.TabIndex = 30;
            this.chkAllowGroupManagemen.Text = "AllowGroupManagement";
            this.chkAllowGroupManagemen.UseVisualStyleBackColor = true;
            // 
            // chkShowIncidents
            // 
            this.chkShowIncidents.AutoSize = true;
            this.chkShowIncidents.Location = new System.Drawing.Point(443, 456);
            this.chkShowIncidents.Name = "chkShowIncidents";
            this.chkShowIncidents.Size = new System.Drawing.Size(96, 17);
            this.chkShowIncidents.TabIndex = 29;
            this.chkShowIncidents.Text = "ShowIncidents";
            this.chkShowIncidents.UseVisualStyleBackColor = true;
            // 
            // chkNotifySubgroups
            // 
            this.chkNotifySubgroups.AutoSize = true;
            this.chkNotifySubgroups.Location = new System.Drawing.Point(181, 455);
            this.chkNotifySubgroups.Name = "chkNotifySubgroups";
            this.chkNotifySubgroups.Size = new System.Drawing.Size(104, 17);
            this.chkNotifySubgroups.TabIndex = 28;
            this.chkNotifySubgroups.Text = "NotifySubgroups";
            this.chkNotifySubgroups.UseVisualStyleBackColor = true;
            // 
            // txtShapeFile
            // 
            this.txtShapeFile.Location = new System.Drawing.Point(181, 380);
            this.txtShapeFile.Name = "txtShapeFile";
            this.txtShapeFile.Size = new System.Drawing.Size(100, 20);
            this.txtShapeFile.TabIndex = 27;
            // 
            // txtLiveAuthID
            // 
            this.txtLiveAuthID.Location = new System.Drawing.Point(181, 352);
            this.txtLiveAuthID.Name = "txtLiveAuthID";
            this.txtLiveAuthID.Size = new System.Drawing.Size(100, 20);
            this.txtLiveAuthID.TabIndex = 26;
            // 
            // txtliveUserID
            // 
            this.txtliveUserID.Location = new System.Drawing.Point(181, 319);
            this.txtliveUserID.Name = "txtliveUserID";
            this.txtliveUserID.Size = new System.Drawing.Size(100, 20);
            this.txtliveUserID.TabIndex = 25;
            // 
            // txtEnrollmentKey
            // 
            this.txtEnrollmentKey.Location = new System.Drawing.Point(181, 283);
            this.txtEnrollmentKey.Name = "txtEnrollmentKey";
            this.txtEnrollmentKey.Size = new System.Drawing.Size(100, 20);
            this.txtEnrollmentKey.TabIndex = 24;
            // 
            // rEnrollmentType3
            // 
            this.rEnrollmentType3.AutoSize = true;
            this.rEnrollmentType3.Location = new System.Drawing.Point(378, 258);
            this.rEnrollmentType3.Name = "rEnrollmentType3";
            this.rEnrollmentType3.Size = new System.Drawing.Size(73, 17);
            this.rEnrollmentType3.TabIndex = 23;
            this.rEnrollmentType3.Text = "Moderator";
            this.rEnrollmentType3.UseVisualStyleBackColor = true;
            // 
            // rEnrollmentType2
            // 
            this.rEnrollmentType2.AutoSize = true;
            this.rEnrollmentType2.Location = new System.Drawing.Point(272, 258);
            this.rEnrollmentType2.Name = "rEnrollmentType2";
            this.rEnrollmentType2.Size = new System.Drawing.Size(73, 17);
            this.rEnrollmentType2.TabIndex = 22;
            this.rEnrollmentType2.Text = "Email/Key";
            this.rEnrollmentType2.UseVisualStyleBackColor = true;
            // 
            // rEnrollmentType1
            // 
            this.rEnrollmentType1.AutoSize = true;
            this.rEnrollmentType1.Location = new System.Drawing.Point(181, 260);
            this.rEnrollmentType1.Name = "rEnrollmentType1";
            this.rEnrollmentType1.Size = new System.Drawing.Size(51, 17);
            this.rEnrollmentType1.TabIndex = 21;
            this.rEnrollmentType1.Text = "None";
            this.rEnrollmentType1.UseVisualStyleBackColor = true;
            // 
            // txtGeoLocation
            // 
            this.txtGeoLocation.Location = new System.Drawing.Point(181, 172);
            this.txtGeoLocation.Name = "txtGeoLocation";
            this.txtGeoLocation.Size = new System.Drawing.Size(100, 20);
            this.txtGeoLocation.TabIndex = 17;
            // 
            // txtGroupLocation
            // 
            this.txtGroupLocation.Location = new System.Drawing.Point(181, 146);
            this.txtGroupLocation.Name = "txtGroupLocation";
            this.txtGroupLocation.Size = new System.Drawing.Size(100, 20);
            this.txtGroupLocation.TabIndex = 16;
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(181, 120);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(100, 20);
            this.txtPhoneNumber.TabIndex = 16;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(181, 94);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 20);
            this.txtEmail.TabIndex = 15;
            // 
            // lblSetting
            // 
            this.lblSetting.AutoSize = true;
            this.lblSetting.Location = new System.Drawing.Point(33, 456);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(40, 13);
            this.lblSetting.TabIndex = 14;
            this.lblSetting.Text = "Setting";
            // 
            // lblCircleKey
            // 
            this.lblCircleKey.AutoSize = true;
            this.lblCircleKey.Location = new System.Drawing.Point(33, 421);
            this.lblCircleKey.Name = "lblCircleKey";
            this.lblCircleKey.Size = new System.Drawing.Size(54, 13);
            this.lblCircleKey.TabIndex = 13;
            this.lblCircleKey.Text = "Circle Key";
            // 
            // lblShapeFiles
            // 
            this.lblShapeFiles.AutoSize = true;
            this.lblShapeFiles.Location = new System.Drawing.Point(33, 387);
            this.lblShapeFiles.Name = "lblShapeFiles";
            this.lblShapeFiles.Size = new System.Drawing.Size(62, 13);
            this.lblShapeFiles.TabIndex = 12;
            this.lblShapeFiles.Text = "Shape Files";
            // 
            // lblLiveAuthID
            // 
            this.lblLiveAuthID.AutoSize = true;
            this.lblLiveAuthID.Location = new System.Drawing.Point(33, 352);
            this.lblLiveAuthID.Name = "lblLiveAuthID";
            this.lblLiveAuthID.Size = new System.Drawing.Size(63, 13);
            this.lblLiveAuthID.TabIndex = 11;
            this.lblLiveAuthID.Text = "Live AuthID";
            // 
            // lblLiveUserID
            // 
            this.lblLiveUserID.AutoSize = true;
            this.lblLiveUserID.Location = new System.Drawing.Point(33, 322);
            this.lblLiveUserID.Name = "lblLiveUserID";
            this.lblLiveUserID.Size = new System.Drawing.Size(63, 13);
            this.lblLiveUserID.TabIndex = 10;
            this.lblLiveUserID.Text = "Live UserID";
            // 
            // lblDomainKey
            // 
            this.lblDomainKey.AutoSize = true;
            this.lblDomainKey.Location = new System.Drawing.Point(33, 285);
            this.lblDomainKey.Name = "lblDomainKey";
            this.lblDomainKey.Size = new System.Drawing.Size(61, 13);
            this.lblDomainKey.TabIndex = 9;
            this.lblDomainKey.Text = "DomainKey";
            // 
            // lblGroupType
            // 
            this.lblGroupType.AutoSize = true;
            this.lblGroupType.Location = new System.Drawing.Point(33, 212);
            this.lblGroupType.Name = "lblGroupType";
            this.lblGroupType.Size = new System.Drawing.Size(63, 13);
            this.lblGroupType.TabIndex = 8;
            this.lblGroupType.Text = "Group Type";
            // 
            // lblAuthenticationType
            // 
            this.lblAuthenticationType.AutoSize = true;
            this.lblAuthenticationType.Location = new System.Drawing.Point(33, 258);
            this.lblAuthenticationType.Name = "lblAuthenticationType";
            this.lblAuthenticationType.Size = new System.Drawing.Size(102, 13);
            this.lblAuthenticationType.TabIndex = 7;
            this.lblAuthenticationType.Text = "Authentication Type";
            // 
            // lblFocusLatLong
            // 
            this.lblFocusLatLong.AutoSize = true;
            this.lblFocusLatLong.Location = new System.Drawing.Point(33, 169);
            this.lblFocusLatLong.Name = "lblFocusLatLong";
            this.lblFocusLatLong.Size = new System.Drawing.Size(75, 13);
            this.lblFocusLatLong.TabIndex = 6;
            this.lblFocusLatLong.Text = "FocusLatLong";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(33, 145);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(48, 13);
            this.lblLocation.TabIndex = 5;
            this.lblLocation.Text = "Location";
            // 
            // lblPhoneNumber
            // 
            this.lblPhoneNumber.AutoSize = true;
            this.lblPhoneNumber.Location = new System.Drawing.Point(33, 116);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(81, 13);
            this.lblPhoneNumber.TabIndex = 4;
            this.lblPhoneNumber.Text = "Phone Number:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(33, 94);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Email";
            // 
            // lblParentGroupName
            // 
            this.lblParentGroupName.AutoSize = true;
            this.lblParentGroupName.Location = new System.Drawing.Point(33, 62);
            this.lblParentGroupName.Name = "lblParentGroupName";
            this.lblParentGroupName.Size = new System.Drawing.Size(101, 13);
            this.lblParentGroupName.TabIndex = 2;
            this.lblParentGroupName.Text = "Parent Group Name";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(181, 30);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(100, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(33, 29);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(67, 13);
            this.lblGroupName.TabIndex = 0;
            this.lblGroupName.Text = "Group Name";
            // 
            // CryptoTab
            // 
            this.CryptoTab.Controls.Add(this.ResultTextBox);
            this.CryptoTab.Controls.Add(this.DecryptButton);
            this.CryptoTab.Controls.Add(this.label3);
            this.CryptoTab.Controls.Add(this.CryptoTextBox);
            this.CryptoTab.Controls.Add(this.EnterTextLabel);
            this.CryptoTab.Controls.Add(this.EncryptButton);
            this.CryptoTab.Location = new System.Drawing.Point(4, 22);
            this.CryptoTab.Margin = new System.Windows.Forms.Padding(2);
            this.CryptoTab.Name = "CryptoTab";
            this.CryptoTab.Padding = new System.Windows.Forms.Padding(2);
            this.CryptoTab.Size = new System.Drawing.Size(705, 536);
            this.CryptoTab.TabIndex = 3;
            this.CryptoTab.Text = "Encrypt/ Decrypt";
            this.CryptoTab.UseVisualStyleBackColor = true;
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Location = new System.Drawing.Point(84, 84);
            this.ResultTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ReadOnly = true;
            this.ResultTextBox.Size = new System.Drawing.Size(608, 20);
            this.ResultTextBox.TabIndex = 6;
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(327, 140);
            this.DecryptButton.Margin = new System.Windows.Forms.Padding(2);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(56, 24);
            this.DecryptButton.TabIndex = 4;
            this.DecryptButton.Text = "Decrypt";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Result:";
            // 
            // CryptoTextBox
            // 
            this.CryptoTextBox.Location = new System.Drawing.Point(84, 34);
            this.CryptoTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CryptoTextBox.Name = "CryptoTextBox";
            this.CryptoTextBox.Size = new System.Drawing.Size(608, 20);
            this.CryptoTextBox.TabIndex = 2;
            // 
            // EnterTextLabel
            // 
            this.EnterTextLabel.AutoSize = true;
            this.EnterTextLabel.Location = new System.Drawing.Point(27, 37);
            this.EnterTextLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EnterTextLabel.Name = "EnterTextLabel";
            this.EnterTextLabel.Size = new System.Drawing.Size(59, 13);
            this.EnterTextLabel.TabIndex = 1;
            this.EnterTextLabel.Text = "Enter Text:";
            // 
            // EncryptButton
            // 
            this.EncryptButton.Location = new System.Drawing.Point(227, 138);
            this.EncryptButton.Margin = new System.Windows.Forms.Padding(2);
            this.EncryptButton.Name = "EncryptButton";
            this.EncryptButton.Size = new System.Drawing.Size(56, 25);
            this.EncryptButton.TabIndex = 0;
            this.EncryptButton.Text = "Encrypt";
            this.EncryptButton.UseVisualStyleBackColor = true;
            this.EncryptButton.Click += new System.EventHandler(this.EncryptButton_Click);
            // 
            // ResendVerificationSMSTab
            // 
            this.ResendVerificationSMSTab.Controls.Add(this.securityCodePhoneTextBox);
            this.ResendVerificationSMSTab.Controls.Add(this.label8);
            this.ResendVerificationSMSTab.Controls.Add(this.GetSecurityCodeButton);
            this.ResendVerificationSMSTab.Controls.Add(this.DaysTextBox);
            this.ResendVerificationSMSTab.Controls.Add(this.label2);
            this.ResendVerificationSMSTab.Controls.Add(this.GetPendingVerificationsButton);
            this.ResendVerificationSMSTab.Controls.Add(this.ResendSMSToAllButton);
            this.ResendVerificationSMSTab.Controls.Add(this.PendingVerificationDataGridView);
            this.ResendVerificationSMSTab.Location = new System.Drawing.Point(4, 22);
            this.ResendVerificationSMSTab.Margin = new System.Windows.Forms.Padding(2);
            this.ResendVerificationSMSTab.Name = "ResendVerificationSMSTab";
            this.ResendVerificationSMSTab.Size = new System.Drawing.Size(705, 536);
            this.ResendVerificationSMSTab.TabIndex = 2;
            this.ResendVerificationSMSTab.Text = "Resend Verification SMS";
            this.ResendVerificationSMSTab.UseVisualStyleBackColor = true;
            // 
            // securityCodePhoneTextBox
            // 
            this.securityCodePhoneTextBox.Location = new System.Drawing.Point(88, 55);
            this.securityCodePhoneTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.securityCodePhoneTextBox.Name = "securityCodePhoneTextBox";
            this.securityCodePhoneTextBox.Size = new System.Drawing.Size(130, 20);
            this.securityCodePhoneTextBox.TabIndex = 13;
            this.securityCodePhoneTextBox.Text = "+91";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 56);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Phone Number:";
            // 
            // GetSecurityCodeButton
            // 
            this.GetSecurityCodeButton.Location = new System.Drawing.Point(230, 50);
            this.GetSecurityCodeButton.Margin = new System.Windows.Forms.Padding(2);
            this.GetSecurityCodeButton.Name = "GetSecurityCodeButton";
            this.GetSecurityCodeButton.Size = new System.Drawing.Size(186, 27);
            this.GetSecurityCodeButton.TabIndex = 11;
            this.GetSecurityCodeButton.Text = "Get Security Code";
            this.GetSecurityCodeButton.UseVisualStyleBackColor = true;
            this.GetSecurityCodeButton.Click += new System.EventHandler(this.GetSecurityCodeButton_Click);
            // 
            // DaysTextBox
            // 
            this.DaysTextBox.Location = new System.Drawing.Point(58, 22);
            this.DaysTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.DaysTextBox.Name = "DaysTextBox";
            this.DaysTextBox.Size = new System.Drawing.Size(76, 20);
            this.DaysTextBox.TabIndex = 10;
            this.DaysTextBox.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Days:";
            // 
            // GetPendingVerificationsButton
            // 
            this.GetPendingVerificationsButton.Location = new System.Drawing.Point(138, 18);
            this.GetPendingVerificationsButton.Margin = new System.Windows.Forms.Padding(2);
            this.GetPendingVerificationsButton.Name = "GetPendingVerificationsButton";
            this.GetPendingVerificationsButton.Size = new System.Drawing.Size(186, 27);
            this.GetPendingVerificationsButton.TabIndex = 8;
            this.GetPendingVerificationsButton.Text = "Get Pending Verification Users";
            this.GetPendingVerificationsButton.UseVisualStyleBackColor = true;
            this.GetPendingVerificationsButton.Click += new System.EventHandler(this.GetPendingVerificationsButton_Click);
            // 
            // ResendSMSToAllButton
            // 
            this.ResendSMSToAllButton.Location = new System.Drawing.Point(338, 18);
            this.ResendSMSToAllButton.Margin = new System.Windows.Forms.Padding(2);
            this.ResendSMSToAllButton.Name = "ResendSMSToAllButton";
            this.ResendSMSToAllButton.Size = new System.Drawing.Size(186, 27);
            this.ResendSMSToAllButton.TabIndex = 7;
            this.ResendSMSToAllButton.Text = "Resend SMS";
            this.ResendSMSToAllButton.UseVisualStyleBackColor = true;
            this.ResendSMSToAllButton.Click += new System.EventHandler(this.ResendSMSToAllButton_Click);
            // 
            // PendingVerificationDataGridView
            // 
            this.PendingVerificationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PendingVerificationDataGridView.Location = new System.Drawing.Point(2, 78);
            this.PendingVerificationDataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.PendingVerificationDataGridView.Name = "PendingVerificationDataGridView";
            this.PendingVerificationDataGridView.RowTemplate.Height = 24;
            this.PendingVerificationDataGridView.Size = new System.Drawing.Size(703, 455);
            this.PendingVerificationDataGridView.TabIndex = 5;
            // 
            // SendEmailTab
            // 
            this.SendEmailTab.Controls.Add(this.EmailContentTextBox);
            this.SendEmailTab.Controls.Add(this.label9);
            this.SendEmailTab.Controls.Add(this.SubjectTextBox);
            this.SendEmailTab.Controls.Add(this.SendEmail);
            this.SendEmailTab.Location = new System.Drawing.Point(4, 22);
            this.SendEmailTab.Name = "SendEmailTab";
            this.SendEmailTab.Size = new System.Drawing.Size(705, 536);
            this.SendEmailTab.TabIndex = 6;
            this.SendEmailTab.Text = "Send Email";
            this.SendEmailTab.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Subject:";
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.Location = new System.Drawing.Point(53, 16);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(649, 20);
            this.SubjectTextBox.TabIndex = 2;
            // 
            // SendEmail
            // 
            this.SendEmail.Location = new System.Drawing.Point(251, 479);
            this.SendEmail.Name = "SendEmail";
            this.SendEmail.Size = new System.Drawing.Size(181, 23);
            this.SendEmail.TabIndex = 1;
            this.SendEmail.Text = "Send Email to All Profile Users";
            this.SendEmail.UseVisualStyleBackColor = true;
            this.SendEmail.Click += new System.EventHandler(this.SendEmail_Click);
            // 
            // ProfileSearchTab
            // 
            this.ProfileSearchTab.Controls.Add(this.label1);
            this.ProfileSearchTab.Controls.Add(this.RegionCodeTextBox);
            this.ProfileSearchTab.Controls.Add(this.GetUsersByRegionButton);
            this.ProfileSearchTab.Controls.Add(this.GetAllUsersOutsideOfIndiaButton);
            this.ProfileSearchTab.Controls.Add(this.ProfileGridView);
            this.ProfileSearchTab.Location = new System.Drawing.Point(4, 22);
            this.ProfileSearchTab.Margin = new System.Windows.Forms.Padding(2);
            this.ProfileSearchTab.Name = "ProfileSearchTab";
            this.ProfileSearchTab.Padding = new System.Windows.Forms.Padding(2);
            this.ProfileSearchTab.Size = new System.Drawing.Size(705, 536);
            this.ProfileSearchTab.TabIndex = 0;
            this.ProfileSearchTab.Text = "Profile Search";
            this.ProfileSearchTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Enter Country Code:";
            // 
            // RegionCodeTextBox
            // 
            this.RegionCodeTextBox.Location = new System.Drawing.Point(111, 22);
            this.RegionCodeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.RegionCodeTextBox.Name = "RegionCodeTextBox";
            this.RegionCodeTextBox.Size = new System.Drawing.Size(91, 20);
            this.RegionCodeTextBox.TabIndex = 7;
            // 
            // GetUsersByRegionButton
            // 
            this.GetUsersByRegionButton.Location = new System.Drawing.Point(206, 17);
            this.GetUsersByRegionButton.Margin = new System.Windows.Forms.Padding(2);
            this.GetUsersByRegionButton.Name = "GetUsersByRegionButton";
            this.GetUsersByRegionButton.Size = new System.Drawing.Size(176, 27);
            this.GetUsersByRegionButton.TabIndex = 6;
            this.GetUsersByRegionButton.Text = "Get Users by Country";
            this.GetUsersByRegionButton.UseVisualStyleBackColor = true;
            this.GetUsersByRegionButton.Click += new System.EventHandler(this.GetUsersByRegionButton_Click);
            // 
            // GetAllUsersOutsideOfIndiaButton
            // 
            this.GetAllUsersOutsideOfIndiaButton.Location = new System.Drawing.Point(519, 17);
            this.GetAllUsersOutsideOfIndiaButton.Margin = new System.Windows.Forms.Padding(2);
            this.GetAllUsersOutsideOfIndiaButton.Name = "GetAllUsersOutsideOfIndiaButton";
            this.GetAllUsersOutsideOfIndiaButton.Size = new System.Drawing.Size(186, 27);
            this.GetAllUsersOutsideOfIndiaButton.TabIndex = 5;
            this.GetAllUsersOutsideOfIndiaButton.Text = "Get All Users Outside of India";
            this.GetAllUsersOutsideOfIndiaButton.UseVisualStyleBackColor = true;
            this.GetAllUsersOutsideOfIndiaButton.Click += new System.EventHandler(this.GetAllUsersOutsideOfIndiaButton_Click);
            // 
            // ProfileGridView
            // 
            this.ProfileGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProfileGridView.Location = new System.Drawing.Point(-3, 58);
            this.ProfileGridView.Margin = new System.Windows.Forms.Padding(2);
            this.ProfileGridView.Name = "ProfileGridView";
            this.ProfileGridView.RowTemplate.Height = 24;
            this.ProfileGridView.Size = new System.Drawing.Size(713, 476);
            this.ProfileGridView.TabIndex = 4;
            // 
            // BuddySearchTab
            // 
            this.BuddySearchTab.Controls.Add(this.BuddyOutsideIndiaButton);
            this.BuddySearchTab.Controls.Add(this.BuddyGridView);
            this.BuddySearchTab.Location = new System.Drawing.Point(4, 22);
            this.BuddySearchTab.Margin = new System.Windows.Forms.Padding(2);
            this.BuddySearchTab.Name = "BuddySearchTab";
            this.BuddySearchTab.Padding = new System.Windows.Forms.Padding(2);
            this.BuddySearchTab.Size = new System.Drawing.Size(705, 536);
            this.BuddySearchTab.TabIndex = 1;
            this.BuddySearchTab.Text = "Buddy Search";
            this.BuddySearchTab.UseVisualStyleBackColor = true;
            // 
            // BuddyOutsideIndiaButton
            // 
            this.BuddyOutsideIndiaButton.Location = new System.Drawing.Point(517, 16);
            this.BuddyOutsideIndiaButton.Margin = new System.Windows.Forms.Padding(2);
            this.BuddyOutsideIndiaButton.Name = "BuddyOutsideIndiaButton";
            this.BuddyOutsideIndiaButton.Size = new System.Drawing.Size(186, 27);
            this.BuddyOutsideIndiaButton.TabIndex = 6;
            this.BuddyOutsideIndiaButton.Text = "Get All Buddies Outside of India";
            this.BuddyOutsideIndiaButton.UseVisualStyleBackColor = true;
            this.BuddyOutsideIndiaButton.Click += new System.EventHandler(this.BuddyOutsideIndiaButton_Click);
            // 
            // BuddyGridView
            // 
            this.BuddyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BuddyGridView.Location = new System.Drawing.Point(4, 58);
            this.BuddyGridView.Margin = new System.Windows.Forms.Padding(2);
            this.BuddyGridView.Name = "BuddyGridView";
            this.BuddyGridView.RowTemplate.Height = 24;
            this.BuddyGridView.Size = new System.Drawing.Size(698, 476);
            this.BuddyGridView.TabIndex = 5;
            // 
            // PackageDownloaderTab
            // 
            this.PackageDownloaderTab.Controls.Add(this.slotDropdown);
            this.PackageDownloaderTab.Controls.Add(this.label7);
            this.PackageDownloaderTab.Controls.Add(this.label6);
            this.PackageDownloaderTab.Controls.Add(this.containerURITextBox);
            this.PackageDownloaderTab.Controls.Add(this.label5);
            this.PackageDownloaderTab.Controls.Add(this.NameTextBox);
            this.PackageDownloaderTab.Controls.Add(this.label4);
            this.PackageDownloaderTab.Controls.Add(this.downloadButton);
            this.PackageDownloaderTab.Controls.Add(this.subIDTextBox);
            this.PackageDownloaderTab.Location = new System.Drawing.Point(4, 22);
            this.PackageDownloaderTab.Margin = new System.Windows.Forms.Padding(2);
            this.PackageDownloaderTab.Name = "PackageDownloaderTab";
            this.PackageDownloaderTab.Padding = new System.Windows.Forms.Padding(2);
            this.PackageDownloaderTab.Size = new System.Drawing.Size(705, 536);
            this.PackageDownloaderTab.TabIndex = 4;
            this.PackageDownloaderTab.Text = "Package Downloader";
            this.PackageDownloaderTab.UseVisualStyleBackColor = true;
            // 
            // slotDropdown
            // 
            this.slotDropdown.Items.Add("Production");
            this.slotDropdown.Items.Add("Staging");
            this.slotDropdown.Location = new System.Drawing.Point(138, 106);
            this.slotDropdown.Margin = new System.Windows.Forms.Padding(2);
            this.slotDropdown.Name = "slotDropdown";
            this.slotDropdown.Size = new System.Drawing.Size(136, 20);
            this.slotDropdown.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 147);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Container URI";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 106);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Deployment Slot ";
            // 
            // containerURITextBox
            // 
            this.containerURITextBox.Location = new System.Drawing.Point(138, 147);
            this.containerURITextBox.Margin = new System.Windows.Forms.Padding(2);
            this.containerURITextBox.Name = "containerURITextBox";
            this.containerURITextBox.Size = new System.Drawing.Size(380, 20);
            this.containerURITextBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 68);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Cloud Service Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(138, 64);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(380, 20);
            this.NameTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 28);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Subscription ID";
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(231, 237);
            this.downloadButton.Margin = new System.Windows.Forms.Padding(2);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(142, 25);
            this.downloadButton.TabIndex = 4;
            this.downloadButton.Text = "Dowload Package";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // subIDTextBox
            // 
            this.subIDTextBox.Location = new System.Drawing.Point(138, 28);
            this.subIDTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.subIDTextBox.Name = "subIDTextBox";
            this.subIDTextBox.Size = new System.Drawing.Size(380, 20);
            this.subIDTextBox.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // EmailContentTextBox
            // 
            this.EmailContentTextBox.Location = new System.Drawing.Point(3, 54);
            this.EmailContentTextBox.Name = "EmailContentTextBox";
            this.EmailContentTextBox.Size = new System.Drawing.Size(699, 391);
            this.EmailContentTextBox.TabIndex = 4;
            this.EmailContentTextBox.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 583);
            this.Controls.Add(this.MainTab);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "OPs Tools";
            this.MainTab.ResumeLayout(false);
            this.GroupCreationTab.ResumeLayout(false);
            this.GroupCreationTab.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.CryptoTab.ResumeLayout(false);
            this.CryptoTab.PerformLayout();
            this.ResendVerificationSMSTab.ResumeLayout(false);
            this.ResendVerificationSMSTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PendingVerificationDataGridView)).EndInit();
            this.SendEmailTab.ResumeLayout(false);
            this.SendEmailTab.PerformLayout();
            this.ProfileSearchTab.ResumeLayout(false);
            this.ProfileSearchTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileGridView)).EndInit();
            this.BuddySearchTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BuddyGridView)).EndInit();
            this.PackageDownloaderTab.ResumeLayout(false);
            this.PackageDownloaderTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage ProfileSearchTab;
        private System.Windows.Forms.TextBox RegionCodeTextBox;
        private System.Windows.Forms.Button GetUsersByRegionButton;
        private System.Windows.Forms.Button GetAllUsersOutsideOfIndiaButton;
        private System.Windows.Forms.DataGridView ProfileGridView;
        private System.Windows.Forms.TabPage BuddySearchTab;
        private System.Windows.Forms.TabPage ResendVerificationSMSTab;
        private System.Windows.Forms.TabPage GroupCreationTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView BuddyGridView;
        private System.Windows.Forms.Button ResendSMSToAllButton;
        private System.Windows.Forms.DataGridView PendingVerificationDataGridView;
        private System.Windows.Forms.Button BuddyOutsideIndiaButton;
        private System.Windows.Forms.TabPage CryptoTab;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CryptoTextBox;
        private System.Windows.Forms.Label EnterTextLabel;
        private System.Windows.Forms.Button EncryptButton;
        private System.Windows.Forms.TextBox DaysTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GetPendingVerificationsButton;
        private System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.TabPage PackageDownloaderTab;
        private System.Windows.Forms.DomainUpDown slotDropdown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox containerURITextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.TextBox subIDTextBox;
        private System.Windows.Forms.TabPage StorageMigration;
        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.TextBox securityCodePhoneTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GetSecurityCodeButton;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label lblDomainKey;
        private System.Windows.Forms.Label lblGroupType;
        private System.Windows.Forms.Label lblAuthenticationType;
        private System.Windows.Forms.Label lblFocusLatLong;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblParentGroupName;
        private System.Windows.Forms.Label lblLiveUserID;
        private System.Windows.Forms.Label lblLiveAuthID;
        private System.Windows.Forms.Label lblShapeFiles;
        private System.Windows.Forms.Label lblCircleKey;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.TextBox txtGroupLocation;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtGeoLocation;
        private System.Windows.Forms.RadioButton rGroupType3;
        private System.Windows.Forms.RadioButton rGroupType2;
        private System.Windows.Forms.RadioButton rGroupType1;
        private System.Windows.Forms.RadioButton rEnrollmentType3;
        private System.Windows.Forms.RadioButton rEnrollmentType2;
        private System.Windows.Forms.RadioButton rEnrollmentType1;
        private System.Windows.Forms.TextBox txtEnrollmentKey;
        private System.Windows.Forms.TextBox txtLiveAuthID;
        private System.Windows.Forms.TextBox txtliveUserID;
        private System.Windows.Forms.TextBox txtShapeFile;
        private System.Windows.Forms.CheckBox chkAllowGroupManagemen;
        private System.Windows.Forms.CheckBox chkShowIncidents;
        private System.Windows.Forms.CheckBox chkNotifySubgroups;
        private System.Windows.Forms.TextBox txtGroupKey;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.ComboBox cmbParentGroupName;
        private System.Windows.Forms.Button btnCreateGroup;
        private System.Windows.Forms.Button btnShapeFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage SendEmailTab;
        private System.Windows.Forms.Button SendEmail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SubjectTextBox;
        private System.Windows.Forms.RichTextBox EmailContentTextBox;
       

    }
}
