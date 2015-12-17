using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOS.Service.Interfaces.DataContracts;
using SOS.AzureStorageAccessLayer;

namespace SOS.Service.Implementation
{
    public class Dummies4SOS
    {

        private static bool _IsLoaded = false;
        public static void InitializeAllDataSet()
        {
            DummyStorage.LoadDummyDataSet();

            GlobalUserPositions_Load();

            Members_UpdateStatus();

            //Buddys_Load();

            //Members_RadarInfo();


            IsDummiesLoaded = true;

            string str = "";
        }

        public static bool IsDummiesLoaded
        {
            get
            {
                return _IsLoaded;
            }
            private set { _IsLoaded = value; }
        }


        public static List<Profile> Members
        {
            get;
            private set;
        }

        public static List<User> Persons
        {
            get;
            private set;
        }

        public static List<Group> Groups
        {
            get;
            private set;
        }

        public static List<Buddy> Buddys
        {
            get;
            private set;
        }

        private static void GlobalUserPositions_Load()
        {
            GlobalUserPositions.UpdateTrack("0.228113154639812", new GeoTag() { Lat = "45.7013861", Long = "-121.0804306", TimeStamp = DateTime.FromOADate(41317.9415509259) });
            GlobalUserPositions.UpdateTrack("0.228113154639812", new GeoTag() { Lat = "45.6958306", Long = "-121.0912389", TimeStamp = DateTime.FromOADate(41317.7372685185) });
            GlobalUserPositions.UpdateTrack("0.228113154639812", new GeoTag() { Lat = "45.6993056", Long = "-121.0889056", TimeStamp = DateTime.FromOADate(41317.7727777778) });
            GlobalUserPositions.UpdateTrack("0.228113154639812", new GeoTag() { Lat = "45.6966028", Long = "-121.0885417", TimeStamp = DateTime.FromOADate(41317.8722916667) });
            GlobalUserPositions.UpdateTrack("0.355814392515995", new GeoTag() { Lat = "45.7089833", Long = "-121.0955778", TimeStamp = DateTime.FromOADate(41317.7130092593) });
            GlobalUserPositions.UpdateTrack("0.355814392515995", new GeoTag() { Lat = "45.7089833", Long = "-121.0955778", TimeStamp = DateTime.FromOADate(41317.8452083333) });
            GlobalUserPositions.UpdateTrack("0.355814392515995", new GeoTag() { Lat = "45.7046444", Long = "-121.0816389", TimeStamp = DateTime.FromOADate(41317.6990277778) });
            GlobalUserPositions.UpdateTrack("0.355814392515995", new GeoTag() { Lat = "45.7031472", Long = "-121.0815889", TimeStamp = DateTime.FromOADate(41317.8888310185) });
            GlobalUserPositions.UpdateTrack("0.355814392515995", new GeoTag() { Lat = "45.6955083", Long = "-121.0917861", TimeStamp = DateTime.FromOADate(41317.7067708333) });
            GlobalUserPositions.UpdateTrack("0.265555728629781", new GeoTag() { Lat = "45.6955083", Long = "-121.0917861", TimeStamp = DateTime.FromOADate(41317.7547685185) });
            GlobalUserPositions.UpdateTrack("0.265555728629781", new GeoTag() { Lat = "45.7009361", Long = "-121.0841583", TimeStamp = DateTime.FromOADate(41317.9613888889) });
            GlobalUserPositions.UpdateTrack("0.265555728629781", new GeoTag() { Lat = "45.6809167", Long = "-121.0833694", TimeStamp = DateTime.FromOADate(41317.9612731481) });
            GlobalUserPositions.UpdateTrack("0.265555728629781", new GeoTag() { Lat = "45.6801444", Long = "-121.0579", TimeStamp = DateTime.FromOADate(41317.791724537) });
            GlobalUserPositions.UpdateTrack("0.623509041923517", new GeoTag() { Lat = "45.6789", Long = "-121.0561611", TimeStamp = DateTime.FromOADate(41317.7854976852) });
            GlobalUserPositions.UpdateTrack("0.0945541470743541", new GeoTag() { Lat = "45.7009417", Long = "-121.0846306", TimeStamp = DateTime.FromOADate(41317.8743402778) });
            GlobalUserPositions.UpdateTrack("0.0945541470743541", new GeoTag() { Lat = "45.7000778", Long = "-121.0841833", TimeStamp = DateTime.FromOADate(41317.8653587963) });
            GlobalUserPositions.UpdateTrack("0.0945541470743541", new GeoTag() { Lat = "45.6955722", Long = "-121.0917583", TimeStamp = DateTime.FromOADate(41317.850474537) });
            GlobalUserPositions.UpdateTrack("0.0945541470743541", new GeoTag() { Lat = "45.7038556", Long = "-121.0875528", TimeStamp = DateTime.FromOADate(41317.801724537) });
            GlobalUserPositions.UpdateTrack("0.955746387562424", new GeoTag() { Lat = "45.7134889", Long = "-121.101725", TimeStamp = DateTime.FromOADate(41317.9115509259) });
            GlobalUserPositions.UpdateTrack("0.955746387562424", new GeoTag() { Lat = "45.709025", Long = "-121.0956167", TimeStamp = DateTime.FromOADate(41317.7769907407) });
            GlobalUserPositions.UpdateTrack("0.955746387562424", new GeoTag() { Lat = "45.6783833", Long = "-121.0929389", TimeStamp = DateTime.FromOADate(41317.6926504629) });
            GlobalUserPositions.UpdateTrack("0.955746387562424", new GeoTag() { Lat = "45.6979972", Long = "-121.0889222", TimeStamp = DateTime.FromOADate(41317.8278703704) });
            GlobalUserPositions.UpdateTrack("0.955746387562424", new GeoTag() { Lat = "45.7130389", Long = "-121.1", TimeStamp = DateTime.FromOADate(41317.7366898148) });
            GlobalUserPositions.UpdateTrack("0.932833409108132", new GeoTag() { Lat = "45.7005917", Long = "-121.0806222", TimeStamp = DateTime.FromOADate(41317.8286574074) });
            GlobalUserPositions.UpdateTrack("0.777366648671736", new GeoTag() { Lat = "45.6982111", Long = "-121.0890778", TimeStamp = DateTime.FromOADate(41317.8910069444) });
            GlobalUserPositions.UpdateTrack("0.777366648671736", new GeoTag() { Lat = "45.6950778", Long = "-121.0924667", TimeStamp = DateTime.FromOADate(41317.9444212963) });
            GlobalUserPositions.UpdateTrack("0.777366648671736", new GeoTag() { Lat = "45.6957639", Long = "-121.0914167", TimeStamp = DateTime.FromOADate(41317.8503125) });
            GlobalUserPositions.UpdateTrack("0.777366648671736", new GeoTag() { Lat = "45.6808722", Long = "-121.0759028", TimeStamp = DateTime.FromOADate(41317.7096296296) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.6789", Long = "-121.0561611", TimeStamp = DateTime.FromOADate(41317.8396064815) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.6789", Long = "-121.0561833", TimeStamp = DateTime.FromOADate(41317.7693518518) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.6789", Long = "-121.0561611", TimeStamp = DateTime.FromOADate(41317.7323611111) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.6788556", Long = "-121.0562028", TimeStamp = DateTime.FromOADate(41317.6907175926) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.6788556", Long = "-121.0562028", TimeStamp = DateTime.FromOADate(41317.9202430556) });
            GlobalUserPositions.UpdateTrack("0.293581972006254", new GeoTag() { Lat = "45.7047333", Long = "-121.0817556", TimeStamp = DateTime.FromOADate(41317.9364236111) });
            GlobalUserPositions.UpdateTrack("0.432940746125607", new GeoTag() { Lat = "45.7046583", Long = "-121.08175", TimeStamp = DateTime.FromOADate(41317.7442592593) });
            GlobalUserPositions.UpdateTrack("0.800861279065265", new GeoTag() { Lat = "45.7137667", Long = "-121.1015667", TimeStamp = DateTime.FromOADate(41317.8917939815) });
            GlobalUserPositions.UpdateTrack("0.800861279065265", new GeoTag() { Lat = "45.6834472", Long = "-121.0963722", TimeStamp = DateTime.FromOADate(41317.8718634259) });
            GlobalUserPositions.UpdateTrack("0.800861279065265", new GeoTag() { Lat = "45.6799722", Long = "-121.0800444", TimeStamp = DateTime.FromOADate(41317.831087963) });
            GlobalUserPositions.UpdateTrack("0.800861279065265", new GeoTag() { Lat = "45.665275", Long = "-121.1064333", TimeStamp = DateTime.FromOADate(41317.856087963) });
            GlobalUserPositions.UpdateTrack("0.210159883387691", new GeoTag() { Lat = "45.7046694", Long = "-121.0818889", TimeStamp = DateTime.FromOADate(41317.8531365741) });
            GlobalUserPositions.UpdateTrack("0.210159883387691", new GeoTag() { Lat = "45.704675", Long = "-121.0819056", TimeStamp = DateTime.FromOADate(41317.8564930556) });
            GlobalUserPositions.UpdateTrack("0.210159883387691", new GeoTag() { Lat = "45.7010639", Long = "-121.0806", TimeStamp = DateTime.FromOADate(41317.8141898148) });
            GlobalUserPositions.UpdateTrack("0.210159883387691", new GeoTag() { Lat = "45.6801444", Long = "-121.0579", TimeStamp = DateTime.FromOADate(41317.8786458333) });
            GlobalUserPositions.UpdateSOS("1.65946146567886", new GeoTag() { Lat = "45.6777417", Long = "-121.0929833", TimeStamp = DateTime.FromOADate(41317.7395254629) });
            GlobalUserPositions.UpdateSOS("1.62326865011814", new GeoTag() { Lat = "45.6982333", Long = "-121.0890556", TimeStamp = DateTime.FromOADate(41317.7084259259) });
            GlobalUserPositions.UpdateSOS("1.62326865011814", new GeoTag() { Lat = "45.6984889", Long = "-121.0888833", TimeStamp = DateTime.FromOADate(41317.7070833333) });
            GlobalUserPositions.UpdateSOS("1.62326865011814", new GeoTag() { Lat = "45.7104194", Long = "-121.0972944", TimeStamp = DateTime.FromOADate(41317.7840509259) });
            GlobalUserPositions.UpdateSOS("1.62326865011814", new GeoTag() { Lat = "45.7002278", Long = "-121.0833917", TimeStamp = DateTime.FromOADate(41317.7168171296) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7009806", Long = "-121.0825333", TimeStamp = DateTime.FromOADate(41317.948599537) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7036194", Long = "-121.0818167", TimeStamp = DateTime.FromOADate(41317.7843171296) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7089611", Long = "-121.0955778", TimeStamp = DateTime.FromOADate(41317.7302893518) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7117944", Long = "-121.0986472", TimeStamp = DateTime.FromOADate(41317.8030902778) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.713725", Long = "-121.1016722", TimeStamp = DateTime.FromOADate(41317.9439236111) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7057861", Long = "-121.0913528", TimeStamp = DateTime.FromOADate(41317.720162037) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.6993917", Long = "-121.0852306", TimeStamp = DateTime.FromOADate(41317.7911111111) });
            GlobalUserPositions.UpdateSOS("1.47846733106848", new GeoTag() { Lat = "45.7035111", Long = "-121.0874667", TimeStamp = DateTime.FromOADate(41317.7281712963) });
            GlobalUserPositions.UpdateSOS("1.15785653481947", new GeoTag() { Lat = "45.7007", Long = "-121.0806361", TimeStamp = DateTime.FromOADate(41317.6951041667) });
            GlobalUserPositions.UpdateSOS("1.03112425331975", new GeoTag() { Lat = "45.698", Long = "-121.0885417", TimeStamp = DateTime.FromOADate(41317.8446412037) });
            GlobalUserPositions.UpdateSOS("1.03112425331975", new GeoTag() { Lat = "45.7031972", Long = "-121.0872306", TimeStamp = DateTime.FromOADate(41317.7249884259) });

        }

        private static void Members_UpdateStatus()
        {
            if (Members == null)
                Members = new List<Profile>();
            foreach (Profile m in Members)
            {
                List<GeoTag> tGtag;
                if (GlobalUserPositions.TrackingDetails.TryGetValue(m.TrackingToken, out tGtag))
                {
                    m.LastLocs = tGtag;
                    m.IsTrackingOn = true;
                }
                else
                {
                    m.IsTrackingOn = false;
                }

                if (GlobalUserPositions.SOSDetails.TryGetValue(m.SOSToken, out tGtag))
                {
                    m.LastLocs = tGtag;
                    m.IsSOSOn = true;
                }
                else
                {
                    m.IsSOSOn = false;
                }
            }
        }

        #region Commented

        //private static void Groups_Load()
        //{
        //    Groups = new List<Group>();
        //    Groups.Add(new Group() { GroupID = "1", GroupName = "SOCHYD", IsActive = true, GroupLocation = "Hyderabad" });
        //}

        //private static void Members_RadarInfo()
        //{
        //    foreach (Profile m in Members)
        //    {
        //        m.MembersITrack = new List<Profile>();
        //        var bdys = Buddys.FindAll(b => b.UserID == m.UserID).ToList();
        //        foreach (Buddy b in bdys)
        //        {
        //            var mm = Members.Find(n => n.ProfileID == b.RelProfileID);
        //            if (mm != null)
        //            {
        //                m.MembersITrack.Add(mm);
        //            }
        //        }

        //        m.BuddiesTrackingMe = new List<Buddy>();

        //        var bs = Buddys.FindAll(x => x.RelProfileID == m.ProfileID);
        //        if (bs != null)
        //        {
        //            m.BuddiesTrackingMe = bs;
        //        }
        //    }
        //}


        //private static void Members_Load()
        //     {
        //         Members = new List<Profile>();

        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "10", Name = "Name1", UserID = "1", TrackingToken = "0.228113154639812", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "20", Name = "Name2", UserID = "2", TrackingToken = "0.355814392515995", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "30", Name = "Name3", UserID = "3", TrackingToken = "0.265555728629781", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "40", Name = "Name4", UserID = "4", TrackingToken = "0.623509041923517", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "50", Name = "Name5", UserID = "5", TrackingToken = "0.0945541470743541", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "60", Name = "Name6", UserID = "6", TrackingToken = "0.955746387562424", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "70", Name = "Name7", UserID = "7", TrackingToken = "0.932833409108132", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "80", Name = "Name8", UserID = "8", TrackingToken = "0.777366648671736", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "90", Name = "Name9", UserID = "9", TrackingToken = "0.293581972006254", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "100", Name = "Name10", UserID = "10", TrackingToken = "0.432940746125607", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "110", Name = "Name11", UserID = "11", TrackingToken = "0.800861279065265", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "120", Name = "Name12", UserID = "12", TrackingToken = "0.210159883387691", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "130", Name = "Name13", UserID = "13", TrackingToken = "0.65946146567886", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "140", Name = "Name14", UserID = "14", TrackingToken = "0.62326865011814", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "150", Name = "Name15", UserID = "15", TrackingToken = "0.47846733106848", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "160", Name = "Name16", UserID = "16", TrackingToken = "0.47846733106848", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "170", Name = "Name17", UserID = "17", TrackingToken = "0.15785653481947", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "180", Name = "Name18", UserID = "18", TrackingToken = "0.03112425331975", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "240", Name = "Name24", UserID = "24", TrackingToken = "0.468159509637797", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "250", Name = "Name25", UserID = "25", TrackingToken = "0.329224957188351", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "260", Name = "Name26", UserID = "26", TrackingToken = "0.75596076766807", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "270", Name = "Name27", UserID = "27", TrackingToken = "0.110586239277153", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "280", Name = "Name28", UserID = "28", TrackingToken = "0.928679617316808", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "290", Name = "Name29", UserID = "29", TrackingToken = "0.240371379771383", TinyURI = "guardian.com" });
        //         Members.Add(new Profile() { AscGroups = null, ProfileID = "300", Name = "Name30", UserID = "30", TrackingToken = "0.233214959125806", TinyURI = "guardian.com" });

        //         User usr = new User();
        //         foreach (Profile pr in Members)
        //         {
        //             usr = Persons.Find(u => u.UserID == pr.UserID);
        //             pr.ContactInfo = usr.ContactInfo;
        //         }

        //         Members.ForEach(x => x.SOSToken = x.TrackingToken.Replace("0.", "1."));

        //         //START:Group seggregation. This has to happen when each member is loaded, that is. while member is loaded, the group will be updated parallely.
        //         //Since for dummy we have only one group, hardcodign the index.
        //         if (Groups[0].Members == null)
        //             Groups[0].Members = new List<Profile>();
        //         Groups[0].Members.AddRange(Members);
        //         //END:Group seggregation.

        //     }

        //     private static void Persons_Load()
        //     {
        //         Persons = new List<User>();
        //         //Persons.Add(new Person() { PID = "A1", Name = "B1", ContactInfo = new List<Contact>() { new Contact() { MobileNumber = "C1", Email = "D1" } } });
        //         Persons.Add(new User() { UserID = "1", Name = "Name1", ContactInfo = new Contact() { MobileNumber = "11111", Email = "11111@1.com" } });
        //         Persons.Add(new User() { UserID = "2", Name = "Name2", ContactInfo = new Contact() { MobileNumber = "22222", Email = "22222@2.com" } });
        //         Persons.Add(new User() { UserID = "3", Name = "Name3", ContactInfo = new Contact() { MobileNumber = "33333", Email = "33333@3.com" } });
        //         Persons.Add(new User() { UserID = "4", Name = "Name4", ContactInfo = new Contact() { MobileNumber = "44444", Email = "44444@4.com" } });
        //         Persons.Add(new User() { UserID = "5", Name = "Name5", ContactInfo = new Contact() { MobileNumber = "55555", Email = "55555@5.com" } });
        //         Persons.Add(new User() { UserID = "6", Name = "Name6", ContactInfo = new Contact() { MobileNumber = "66666", Email = "66666@6.com" } });
        //         Persons.Add(new User() { UserID = "7", Name = "Name7", ContactInfo = new Contact() { MobileNumber = "77777", Email = "77777@7.com" } });
        //         Persons.Add(new User() { UserID = "8", Name = "Name8", ContactInfo = new Contact() { MobileNumber = "88888", Email = "88888@8.com" } });
        //         Persons.Add(new User() { UserID = "9", Name = "Name9", ContactInfo = new Contact() { MobileNumber = "99999", Email = "99999@9.com" } });
        //         Persons.Add(new User() { UserID = "10", Name = "Name10", ContactInfo = new Contact() { MobileNumber = "1010101010", Email = "1010101010@10.com" } });
        //         Persons.Add(new User() { UserID = "11", Name = "Name11", ContactInfo = new Contact() { MobileNumber = "1111111111", Email = "1111111111@11.com" } });
        //         Persons.Add(new User() { UserID = "12", Name = "Name12", ContactInfo = new Contact() { MobileNumber = "1212121212", Email = "1212121212@12.com" } });
        //         Persons.Add(new User() { UserID = "13", Name = "Name13", ContactInfo = new Contact() { MobileNumber = "1313131313", Email = "1313131313@13.com" } });
        //         Persons.Add(new User() { UserID = "14", Name = "Name14", ContactInfo = new Contact() { MobileNumber = "1414141414", Email = "1414141414@14.com" } });
        //         Persons.Add(new User() { UserID = "15", Name = "Name15", ContactInfo = new Contact() { MobileNumber = "1515151515", Email = "1515151515@15.com" } });
        //         Persons.Add(new User() { UserID = "16", Name = "Name16", ContactInfo = new Contact() { MobileNumber = "1616161616", Email = "1616161616@16.com" } });
        //         Persons.Add(new User() { UserID = "17", Name = "Name17", ContactInfo = new Contact() { MobileNumber = "1717171717", Email = "1717171717@17.com" } });
        //         Persons.Add(new User() { UserID = "18", Name = "Name18", ContactInfo = new Contact() { MobileNumber = "1818181818", Email = "1818181818@18.com" } });
        //         Persons.Add(new User() { UserID = "19", Name = "Name19", ContactInfo = new Contact() { MobileNumber = "1919191919", Email = "1919191919@19.com" } });
        //         Persons.Add(new User() { UserID = "20", Name = "Name20", ContactInfo = new Contact() { MobileNumber = "2020202020", Email = "2020202020@20.com" } });
        //         Persons.Add(new User() { UserID = "21", Name = "Name21", ContactInfo = new Contact() { MobileNumber = "2121212121", Email = "2121212121@21.com" } });
        //         Persons.Add(new User() { UserID = "22", Name = "Name22", ContactInfo = new Contact() { MobileNumber = "2222222222", Email = "2222222222@22.com" } });
        //         Persons.Add(new User() { UserID = "23", Name = "Name23", ContactInfo = new Contact() { MobileNumber = "2323232323", Email = "2323232323@23.com" } });
        //         Persons.Add(new User() { UserID = "24", Name = "Name24", ContactInfo = new Contact() { MobileNumber = "2424242424", Email = "2424242424@24.com" } });
        //         Persons.Add(new User() { UserID = "25", Name = "Name25", ContactInfo = new Contact() { MobileNumber = "2525252525", Email = "2525252525@25.com" } });
        //         Persons.Add(new User() { UserID = "26", Name = "Name26", ContactInfo = new Contact() { MobileNumber = "2626262626", Email = "2626262626@26.com" } });
        //         Persons.Add(new User() { UserID = "27", Name = "Name27", ContactInfo = new Contact() { MobileNumber = "2727272727", Email = "2727272727@27.com" } });
        //         Persons.Add(new User() { UserID = "28", Name = "Name28", ContactInfo = new Contact() { MobileNumber = "2828282828", Email = "2828282828@28.com" } });
        //         Persons.Add(new User() { UserID = "29", Name = "Name29", ContactInfo = new Contact() { MobileNumber = "2929292929", Email = "2929292929@29.com" } });
        //         Persons.Add(new User() { UserID = "30", Name = "Name30", ContactInfo = new Contact() { MobileNumber = "3030303030", Email = "3030303030@30.com" } });



        //     }


        //     private static void Buddys_Load()
        //     {
        //         Buddys = new List<Buddy>();

        //         //            Buddys.Add(new Buddy() {RelMemberID = "G2", PID = "F2", BID = "L2"});
        //         Buddys.Add(new Buddy() { RelProfileID = "10", UserID = "2", BID = "10_2" });
        //         Buddys.Add(new Buddy() { RelProfileID = "10", UserID = "3", BID = "10_3" });
        //         Buddys.Add(new Buddy() { RelProfileID = "10", UserID = "4", BID = "10_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "10", UserID = "5", BID = "10_5" });
        //         Buddys.Add(new Buddy() { RelProfileID = "20", UserID = "1", BID = "20_1" });
        //         Buddys.Add(new Buddy() { RelProfileID = "20", UserID = "3", BID = "20_3" });
        //         Buddys.Add(new Buddy() { RelProfileID = "20", UserID = "4", BID = "20_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "20", UserID = "5", BID = "20_5" });
        //         Buddys.Add(new Buddy() { RelProfileID = "20", UserID = "6", BID = "20_6" });
        //         Buddys.Add(new Buddy() { RelProfileID = "30", UserID = "11", BID = "30_11" });
        //         Buddys.Add(new Buddy() { RelProfileID = "30", UserID = "12", BID = "30_12" });
        //         Buddys.Add(new Buddy() { RelProfileID = "30", UserID = "21", BID = "30_21" });
        //         Buddys.Add(new Buddy() { RelProfileID = "30", UserID = "23", BID = "30_23" });
        //         Buddys.Add(new Buddy() { RelProfileID = "40", UserID = "23", BID = "40_23" });
        //         Buddys.Add(new Buddy() { RelProfileID = "50", UserID = "3", BID = "50_3" });
        //         Buddys.Add(new Buddy() { RelProfileID = "50", UserID = "2", BID = "50_2" });
        //         Buddys.Add(new Buddy() { RelProfileID = "50", UserID = "7", BID = "50_7" });
        //         Buddys.Add(new Buddy() { RelProfileID = "50", UserID = "8", BID = "50_8" });
        //         Buddys.Add(new Buddy() { RelProfileID = "60", UserID = "12", BID = "60_12" });
        //         Buddys.Add(new Buddy() { RelProfileID = "60", UserID = "15", BID = "60_15" });
        //         Buddys.Add(new Buddy() { RelProfileID = "60", UserID = "16", BID = "60_16" });
        //         Buddys.Add(new Buddy() { RelProfileID = "60", UserID = "20", BID = "60_20" });
        //         Buddys.Add(new Buddy() { RelProfileID = "60", UserID = "22", BID = "60_22" });
        //         Buddys.Add(new Buddy() { RelProfileID = "70", UserID = "9", BID = "70_9" });
        //         Buddys.Add(new Buddy() { RelProfileID = "80", UserID = "9", BID = "80_9" });
        //         Buddys.Add(new Buddy() { RelProfileID = "80", UserID = "10", BID = "80_10" });
        //         Buddys.Add(new Buddy() { RelProfileID = "80", UserID = "2", BID = "80_2" });
        //         Buddys.Add(new Buddy() { RelProfileID = "80", UserID = "4", BID = "80_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "6", BID = "90_6" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "7", BID = "90_7" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "4", BID = "90_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "13", BID = "90_13" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "14", BID = "90_14" });
        //         Buddys.Add(new Buddy() { RelProfileID = "90", UserID = "16", BID = "90_16" });
        //         Buddys.Add(new Buddy() { RelProfileID = "100", UserID = "16", BID = "100_16" });
        //         Buddys.Add(new Buddy() { RelProfileID = "110", UserID = "17", BID = "110_17" });
        //         Buddys.Add(new Buddy() { RelProfileID = "110", UserID = "12", BID = "110_12" });
        //         Buddys.Add(new Buddy() { RelProfileID = "110", UserID = "14", BID = "110_14" });
        //         Buddys.Add(new Buddy() { RelProfileID = "110", UserID = "16", BID = "110_16" });
        //         Buddys.Add(new Buddy() { RelProfileID = "120", UserID = "18", BID = "120_18" });
        //         Buddys.Add(new Buddy() { RelProfileID = "120", UserID = "19", BID = "120_19" });
        //         Buddys.Add(new Buddy() { RelProfileID = "120", UserID = "11", BID = "120_11" });
        //         Buddys.Add(new Buddy() { RelProfileID = "120", UserID = "21", BID = "120_21" });
        //         Buddys.Add(new Buddy() { RelProfileID = "130", UserID = "22", BID = "130_22" });
        //         Buddys.Add(new Buddy() { RelProfileID = "140", UserID = "5", BID = "140_5" });
        //         Buddys.Add(new Buddy() { RelProfileID = "140", UserID = "6", BID = "140_6" });
        //         Buddys.Add(new Buddy() { RelProfileID = "140", UserID = "7", BID = "140_7" });
        //         Buddys.Add(new Buddy() { RelProfileID = "140", UserID = "3", BID = "140_3" });
        //         Buddys.Add(new Buddy() { RelProfileID = "150", UserID = "2", BID = "150_2" });
        //         Buddys.Add(new Buddy() { RelProfileID = "150", UserID = "4", BID = "150_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "150", UserID = "9", BID = "150_9" });
        //         Buddys.Add(new Buddy() { RelProfileID = "150", UserID = "10", BID = "150_10" });
        //         Buddys.Add(new Buddy() { RelProfileID = "150", UserID = "12", BID = "150_12" });
        //         Buddys.Add(new Buddy() { RelProfileID = "160", UserID = "21", BID = "160_21" });
        //         Buddys.Add(new Buddy() { RelProfileID = "160", UserID = "20", BID = "160_20" });
        //         Buddys.Add(new Buddy() { RelProfileID = "160", UserID = "18", BID = "160_18" });
        //         Buddys.Add(new Buddy() { RelProfileID = "170", UserID = "3", BID = "170_3" });
        //         Buddys.Add(new Buddy() { RelProfileID = "180", UserID = "4", BID = "180_4" });
        //         Buddys.Add(new Buddy() { RelProfileID = "180", UserID = "2", BID = "180_2" });


        //     }

        #endregion
    }
}
