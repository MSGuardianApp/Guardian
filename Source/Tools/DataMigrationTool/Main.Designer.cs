namespace Tools
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
            this.StorageMigration = new System.Windows.Forms.TabPage();
            this.chkGroupAdmins = new System.Windows.Forms.CheckBox();
            this.chkGroup = new System.Windows.Forms.CheckBox();
            this.chkGroupMemberValidator = new System.Windows.Forms.CheckBox();
            this.chkTease = new System.Windows.Forms.CheckBox();
            this.chkHistoryGeoLocation = new System.Windows.Forms.CheckBox();
            this.chkPhoneValidation = new System.Windows.Forms.CheckBox();
            this.ALL = new System.Windows.Forms.CheckBox();
            this.pgbMigration = new System.Windows.Forms.ProgressBar();
            this.chkGroupMarshal = new System.Windows.Forms.CheckBox();
            this.chkBuddy = new System.Windows.Forms.CheckBox();
            this.chkGroupMembership = new System.Windows.Forms.CheckBox();
            this.chkProfile = new System.Windows.Forms.CheckBox();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.MainTab.SuspendLayout();
            this.StorageMigration.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.StorageMigration);
            this.MainTab.Location = new System.Drawing.Point(7, 9);
            this.MainTab.Margin = new System.Windows.Forms.Padding(2);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(713, 562);
            this.MainTab.TabIndex = 4;
            // 
            // StorageMigration
            // 
            this.StorageMigration.Controls.Add(this.chkGroupAdmins);
            this.StorageMigration.Controls.Add(this.chkGroup);
            this.StorageMigration.Controls.Add(this.chkGroupMemberValidator);
            this.StorageMigration.Controls.Add(this.chkTease);
            this.StorageMigration.Controls.Add(this.chkHistoryGeoLocation);
            this.StorageMigration.Controls.Add(this.chkPhoneValidation);
            this.StorageMigration.Controls.Add(this.ALL);
            this.StorageMigration.Controls.Add(this.pgbMigration);
            this.StorageMigration.Controls.Add(this.chkGroupMarshal);
            this.StorageMigration.Controls.Add(this.chkBuddy);
            this.StorageMigration.Controls.Add(this.chkGroupMembership);
            this.StorageMigration.Controls.Add(this.chkProfile);
            this.StorageMigration.Controls.Add(this.chkUser);
            this.StorageMigration.Controls.Add(this.btnMigrate);
            this.StorageMigration.Location = new System.Drawing.Point(4, 22);
            this.StorageMigration.Margin = new System.Windows.Forms.Padding(2);
            this.StorageMigration.Name = "StorageMigration";
            this.StorageMigration.Padding = new System.Windows.Forms.Padding(2);
            this.StorageMigration.Size = new System.Drawing.Size(705, 536);
            this.StorageMigration.TabIndex = 4;
            this.StorageMigration.Text = "Data Migration";
            this.StorageMigration.UseVisualStyleBackColor = true;
            // 
            // chkGroupAdmins
            // 
            this.chkGroupAdmins.AutoSize = true;
            this.chkGroupAdmins.Location = new System.Drawing.Point(341, 69);
            this.chkGroupAdmins.Name = "chkGroupAdmins";
            this.chkGroupAdmins.Size = new System.Drawing.Size(89, 17);
            this.chkGroupAdmins.TabIndex = 14;
            this.chkGroupAdmins.Text = "GroupAdmins";
            this.chkGroupAdmins.UseVisualStyleBackColor = true;
            // 
            // chkGroup
            // 
            this.chkGroup.AutoSize = true;
            this.chkGroup.Location = new System.Drawing.Point(341, 36);
            this.chkGroup.Name = "chkGroup";
            this.chkGroup.Size = new System.Drawing.Size(55, 17);
            this.chkGroup.TabIndex = 13;
            this.chkGroup.Text = "Group";
            this.chkGroup.UseVisualStyleBackColor = true;
            // 
            // chkGroupMemberValidator
            // 
            this.chkGroupMemberValidator.AutoSize = true;
            this.chkGroupMemberValidator.Location = new System.Drawing.Point(341, 139);
            this.chkGroupMemberValidator.Name = "chkGroupMemberValidator";
            this.chkGroupMemberValidator.Size = new System.Drawing.Size(134, 17);
            this.chkGroupMemberValidator.TabIndex = 12;
            this.chkGroupMemberValidator.Text = "GroupMemberValidator";
            this.chkGroupMemberValidator.UseVisualStyleBackColor = true;
            // 
            // chkTease
            // 
            this.chkTease.AutoSize = true;
            this.chkTease.Location = new System.Drawing.Point(341, 172);
            this.chkTease.Name = "chkTease";
            this.chkTease.Size = new System.Drawing.Size(56, 17);
            this.chkTease.TabIndex = 10;
            this.chkTease.Text = "Tease";
            this.chkTease.UseVisualStyleBackColor = true;
            // 
            // chkHistoryGeoLocation
            // 
            this.chkHistoryGeoLocation.AutoSize = true;
            this.chkHistoryGeoLocation.Location = new System.Drawing.Point(341, 209);
            this.chkHistoryGeoLocation.Name = "chkHistoryGeoLocation";
            this.chkHistoryGeoLocation.Size = new System.Drawing.Size(119, 17);
            this.chkHistoryGeoLocation.TabIndex = 9;
            this.chkHistoryGeoLocation.Text = "HistoryGeoLocation";
            this.chkHistoryGeoLocation.UseVisualStyleBackColor = true;
            // 
            // chkPhoneValidation
            // 
            this.chkPhoneValidation.AutoSize = true;
            this.chkPhoneValidation.Location = new System.Drawing.Point(341, 104);
            this.chkPhoneValidation.Name = "chkPhoneValidation";
            this.chkPhoneValidation.Size = new System.Drawing.Size(103, 17);
            this.chkPhoneValidation.TabIndex = 8;
            this.chkPhoneValidation.Text = "PhoneValidation";
            this.chkPhoneValidation.UseVisualStyleBackColor = true;
            // 
            // ALL
            // 
            this.ALL.AutoSize = true;
            this.ALL.Location = new System.Drawing.Point(222, 36);
            this.ALL.Name = "ALL";
            this.ALL.Size = new System.Drawing.Size(45, 17);
            this.ALL.TabIndex = 7;
            this.ALL.Text = "ALL";
            this.ALL.UseVisualStyleBackColor = true;
            // 
            // pgbMigration
            // 
            this.pgbMigration.Location = new System.Drawing.Point(205, 270);
            this.pgbMigration.Name = "pgbMigration";
            this.pgbMigration.Size = new System.Drawing.Size(239, 23);
            this.pgbMigration.TabIndex = 6;
            this.pgbMigration.Visible = false;
            // 
            // chkGroupMarshal
            // 
            this.chkGroupMarshal.AutoSize = true;
            this.chkGroupMarshal.Location = new System.Drawing.Point(222, 209);
            this.chkGroupMarshal.Name = "chkGroupMarshal";
            this.chkGroupMarshal.Size = new System.Drawing.Size(92, 17);
            this.chkGroupMarshal.TabIndex = 5;
            this.chkGroupMarshal.Text = "GroupMarshal";
            this.chkGroupMarshal.UseVisualStyleBackColor = true;
            // 
            // chkBuddy
            // 
            this.chkBuddy.AutoSize = true;
            this.chkBuddy.Location = new System.Drawing.Point(222, 137);
            this.chkBuddy.Name = "chkBuddy";
            this.chkBuddy.Size = new System.Drawing.Size(56, 17);
            this.chkBuddy.TabIndex = 4;
            this.chkBuddy.Text = "Buddy";
            this.chkBuddy.UseVisualStyleBackColor = true;
            // 
            // chkGroupMembership
            // 
            this.chkGroupMembership.AutoSize = true;
            this.chkGroupMembership.Location = new System.Drawing.Point(222, 172);
            this.chkGroupMembership.Name = "chkGroupMembership";
            this.chkGroupMembership.Size = new System.Drawing.Size(112, 17);
            this.chkGroupMembership.TabIndex = 3;
            this.chkGroupMembership.Text = "GroupMembership";
            this.chkGroupMembership.UseVisualStyleBackColor = true;
            // 
            // chkProfile
            // 
            this.chkProfile.AutoSize = true;
            this.chkProfile.Location = new System.Drawing.Point(222, 104);
            this.chkProfile.Name = "chkProfile";
            this.chkProfile.Size = new System.Drawing.Size(55, 17);
            this.chkProfile.TabIndex = 2;
            this.chkProfile.Text = "Profile";
            this.chkProfile.UseVisualStyleBackColor = true;
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.Location = new System.Drawing.Point(222, 69);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(48, 17);
            this.chkUser.TabIndex = 1;
            this.chkUser.Text = "User";
            this.chkUser.UseVisualStyleBackColor = true;
            // 
            // btnMigrate
            // 
            this.btnMigrate.Location = new System.Drawing.Point(243, 321);
            this.btnMigrate.Margin = new System.Windows.Forms.Padding(2);
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Size = new System.Drawing.Size(153, 48);
            this.btnMigrate.TabIndex = 0;
            this.btnMigrate.Text = "Migrate";
            this.btnMigrate.UseVisualStyleBackColor = true;
            this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 583);
            this.Controls.Add(this.MainTab);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "Data Migration Tool V1.3 -> V2.0";
            this.MainTab.ResumeLayout(false);
            this.StorageMigration.ResumeLayout(false);
            this.StorageMigration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;   
        private System.Windows.Forms.TabPage StorageMigration;
        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.TextBox securityCodePhoneTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GetSecurityCodeButton;
        private System.Windows.Forms.CheckBox chkBuddy;
        private System.Windows.Forms.CheckBox chkGroupMembership;
        private System.Windows.Forms.CheckBox chkProfile;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.CheckBox chkGroupMarshal;
        private System.Windows.Forms.ProgressBar pgbMigration;
        private System.Windows.Forms.CheckBox ALL;
        private System.Windows.Forms.CheckBox chkGroupMemberValidator;
        private System.Windows.Forms.CheckBox chkTease;
        private System.Windows.Forms.CheckBox chkHistoryGeoLocation;
        private System.Windows.Forms.CheckBox chkPhoneValidation;
        private System.Windows.Forms.CheckBox chkGroupAdmins;
        private System.Windows.Forms.CheckBox chkGroup;

    }
}

