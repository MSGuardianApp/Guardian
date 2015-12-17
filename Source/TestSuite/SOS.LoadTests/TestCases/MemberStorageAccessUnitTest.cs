using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuardianLoadTestProject
{
    [TestClass]
    public class MemberStorageAccessUnitTests
    {
        //string path = @"E:\Guardian\LoadTests\3_MultipleProfileIDs.log";
        //[TestMethod]
        //public void Storage_GetMyBuddiesUnitTest()
        //{
        //    DateTime startTime = DateTime.Now;
        //    MemberStorageAccess storage = new MemberStorageAccess();
        //    var res = storage.GetBuddiesForProfileID("51d7aa1d-e172-40e3-9579-1be17fab7a75");
        //    var endTime = DateTime.Now;
        //    var a = (endTime - startTime).TotalMilliseconds;

        //    //if (res != null)
        //    System.IO.File.AppendAllText(path, "GOOD - R: " + a.ToString() + "\t S: " + startTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\t E: " + endTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + Environment.NewLine);
        //    //else
        //    //    System.IO.File.AppendAllText(path, "FAIL - R: " + a.ToString() + "\t S: " + startTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\t E: " + endTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + Environment.NewLine);

        //    Assert.AreEqual(HttpStatusCode.OK, HttpStatusCode.OK);
        //}

        //string[] arr = { "51d7aa1d-e172-40e3-9579-1be17fab7a75", "3869c050-79c1-48f0-9737-48a0e703c39c", "2cb3d686-7a74-4daf-8375-e5c6a3526604", "53186aa2-ab37-4da7-8c1b-2653f64b04bb" 
        //               ,"592bcd14-d9e6-46ae-abf3-830430a42df9","6a3443a6-e295-4dd6-b8c4-834584ee7090","c66b5e68-c6ca-4c1d-8632-233d3f2cf21a","da4a2c28-b10e-4daa-a509-3d4cbc603049"};

        //[TestMethod]
        //public void Storage_GetMyBuddiesMultipleUnitTest()
        //{
        //    int rand = new Random().Next(0, 7);
        //    DateTime startTime = DateTime.Now;
        //    MemberStorageAccess storage = new MemberStorageAccess();
        //    var res = storage.GetBuddiesForProfileID(arr[rand]);
        //    var endTime = DateTime.Now;
        //    var a = (endTime - startTime).TotalMilliseconds;

        //    //if (res != null)
        //    System.IO.File.AppendAllText(path, "GOOD - R: " + a.ToString() + "\t S: " + startTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\t E: " + endTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + Environment.NewLine);
        //    //else
        //    //    System.IO.File.AppendAllText(path, "FAIL - R: " + a.ToString() + "\t S: " + startTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\t E: " + endTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + Environment.NewLine);

        //    Assert.AreEqual(HttpStatusCode.OK, HttpStatusCode.OK);
        //}
    }
}
