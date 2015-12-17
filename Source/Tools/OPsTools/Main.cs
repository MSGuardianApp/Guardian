using SOS.AzureStorageAccessLayer;
using SOS.AzureStorageAccessLayer.Entities;
using SOS.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SOS.Model;
using SOS.AzureSQLAccessLayer;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using dataContracts = SOS.Service.Interfaces.DataContracts;
using SOS.Service.Implementation;
using SendGrid;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;

namespace SOS.OPsTools
{
    public partial class Main : Form
    {
        private const string DisplayName = "Guardian";
        private const string FromAddress = "guardianapp@outlook.com";

        private static string sendGridUserID;
        private static string sendGridPassword;

        private readonly ReportRepository _ReportRepository;
        private Random _Random;

        public GroupService groupService = null;
        List<dataContracts.OutBound.GroupDTO> groups = new List<dataContracts.OutBound.GroupDTO>();
        public Main()
        {
            InitializeComponent();

            PackageDownloaderTab.Visible = false;

            this.rEnrollmentType2.Checked = true;
            this.rGroupType1.Checked = true;
            groupService = new GroupService();
            _Random = new Random();

            dataContracts.OutBound.GroupDTO group = new dataContracts.OutBound.GroupDTO() { GroupName = "Select", GroupID = 0 };

            groups.Add(group);
            groups = groups.Concat(groupService.GetAllGroupWithAdmins()).ToList();

            cmbParentGroupName.DataSource = groups.Select(res => res.GroupName).ToList();
            cmbParentGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            sendGridUserID = ConfigurationManager.AppSettings["sendGridUserID"];
            sendGridPassword = ConfigurationManager.AppSettings["sendGridPassword"];
            _ReportRepository = new ReportRepository();
        }


        private void GetUsersByRegionButton_Click(object sender, EventArgs e)
        {
            List<Profile> profiles = (new MemberRepository()).GetAllProfilesByRegionCode(RegionCodeTextBox.Text.Trim());
            ProfileGridView.DataSource = profiles;
        }

        private void GetAllUsersOutsideOfIndiaButton_Click(object sender, EventArgs e)
        {
            List<Profile> profiles = (new MemberRepository()).GetAllProfilesOutsideIndia();
            ProfileGridView.DataSource = profiles;
        }

        private void BuddyOutsideIndiaButton_Click(object sender, EventArgs e)
        {
            List<Buddy> buddies = (new MemberRepository()).GetAllBuddiesOutsideIndia();
            BuddyGridView.DataSource = buddies;
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            ResultTextBox.Text = Security.Encrypt(CryptoTextBox.Text.Trim());
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            ResultTextBox.Text = Security.Decrypt(CryptoTextBox.Text.Trim());
        }

        private void GetPendingVerificationsButton_Click(object sender, EventArgs e)
        {
            List<PhoneValidation> profiles = (new MemberStorageAccess()).GetAllPendingVerificationProfiles(int.Parse(DaysTextBox.Text.Trim()));
            PendingVerificationDataGridView.DataSource = profiles;
        }

        class Notification
        {
            public string Number { get; set; }
            public string Message { get; set; }
        }
        private void ResendSMSToAllButton_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.No;
            //IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            if (DaysTextBox.Text.Trim() == string.Empty)
                result = MessageBox.Show("Please provide number of days", "Alert!");
            else
                result = MessageBox.Show("Are you sure you want to send messages to all the pending users for the past " + DaysTextBox.Text.Trim() + " days?", "Confirm!", MessageBoxButtons.YesNo);

            List<Notification> notifications = new List<Notification>();
            if (result == DialogResult.Yes)
            {
                List<PhoneValidation> profiles = (new MemberStorageAccess()).GetAllPendingVerificationProfiles(int.Parse(DaysTextBox.Text.Trim()));
                foreach (var prf in profiles)
                {
                    var number = Security.Decrypt(prf.PhoneNumber);
                    if (number.StartsWith("+91") && number.Length == 13 && (number[3] == '7' || number[3] == '8' || number[3] == '9'))
                    {
                        //if (MonthsTextBox.Text.Trim() != String.Empty && Convert.ToDateTime(prf.Timestamp, culture) < DateTime.Now.AddMonths(-1 * int.Parse(MonthsTextBox.Text)))
                        //    continue;
                        var name = prf.Name.Substring(0, prf.Name.Length > 9 ? 9 : prf.Name.Length);
                        name = name.Substring(0, name.IndexOf(' ') > 0 ? name.IndexOf(' ') : name.Length);
                        var message = string.Format("GuardianApp: Hi " + name + ", You had tried to register with us sometime back. Use {0} verification code to create your Profile for phone number ending with {1}", prf.SecurityToken, number.Substring(number.Length - 4, 4));

                        SOS.Service.Utility.SMS.SendSMS(number, message);

                        notifications.Add(new Notification() { Number = number, Message = message });
                    }
                }
                PendingVerificationDataGridView.DataSource = notifications;
            }
        }

        private void SendSMS()
        {
            SOS.Service.Utility.SMS.SendSMS("+919949091097", "GuardianApp: We have noticed, you had tried to register with us sometime back. Use 12345 verification code to create your Profile for phone ending with 1097");
        }

        #region Download packages from Azure Management Portal - Not working
        private void downloadButton_Click(object sender, EventArgs e)
        {
            DownloadPackage(subIDTextBox.Text, NameTextBox.Text, slotDropdown.Text, containerURITextBox.Text);
        }

        private static void DownloadPackage1(string subID, string serviceName, string slot, string containerUri)
        {
            string getPackageUri = string.Format("https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/{2}/package?containerUri={3}", subID, serviceName, slot, containerUri);

            using (MemoryStream ms = new MemoryStream())
            {
                WebClient wc = new WebClient();
                wc.Headers["x-ms-version"] = "2012-03-01 ";
                wc.Headers["ContentLength"] = "0";
                wc.Encoding = Encoding.UTF8;
                wc.UploadStringAsync(new Uri(getPackageUri), string.Empty);

            }
        }

        public static void DownloadPackage(string subID, string serviceName, string slot, string containerUri)
        {
            X509Certificate2 certificate = GetManagementCertificate("387A1F1090DB961DF3EC6668F68515A39686CD5F");

            //UriBuilder requestUri = new UriBuilder();
            //requestUri.Host = "https://management.core.windows.net";
            //requestUri.Path = string.Format(CultureInfo.InvariantCulture, "/{0}/services/hostedservices/{1}/deploymentslots/{2}/package", subID, serviceName, slot);
            //requestUri.Query = string.Format("containerUri={0}", HttpUtility.UrlEncode(containerUri));
            containerUri = HttpUtility.UrlEncode(containerUri);
            string getPackageUri = string.Format("https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/{2}/package?containerUri={3}", subID, serviceName, slot, containerUri);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getPackageUri);
            request.ContentLength = 0;
            request.Headers.Add("x-ms-version", "2012-03-01");
            //request.Headers.Add("ContentLength", "0");
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.ClientCertificates.Add(certificate);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {

                    }
                }
            }
        }

        private static X509Certificate2 GetManagementCertificate(string managementCertThumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, managementCertThumbprint, false);
            if (certificates.Count == 0)
            {
                throw new Exception(string.Format("Could not find certificate with thumbprint {0}", managementCertThumbprint));
            }

            return certificates[0];

        }

        private X509Certificate2 GetX509Certificate2(String managementCertThumbprint)
        {
            X509Certificate2 x509Certificate2 = null;
            X509Store store = new X509Store("My", StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection x509Certificate2Collection = store.Certificates.Find(X509FindType.FindByThumbprint, managementCertThumbprint, false);
                x509Certificate2 = x509Certificate2Collection[0];
            }
            finally
            {
                store.Close();
            }
            return x509Certificate2;
        }

        private HttpWebRequest CreateHttpWebRequest(Uri uri, String httpWebRequestMethod)
        {
            X509Certificate2 x509Certificate2 = GetX509Certificate2("387A1F1090DB961DF3EC6668F68515A39686CD5F");

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.Method = httpWebRequestMethod;
            httpWebRequest.Headers.Add("x-ms-version", "2013-03-01");
            httpWebRequest.ClientCertificates.Add(x509Certificate2);
            httpWebRequest.ContentType = "application/xml";

            return httpWebRequest;
        }
        #endregion


        private void GetSecurityCodeButton_Click(object sender, EventArgs e)
        {
            List<object> profiles = (new MemberStorageAccess()).GetSecurityTokenByNumber(securityCodePhoneTextBox.Text.Trim());
            PendingVerificationDataGridView.DataSource = profiles;
        }


        private void btnShapeFile_Click(object sender, EventArgs e)
        {
            // call will result a value from the DialogResult enum
            // when the dialog is dismissed.
            DialogResult result = this.openFileDialog1.ShowDialog();
            // if a file is selected
            if (result == DialogResult.OK)
            {
                // Set the selected file URL to the textbox
                this.txtShapeFile.Text = this.openFileDialog1.FileName;
            }
        }

        private void btnCreateGroup_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();

            dataContracts.Group grp = new dataContracts.Group();

            grp.GroupID = Convert.ToString(groups.Count);

            if (string.IsNullOrEmpty(txtGroupName.Text))
                sb.Append(" GroupName is blank ");
            else
                grp.GroupName = txtGroupName.Text;

            grp.ParentGroupID = cmbParentGroupName.SelectedIndex;

            if (string.IsNullOrEmpty(txtEmail.Text))
                sb.Append("Email is blank");
            else
                grp.Email = txtEmail.Text;

            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
                sb.Append(" PhoneNumber is blank ");
            else
                grp.PhoneNumber = txtPhoneNumber.Text;

            if (string.IsNullOrEmpty(txtEnrollmentKey.Text))
                sb.Append(" EnrollmentKey is blank ");
            else
                grp.EnrollmentKey = txtEnrollmentKey.Text;


            if (string.IsNullOrEmpty(txtShapeFile.Text))
                sb.Append(" ShapeFileID is blank ");
            else
                grp.ShapeFileID = txtShapeFile.Text;

            dataContracts.LiveCred liveCred = new dataContracts.LiveCred();

            if (string.IsNullOrEmpty(txtliveUserID.Text))
                sb.Append(" LiveID is blank ");
            else
                liveCred.LiveID = txtliveUserID.Text;


            if (string.IsNullOrEmpty(txtLiveAuthID.Text))
                sb.Append(" LiveAuthID is blank ");
            else
            {
                grp.LiveInfo = liveCred;
            }
            if (string.IsNullOrEmpty(txtGroupKey.Text))
                sb.Append(" SubGroupIdentificationKey is blank ");
            else
                grp.SubGroupIdentificationKey = txtGroupKey.Text;

            if (string.IsNullOrEmpty(txtEnrollmentKey.Text))
                sb.Append(" EnrollmentKey is blank ");
            else
                grp.EnrollmentKey = txtEnrollmentKey.Text;

            if (string.IsNullOrEmpty(txtGroupLocation.Text))
                sb.Append(" GroupLocation is blank ");
            else
                grp.GroupLocation = txtGroupLocation.Text;

            if (string.IsNullOrEmpty(txtGeoLocation.Text))
                sb.Append(" GeoLocation is blank ");
            else
                grp.GeoLocation = txtGeoLocation.Text;


            if (rEnrollmentType1.Checked) grp.EnrollmentType = dataContracts.Enrollment.None;
            if (rEnrollmentType2.Checked) grp.EnrollmentType = dataContracts.Enrollment.AutoOrgMail;
            if (rEnrollmentType3.Checked) grp.EnrollmentType = dataContracts.Enrollment.Moderator;

            if (rGroupType1.Checked) grp.Type = dataContracts.GroupType.Private;
            if (rGroupType2.Checked) grp.Type = dataContracts.GroupType.Public;
            if (rGroupType3.Checked) grp.Type = dataContracts.GroupType.Social;

            if (chkNotifySubgroups.Checked)
                grp.NotifySubgroups = true;
            else
                grp.NotifySubgroups = false;

            if (chkAllowGroupManagemen.Checked)
                grp.AllowGroupManagement = true;
            else
                grp.AllowGroupManagement = false;

            if (chkShowIncidents.Checked)
                grp.ShowIncidents = true;
            else
                grp.ShowIncidents = false;

            grp.IsActive = true;

            if (sb.Length > 0)
                MessageBox.Show(sb.ToString());
            else
            {
                groupService.EditGroup(grp, null, null, true);
                MessageBox.Show("Group has been created successfully");
            }
        }

        public static async Task<bool> SendEmailUsingSendGrid(string to, string subject, string body)
        {
            //create a new message object 
            var message = new SendGridMessage();

            message.AddTo(to);

            message.From = new MailAddress(FromAddress, DisplayName);
            message.Html = body;
            message.Subject = subject;

            //create an instance of the Web transport mechanism 
            var transportInstance = new Web(new NetworkCredential(sendGridUserID, sendGridPassword));

            //send the mail 
            await transportInstance.DeliverAsync(message);
            return true;
        }

        private async void SendEmail_Click(object sender, EventArgs e)
        {
            string subject = SubjectTextBox.Text;

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(EmailContentTextBox.Text))
            {
                MessageBox.Show("Subject/ Email body cannot be empty");
                return;
            }

            SendEmail.Enabled = false;

            int count = 0, failCount = 0;
            StringBuilder errors = new StringBuilder();

            Dictionary<long, Tuple<string, string>> basicProfiles = _ReportRepository.GetBasicProfileInfo(0, 0);

            if (basicProfiles.Count <= 0)
            {
                MessageBox.Show("No profiles found. Please check the Profile range");
                return;
            }

            if (MessageBox.Show("This will send " + basicProfiles.Count.ToString() + " emails to the profiles. Confirm?", "Send Emails", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sentEmails = string.Empty; //Add email ids to exclude from notification list

                foreach (var basicProfile in basicProfiles)
                {
                    string email = basicProfile.Value.Item1;
                    if (!sentEmails.Contains("," + email + ","))
                    {
                        string name = basicProfile.Value.Item2 ?? "User";
                        var body = EmailContentTextBox.Text.Replace("{UserName}", name);

                        try
                        {
                            await SendEmailUsingSendGrid(email, subject, body);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            errors.Append("Error: " + name + " " + email + " " + ex.Message + Environment.NewLine);
                        }
                    }
                }

                MessageBox.Show("Emails have been successfully sent to " + count.ToString() + " Users");

                if (failCount > 0)
                    MessageBox.Show(errors.ToString());
            }
        }
    }

}