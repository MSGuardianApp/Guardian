﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Guardian.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Guardian.Common.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi,&lt;br /&gt; &lt;br /&gt; {0}({1}) has added you as a buddy in Guardian for receiving SOS notifications and Tracking. &lt;br /&gt; If you wish not to receive notifications for this user, click below link to unsubscribe &lt;br /&gt; {2} &lt;br /&gt; &lt;br /&gt; Thanks, &lt;br /&gt; Guardian Team  &lt;br /&gt; **This email is sent from an unmonitored alias.**.
        /// </summary>
        public static string BuddyNotificationEmailBody {
            get {
                return ResourceManager.GetString("BuddyNotificationEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Buddy addition in Guardian for {0}({1}).
        /// </summary>
        public static string BuddyNotificationEmailSubject {
            get {
                return ResourceManager.GetString("BuddyNotificationEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}({1}) added you as a buddy in Guardian App. To unsubscribe, click {2}.
        /// </summary>
        public static string BuddyNotificationSMSBody {
            get {
                return ResourceManager.GetString("BuddyNotificationSMSBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi, &lt;br /&gt; &lt;br /&gt;Please click the below link to validate your Guardian group association. &lt;br /&gt; &lt;br /&gt; Validation Link: {2}/Verification.aspx?V=2&amp;amp;key={0}&amp;amp;pr={1}&amp;amp;et=G&lt;br /&gt; &lt;br /&gt;Thanks, &lt;br /&gt; Guardian Team &lt;br /&gt;**This email is sent from an unmonitored alias.** &lt;br /&gt; &lt;br /&gt;.
        /// </summary>
        public static string EmailGroupValidationMsgTmp {
            get {
                return ResourceManager.GetString("EmailGroupValidationMsgTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activate your Group validation for Guardian.
        /// </summary>
        public static string EmailGroupValidationSubj {
            get {
                return ResourceManager.GetString("EmailGroupValidationSubj", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi, &lt;br /&gt; &lt;br /&gt; Please click the bollowing link to confirm yourself as Guardian group marshal. &lt;br /&gt; &lt;br /&gt; Validation Link: {2}/Verification.aspx?V=2&amp;amp;key={0}&amp;amp;pr={1}&amp;amp;et=M &lt;br /&gt; &lt;br /&gt; Thanks,&lt;br /&gt; &lt;br /&gt; Guardian Team &lt;br /&gt; &lt;br /&gt; **This email is sent from an unmonitored alias.** &lt;br /&gt;&lt;br /&gt;.
        /// </summary>
        public static string EmailMarshalValidationMsgTmp {
            get {
                return ResourceManager.GetString("EmailMarshalValidationMsgTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Guardian marshal confirmation.
        /// </summary>
        public static string EmailMarshalValidationSubj {
            get {
                return ResourceManager.GetString("EmailMarshalValidationSubj", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hello, &lt;br /&gt; &lt;br /&gt; {name} (Mobile No {phone}) needs urgent help at {address}.  Track at {tinyuri} and help urgently. &lt;br /&gt; &lt;br /&gt; Thanks, &lt;br /&gt;&lt;br /&gt; Guardian Team &lt;br /&gt; &lt;br /&gt; &lt;br /&gt; **This email is sent from an unmonitored alias. If you can’t provide help, try reaching appropriate authorities for help.** &lt;br /&gt;&lt;br /&gt;.
        /// </summary>
        public static string EmailMessage {
            get {
                return ResourceManager.GetString("EmailMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hello, &lt;br /&gt; &lt;br /&gt; {name} (Mobile No {phone}) is safe now. &lt;br /&gt;&lt;br /&gt; Thanks, &lt;br /&gt; Guardian Team &lt;br /&gt;&lt;br /&gt; &lt;br /&gt; **This email is sent from an unmonitored alias. If you can’t provide help, try reaching appropriate authorities for help.** &lt;br /&gt;.
        /// </summary>
        public static string EmailSafeMessage {
            get {
                return ResourceManager.GetString("EmailSafeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Guardian SOS: {name} is safe now!.
        /// </summary>
        public static string EmailSafeSubject {
            get {
                return ResourceManager.GetString("EmailSafeSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Guardian SOS: {name} needs urgent help!.
        /// </summary>
        public static string EmailSubject {
            get {
                return ResourceManager.GetString("EmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter {0} for Enterprise verification code to create your Profile.
        /// </summary>
        public static string EnterpriseValidationMsg {
            get {
                return ResourceManager.GetString("EnterpriseValidationMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {name} (Mobile No {phone}) needs urgent help at {address}. Track at {tinyuri} and help urgently..
        /// </summary>
        public static string FacebookMessage {
            get {
                return ResourceManager.GetString("FacebookMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {name} is safe now..
        /// </summary>
        public static string FacebookSafeMessage {
            get {
                return ResourceManager.GetString("FacebookSafeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Guardian SOS: {name} is safe now!.
        /// </summary>
        public static string FacebookSafeSubject {
            get {
                return ResourceManager.GetString("FacebookSafeSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Guardian SOS: {name} needs urgent help!.
        /// </summary>
        public static string FacebookSubject {
            get {
                return ResourceManager.GetString("FacebookSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter {0} verification code to create your Profile for phone number ending with {1}.
        /// </summary>
        public static string PhoneReValidationSMS {
            get {
                return ResourceManager.GetString("PhoneReValidationSMS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter {0} verification code to create your Profile for phone number ending with {1}.
        /// </summary>
        public static string PhoneValidationSMS {
            get {
                return ResourceManager.GetString("PhoneValidationSMS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {name} ({phone}) needs urgent help at {address}. Track at {tinyuri}.
        /// </summary>
        public static string SMSMessage {
            get {
                return ResourceManager.GetString("SMSMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {name} is safe now..
        /// </summary>
        public static string SMSSafeMessage {
            get {
                return ResourceManager.GetString("SMSSafeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi,  &lt;br /&gt; &lt;br /&gt; You have unregistered yourself from Guardian App. We have ensured to delete your profile from our servers. &lt;br /&gt; Please be informed that Guardian works best when you use it as a registered user. &lt;br /&gt; &lt;br /&gt; Thanks, &lt;br /&gt; Guardian Team  &lt;br /&gt; **This email is sent from an unmonitored alias.**.
        /// </summary>
        public static string UnRegisterNotificationEmailBody {
            get {
                return ResourceManager.GetString("UnRegisterNotificationEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Profile unregistered in Guardian.
        /// </summary>
        public static string UnRegisterNotificationEmailSubject {
            get {
                return ResourceManager.GetString("UnRegisterNotificationEmailSubject", resourceCulture);
            }
        }
    }
}