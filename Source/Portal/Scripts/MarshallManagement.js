var MarshalEntitiesLayer;
function SavemarshalInfo() {
    GroupID = getSession('GroupID');
    AdminID = getSession('AdminID');
    FormEdit = false;
    var marshalLiveID = $("#LiveId").val();
    var phoneNumber = $("#MarshalPhoneNum").val();
    
    if ($.trim(marshalLiveID) == '') {
        alert("Enter Valid LiveID");
        return;
    }
    if ($.trim(phoneNumber) == '') {
        alert("Enter Valid Phone Number");
        return;
    }
    var saveMarshalUrl = GuardianConfig.ServiceURL + '/SaveMarshalInfo';
    var postData = {};
    postData.GroupID = GroupID;
    postData.LiveMail = marshalLiveID;
    postData.PhoneNumber = phoneNumber;
    var jsonPostData = JSON.stringify(postData);
    $.ajax({
        type: "POST",
        url: saveMarshalUrl,
        dataType: "json",
        data: jsonPostData,
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.ResultType == 0) {
                displayMarshalList();
                alert('Request Has Been Submited !');
            } else {
                alert('Invalid User !');
            }
        },
        error: function (data) {
        }
    });
}
function managemarshals()
{
    $( "#marshal_content" ).removeClass( "disappear" );
    $( "#marshal_content" ).addClass( "appear" );
}
var MarshalList = null;
var userProfileId;
//{ "ContactInfo": { "Email": "11111@1.com", "MobileNumber": "11111", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name1", "UserID": "1", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7013861", "Long": "-121.0804306", "Speed": 0, "TimeStamp": "\/Date(1360708550000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6958306", "Long": "-121.0912389", "Speed": 0, "TimeStamp": "\/Date(1360690900000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6993056", "Long": "-121.0889056", "Speed": 0, "TimeStamp": "\/Date(1360693968000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6966028", "Long": "-121.0885417", "Speed": 0, "TimeStamp": "\/Date(1360702566000+0000)\/" }], "ProfileID": "10", "SOSToken": "1.228113154639812", "TrackingToken": "0.228113154639812", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null },
var dummyLocateBuddies=[{ "ContactInfo": { "Email": "22222@2.com", "MobileNumber": "22222", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name2", "UserID": "2", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7089833", "Long": "-121.0955778", "Speed": 0, "TimeStamp": "\/Date(1360688804000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7089833", "Long": "-121.0955778", "Speed": 0, "TimeStamp": "\/Date(1360700226000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7046444", "Long": "-121.0816389", "Speed": 0, "TimeStamp": "\/Date(1360687596000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7031472", "Long": "-121.0815889", "Speed": 0, "TimeStamp": "\/Date(1360703995000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6955083", "Long": "-121.0917861", "Speed": 0, "TimeStamp": "\/Date(1360688265000+0000)\/" }], "ProfileID": "20", "SOSToken": "1.355814392515995", "TrackingToken": "0.355814392515995", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "33333@3.com", "MobileNumber": "33333", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name3", "UserID": "3", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6955083", "Long": "-121.0917861", "Speed": 0, "TimeStamp": "\/Date(1360692412000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7009361", "Long": "-121.0841583", "Speed": 0, "TimeStamp": "\/Date(1360710264000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6809167", "Long": "-121.0833694", "Speed": 0, "TimeStamp": "\/Date(1360710254000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6801444", "Long": "-121.0579", "Speed": 0, "TimeStamp": "\/Date(1360695605000+0000)\/" }], "ProfileID": "70", "SOSToken": "1.265555728629781", "TrackingToken": "0.265555728629781", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "44444@4.com", "MobileNumber": "44444", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name4", "UserID": "4", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6789", "Long": "-121.0561611", "Speed": 0, "TimeStamp": "\/Date(1360695067000+0000)\/" }], "ProfileID": "40", "SOSToken": "1.623509041923517", "TrackingToken": "0.623509041923517", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "55555@5.com", "MobileNumber": "55555", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name5", "UserID": "5", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7009417", "Long": "-121.0846306", "Speed": 0, "TimeStamp": "\/Date(1360702743000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7000778", "Long": "-121.0841833", "Speed": 0, "TimeStamp": "\/Date(1360701967000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6955722", "Long": "-121.0917583", "Speed": 0, "TimeStamp": "\/Date(1360700681000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7038556", "Long": "-121.0875528", "Speed": 0, "TimeStamp": "\/Date(1360696469000+0000)\/" }], "ProfileID": "50", "SOSToken": "1.0945541470743541", "TrackingToken": "0.0945541470743541", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "66666@6.com", "MobileNumber": "66666", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name6", "UserID": "6", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7134889", "Long": "-121.101725", "Speed": 0, "TimeStamp": "\/Date(1360705958000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.709025", "Long": "-121.0956167", "Speed": 0, "TimeStamp": "\/Date(1360694332000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6783833", "Long": "-121.0929389", "Speed": 0, "TimeStamp": "\/Date(1360687045000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6979972", "Long": "-121.0889222", "Speed": 0, "TimeStamp": "\/Date(1360698728000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7130389", "Long": "-121.1", "Speed": 0, "TimeStamp": "\/Date(1360690850000+0000)\/" }], "ProfileID": "60", "SOSToken": "1.955746387562424", "TrackingToken": "0.955746387562424", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "77777@7.com", "MobileNumber": "77777", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name7", "UserID": "7", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7005917", "Long": "-121.0806222", "Speed": 0, "TimeStamp": "\/Date(1360698796000+0000)\/" }], "ProfileID": "70", "SOSToken": "1.932833409108132", "TrackingToken": "0.932833409108132", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "88888@8.com", "MobileNumber": "88888", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name8", "UserID": "8", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6982111", "Long": "-121.0890778", "Speed": 0, "TimeStamp": "\/Date(1360704183000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6950778", "Long": "-121.0924667", "Speed": 0, "TimeStamp": "\/Date(1360708798000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6957639", "Long": "-121.0914167", "Speed": 0, "TimeStamp": "\/Date(1360700667000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6808722", "Long": "-121.0759028", "Speed": 0, "TimeStamp": "\/Date(1360688512000+0000)\/" }], "ProfileID": "80", "SOSToken": "1.777366648671736", "TrackingToken": "0.777366648671736", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "99999@9.com", "MobileNumber": "99999", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name9", "UserID": "9", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6789", "Long": "-121.0561611", "Speed": 0, "TimeStamp": "\/Date(1360699742000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6789", "Long": "-121.0561833", "Speed": 0, "TimeStamp": "\/Date(1360693672000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6789", "Long": "-121.0561611", "Speed": 0, "TimeStamp": "\/Date(1360690476000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6788556", "Long": "-121.0562028", "Speed": 0, "TimeStamp": "\/Date(1360686878000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6788556", "Long": "-121.0562028", "Speed": 0, "TimeStamp": "\/Date(1360706709000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7047333", "Long": "-121.0817556", "Speed": 0, "TimeStamp": "\/Date(1360708107000+0000)\/" }], "ProfileID": "90", "SOSToken": "1.293581972006254", "TrackingToken": "0.293581972006254", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1010101010@10.com", "MobileNumber": "1010101010", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name10", "UserID": "10", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7046583", "Long": "-121.08175", "Speed": 0, "TimeStamp": "\/Date(1360691504000+0000)\/" }], "ProfileID": "100", "SOSToken": "1.432940746125607", "TrackingToken": "0.432940746125607", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1111111111@11.com", "MobileNumber": "1111111111", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name11", "UserID": "11", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7137667", "Long": "-121.1015667", "Speed": 0, "TimeStamp": "\/Date(1360704251000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6834472", "Long": "-121.0963722", "Speed": 0, "TimeStamp": "\/Date(1360702529000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6799722", "Long": "-121.0800444", "Speed": 0, "TimeStamp": "\/Date(1360699006000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.665275", "Long": "-121.1064333", "Speed": 0, "TimeStamp": "\/Date(1360701166000+0000)\/" }], "ProfileID": "110", "SOSToken": "1.800861279065265", "TrackingToken": "0.800861279065265", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1212121212@12.com", "MobileNumber": "1212121212", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name12", "UserID": "12", "IsSOSOn": false, "IsTrackingOn": true, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7046694", "Long": "-121.0818889", "Speed": 0, "TimeStamp": "\/Date(1360700911000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.704675", "Long": "-121.0819056", "Speed": 0, "TimeStamp": "\/Date(1360701201000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7010639", "Long": "-121.0806", "Speed": 0, "TimeStamp": "\/Date(1360697546000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6801444", "Long": "-121.0579", "Speed": 0, "TimeStamp": "\/Date(1360703115000+0000)\/" }], "ProfileID": "120", "SOSToken": "1.210159883387691", "TrackingToken": "0.210159883387691", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1313131313@13.com", "MobileNumber": "1313131313", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name13", "UserID": "13", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6777417", "Long": "-121.0929833", "Speed": 0, "TimeStamp": "\/Date(1360691095000+0000)\/" }], "ProfileID": "130", "SOSToken": "1.65946146567886", "TrackingToken": "0.65946146567886", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1414141414@14.com", "MobileNumber": "1414141414", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name14", "UserID": "14", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6982333", "Long": "-121.0890556", "Speed": 0, "TimeStamp": "\/Date(1360688408000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6984889", "Long": "-121.0888833", "Speed": 0, "TimeStamp": "\/Date(1360688292000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7104194", "Long": "-121.0972944", "Speed": 0, "TimeStamp": "\/Date(1360694942000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7002278", "Long": "-121.0833917", "Speed": 0, "TimeStamp": "\/Date(1360689133000+0000)\/" }], "ProfileID": "140", "SOSToken": "1.62326865011814", "TrackingToken": "0.62326865011814", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1515151515@15.com", "MobileNumber": "1515151515", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name15", "UserID": "15", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7009806", "Long": "-121.0825333", "Speed": 0, "TimeStamp": "\/Date(1360709159000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7036194", "Long": "-121.0818167", "Speed": 0, "TimeStamp": "\/Date(1360694965000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7089611", "Long": "-121.0955778", "Speed": 0, "TimeStamp": "\/Date(1360690297000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7117944", "Long": "-121.0986472", "Speed": 0, "TimeStamp": "\/Date(1360696587000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.713725", "Long": "-121.1016722", "Speed": 0, "TimeStamp": "\/Date(1360708755000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7057861", "Long": "-121.0913528", "Speed": 0, "TimeStamp": "\/Date(1360689422000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6993917", "Long": "-121.0852306", "Speed": 0, "TimeStamp": "\/Date(1360695552000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "17.43594370000000", "Long": "78.34167309999998000", "Speed": 0, "TimeStamp": "\/Date(1360690114000+0000)\/" }], "ProfileID": "150", "SOSToken": "1.47846733106848", "TrackingToken": "0.47846733106848", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1616161616@16.com", "MobileNumber": "1616161616", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name16", "UserID": "16", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7009806", "Long": "-121.0825333", "Speed": 0, "TimeStamp": "\/Date(1360709159000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7036194", "Long": "-121.0818167", "Speed": 0, "TimeStamp": "\/Date(1360694965000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7089611", "Long": "-121.0955778", "Speed": 0, "TimeStamp": "\/Date(1360690297000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7117944", "Long": "-121.0986472", "Speed": 0, "TimeStamp": "\/Date(1360696587000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.713725", "Long": "-121.1016722", "Speed": 0, "TimeStamp": "\/Date(1360708755000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7057861", "Long": "-121.0913528", "Speed": 0, "TimeStamp": "\/Date(1360689422000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.6993917", "Long": "-121.0852306", "Speed": 0, "TimeStamp": "\/Date(1360695552000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "17.43594370000000", "Long": "-121.0874667", "Speed": 0, "TimeStamp": "\/Date(1360690114000+0000)\/" }], "ProfileID": "160", "SOSToken": "1.47846733106848", "TrackingToken": "0.47846733106848", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1717171717@17.com", "MobileNumber": "1717171717", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name17", "UserID": "17", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7007", "Long": "-121.0806361", "Speed": 0, "TimeStamp": "\/Date(1360687257000+0000)\/" }], "ProfileID": "170", "SOSToken": "1.15785653481947", "TrackingToken": "0.15785653481947", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "1818181818@18.com", "MobileNumber": "1818181818", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name18", "UserID": "18", "IsSOSOn": true, "IsTrackingOn": false, "LastLocs": [{ "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.698", "Long": "-121.0885417", "Speed": 0, "TimeStamp": "\/Date(1360700177000+0000)\/" }, { "Alt": null, "GeoDirection": 0, "Identifier": null, "Lat": "45.7031972", "Long": "-121.0872306", "Speed": 0, "TimeStamp": "\/Date(1360689839000+0000)\/" }], "ProfileID": "180", "SOSToken": "1.03112425331975", "TrackingToken": "0.03112425331975", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2424242424@24.com", "MobileNumber": "2424242424", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name24", "UserID": "24", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "240", "SOSToken": "1.468159509637797", "TrackingToken": "0.468159509637797", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2525252525@25.com", "MobileNumber": "2525252525", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name25", "UserID": "25", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "250", "SOSToken": "1.329224957188351", "TrackingToken": "0.329224957188351", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2626262626@26.com", "MobileNumber": "2626262626", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name26", "UserID": "26", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "260", "SOSToken": "1.75596076766807", "TrackingToken": "0.75596076766807", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2727272727@27.com", "MobileNumber": "2727272727", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name27", "UserID": "27", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "270", "SOSToken": "1.110586239277153", "TrackingToken": "0.110586239277153", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2828282828@28.com", "MobileNumber": "2828282828", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name28", "UserID": "28", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "280", "SOSToken": "1.928679617316808", "TrackingToken": "0.928679617316808", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "2929292929@29.com", "MobileNumber": "2929292929", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name29", "UserID": "29", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "290", "SOSToken": "1.240371379771383", "TrackingToken": "0.240371379771383", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }, { "ContactInfo": { "Email": "3030303030@30.com", "MobileNumber": "3030303030", "Platform": null, "ToastChannel": null, "ToastExpiry": null }, "Name": "Name30", "UserID": "30", "IsSOSOn": false, "IsTrackingOn": false, "LastLocs": null, "ProfileID": "300", "SOSToken": "1.233214959125806", "TrackingToken": "0.233214959125806", "AscGroups": null, "BuddiesTrackingMe": null, "FBDetails": null, "LiveDetails": null, "MembersITrack": null, "PhoneSetting": null, "PrimeBuddyId": null, "PrimeGroupID": null, "SiteSetting": null }];

function getMarshals(groupID,plotRequired)
{
    var getMarshalUrl = GuardianConfig.ServiceURL+'/GetMarshalList/' + groupID + '/' + Date.now();
    $.ajax({
        url: getMarshalUrl,
        dataType: "json",
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        success: function (data)
        {
            if (data) {
                if (data.DataInfo[0].ResultType == 0) {
                    MarshalList = data.List;
                    if (plotRequired)
                        plotMarshals();
                    else
                        drawMarshalTable();
                    return true;
                }
            }
            return false;
        },
        error: function (e)
        {
            return false;
        }
    });
}

function directionToUser()
{
    var profileID = this.id;
    var marshal = null;
    for (var ctr = 0; ctr < MarshalList.length; ctr++) {
        if (MarshalList[ctr].ProfileID == profileID) {
            marshal = MarshalList[ctr];
            break;
        }
    }
    var marshalCurrentLocation=marshal.LastLocs[marshal.LastLocs.length-1];
    var marshalLocation = new Microsoft.Maps.Location(marshalCurrentLocation.Lat, marshalCurrentLocation.Long);
    var userLocation = new Microsoft.Maps.Location(SubjectUser.currentLocation.lat, SubjectUser.currentLocation.long);

    createDrivingRoute(marshalLocation, userLocation);
}
//var SubjectUser;
function plotMarshals() {
    //SubjectUser = { "profileID":"-1","userID": "-1", "userIndex": "-1", "pin": "-1", "info": { "infobox": "-1", "description": "-1", "address": "-1" }, "currentLocation": { "lat": "-1", "long": "-1" } };

    //MarshalList = [{ "DataInfo": null, "Email": "renukasharath@live.com", "FBAuthID": null, "FBID": null, "LiveDetails": null, "MobileNumber": "+917702690005", "Name": "renuka", "RegionCode": null, "UserID": "d20da772-9bc5-48db-80dd-52c7a61eba45", "IsSOSOn": true, "IsTrackingOn": true, "LastLocs": [{ "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581458804)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581611992)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "45.435943700000000000", "Long": "-121", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374578575943)\/" }], "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "SOSToken": null, "TinyURI": null, "TrackingToken": null, "LocateBuddies": null }];

    if (SubjectUser.userIndex == -1)
        return;
    var pin;
    var infoboxOptions;
    var infobox;
    if (MarshalEntitiesLayer)
        MarshalEntitiesLayer.clear();
  
    //SubjectUser.profileID=dummyLocateBuddies[0].ProfileID;

    $.each(MarshalList, function (index, marshal) {

        if (marshal.ProfileID != null && marshal.LastLocs != null) {
            var assignFlag = false;
            var followingCtr = 0;
            //marshal.LocateBuddies=dummyLocateBuddies;
            var currentLocation = marshal.LastLocs[marshal.LastLocs.length - 1];
            pin = PutPushPin(marshal.Name, currentLocation.Lat, currentLocation.Long, 'marshal');
            if (marshal.LocateBuddies != null) {
                $.each(marshal.LocateBuddies, function (index, user) {
                    if (user.ProfileID == SubjectUser.profileID)
                        assignFlag = true;
                });
                followingCtr = marshal.LocateBuddies.length;
            }


            if (assignFlag) {
                infoboxOptions = {
                    offset: new Microsoft.Maps.Point(0, 100),
                    title: marshal.Name, description: "Following :" + followingCtr, zIndex: 0, visible: true, pushpin: pin, actions: [{ label: 'Unassign', eventHandler: unassignMarshal, id: marshal.ProfileID }, { label: 'Directions', eventHandler: directionToUser, id: marshal.ProfileID }]
                };
            } else {
                infoboxOptions = {
                    offset: new Microsoft.Maps.Point(0, 100),
                    title: marshal.Name, description: "Following :" + followingCtr, zIndex: 0, visible: true, pushpin: pin, actions: [{ label: 'Assign', eventHandler: assignMarshal, id: marshal.ProfileID }, { label: 'Directions', eventHandler: directionToUser, id: marshal.ProfileID }]
                };
            }
            infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(currentLocation.Lat, currentLocation.Long), infoboxOptions);
            MarshalEntitiesLayer.push(infobox);
            MarshalEntitiesLayer.push(pin);

            marshal.pin = pin;
            marshal.infobox = infobox;
        }
    });
}
function unassignMarshal() {
    var profileID=this.id;
    var marshal=null;
    for(var ctr=0;ctr<MarshalList.length;ctr++)
    {
        if(MarshalList[ctr].ProfileID==profileID)
        {
            marshal = MarshalList[ctr];
            break;
        }
    }
   
    if(marshal!=null)
    {
        BuddyMarshalAction(AdminID, GroupID, marshal.ProfileID, marshal.UserID, SubjectUser.profileID, "unassign");
        alert('Request Has been Submitted');

        //alert(marshal.Name +" Unassigned !");
    }else
    {
      //  alert("Please Try Again !");
    }
}

function assignMarshal() {
    var profileID=this.id;
    var marshal=null;
    for (var ctr = 0; ctr < MarshalList.length; ctr++) {
        if (MarshalList[ctr].ProfileID == profileID) {
            marshal = MarshalList[ctr];
            break;
        }
    }
    //var SubjectUser = { "profileID":"-1","userID": "-1", "userIndex": "-1", "pin": "-1", "info": { "infobox": "-1", "description": "-1", "address": "-1" }, "currentLocation": { "lat": "-1", "long": "-1" } };

    if(marshal!=null)
    {
        BuddyMarshalAction(AdminID, GroupID, marshal.ProfileID, marshal.UserID, SubjectUser.profileID, "assign");       
    }
}

function BuddyMarshalAction(AdminID, GroupID, MarshalProfileID, MarshalUserID, TargetUserProfileID,ActionType)
{
    var actionUrl;
    if (ActionType == "unassign")
        actionUrl = GuardianConfig.ServiceURL+'/RemoveBuddyFromMarshal';
    else if(ActionType == "assign")
        actionUrl = GuardianConfig.ServiceURL+'/AssignBuddyToMarshal';
        
    $.ajax({
        url: actionUrl + '/' + AdminID + '/' + GroupID + '/' + MarshalProfileID + '/' + MarshalUserID + '/' + TargetUserProfileID + '/' + Date.now(),
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: 'application/json; charset=utf-8',
        async: true,
        success: function (data)
        {
            if (data.ResultType == 0) {
                alert('Marshal has been assigned successfully.');
                return true;
            } else {
                alert('Unable to assign Marshal to the user. Please try again! If you continue to receive this, contact administrator.');
                return false;
            }
        },
        error: function (data)
        {
           // alert('Network Error');
        }
    } );
}

function deleteMarshal(profileID) {

    //var id = $(this).attr('id');
    var deleteMarshalUrl = GuardianConfig.ServiceURL+'/DeleteMarshal/' + AdminID + '/' + GroupID + '/' + profileID + '/' + Date.now();

    for (i = 0; i < MarshalList.length; i++) {
        if (MarshalList[i].ProfileID == profileID) {
            if (MarshalList[i].LocateBuddies != null) {
                alert("Marshal has buddies assigned to it. Cant be deleted!!");
                return;
            }
        }
    }
    $.ajax({
        url: deleteMarshalUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.ResultType == 0) {

                displayMarshalList();
                alert('Marshal is Deleted !');
                return true;
            }
            else {
                alert("OOPS! Something Went Wrong !");
                return;
            }
        },
        error: function (data) {
    //        alert("Marshal not deleted");
            return;
        }

    });
}

function displayMarshalList() {
    //MarshalList = [{ "DataInfo": null, "Email": "renukasharath@live.com", "FBAuthID": null, "FBID": null, "LiveDetails": null, "MobileNumber": "+917702690005", "Name": "renuka", "RegionCode": null, "UserID": "d20da772-9bc5-48db-80dd-52c7a61eba45", "IsSOSOn": true, "IsTrackingOn": true, "LastLocs": [{ "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581458804)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "18.435943700000000000", "Long": "89.121673099999980000", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374581611992)\/" }, { "Alt": "", "Command": null, "GeoDirection": 0, "Identifier": "1.336173859039812", "Lat": "45.435943700000000000", "Long": "-121", "ProfileID": "3810606c-ceb3-4f61-bdd0-90b26eea78ad", "Speed": 10, "TimeStamp": "\/Date(1374578575943)\/" }], "ProfileID": "38", "SOSToken": null, "TinyURI": null, "TrackingToken": null, "LocateBuddies": null }];
    GroupID = getSession('GroupID');
    AdminID = getSession('AdminID');
    getMarshals(GroupID, false);
    //drawMarshalTable();
}

function drawMarshalTable(){
    var rowhtml = '';
    if (MarshalList != null && MarshalList.length>0) {
        $.each(MarshalList, function (index, marshal) {
            if (marshal.ProfileID != null) {
                var id = marshal.ProfileID;
                rowhtml += '<tr style="background:#f5f5f5"><td>' + marshal.Name + '</td><td>' + marshal.Email + '</td><td>' + marshal.MobileNumber + '</td>' + '<td>' + marshal.IsValidated + '</td>';

                //<a href="#" data-LinkAction="EditCard" style="margin-right:10px;" id="EditButton_' + id + '"><img class="EditImage" src="/Content/Images/edit.png" height="32" width="32"/></a>
                rowhtml += '<td><div title="Delete" onclick="deleteMarshal(\'' + id + '\')" >';
                rowhtml += '<img class="DeleteImage" src="/Content/Images/delete.png" height="32" width="32"></div></td></tr>';
            }
            else {
                $("#marshalTableDiv").hide();
            }
        });

        $("#marshalTableDiv").html('<h3  style="color:#ED7D7D; font:20px helvetica; background:#E2EBFC;  text-align:center; color:black">List of marshals</h3><table class="marshalTable" style="background:#E2EBFC;  margin-top:-20px" width="100%"><thead><tr style="background:#dcdcdc; color:black"><td>Name</td><td>EmailID</td><td>Phone Number</td><td>IsValidated</td><td>Delete</td></tr></thead>'
                                  + '<tbody>' + rowhtml + '</tbody></table>');
        $("#marshalTableDiv").show();
    }
}

var Data = null;
function GetMarshalsToUnAssign() {
    var groupID = getSession('GroupID');
    var getUnAssignMarshalUrl = GuardianConfig.ServiceURL + '/GetMarshalsListToUnAssign/' + groupID + '/' + Date.now();
    $.ajax({
        url: getUnAssignMarshalUrl,
        dataType: "json",
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            Data = data;
            drawMarshalsTableToUnAssign();
            return true;
        },
        error: function (e) {
            return false;
        }
    });
    //drawMarshalsTableToUnAssign();
}


function drawMarshalsTableToUnAssign() {

    $("#manageDiv").hide();
    var id = null;
    var rowhtml = '';
    if (Data != null) {
        for (var key in Data) {

            var obj = Data[key];
            rowhtml += '<tr style="background:#f5f5f5"><td>' + obj.GroupID + '</td><td>' + obj.UserName + '</td><td>' + obj.MarshalName + '</td>';
            rowhtml += '<td><div title="Delete" onclick="deleteMarshalRelation(\'' + obj.TargetUserProfileID + '\',\'' + obj.MarshalUserID + '\')" >';
            rowhtml += '<img class="DeleteImage" src="/Content/Images/delete.png" height="32" width="32"></div></td></tr>';
        }

        $("#marshalTableDivUnassign").html('<h3 style="color:#ED7D7D; font:20px helvetica; background:#E2EBFC;  text-align:center; color:black">List of assigned marshals</h3><table class="marshalTable" style="background:#E2EBFC;  margin-top:-20px" width="100%"><thead><tr style="background:#dcdcdc; color:black"><td>GroupID</td><td>User Name</td><td>Marshal Name</td><td>Unassign</td></tr></thead>' + '<tbody>' + rowhtml + '</tbody></table>');

        $("#unassignDiv").show();
    }
    else
    {
        $("#manageDiv").hide();

        $("#marshalTableDivUnassign").html("<h4>There are NO assigned marshals to unassign<h4>");

        $("#unassignDiv").show();

    }
}

function deleteMarshalRelation(targetUserprofileID, buddyUserID) {

    var unAssignMarshalUrl = GuardianConfig.ServiceURL + '/UnAssignMarshalFromList/' + targetUserprofileID + '/' + buddyUserID + '/' + Date.now();
    $.ajax({
        url: unAssignMarshalUrl,
        dataType: 'json',
        headers: { 'AuthID': getSession('authentication_token'), 'UserType': getSession('userTypeAuth') },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.ResultType == 0) {

                GetMarshalsToUnAssign();
                alert('Marshal is Deleted !');
                return true;
            }
            else {
                alert("OOPS! Something Went Wrong !");
                return;
            }
        },
        error: function (data) {
            alert("Marshal not deleted");
            return;
        }

    });
}


function ShowMarshalsForManaging() {
    $("#unassignDiv").hide();
    $("#manageDiv").show();

}
