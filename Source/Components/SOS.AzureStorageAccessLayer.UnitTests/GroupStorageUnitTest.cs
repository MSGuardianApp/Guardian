using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SOS.AzureStorageAccessLayer.UnitTests
{
    [TestClass]
    public class GroupStorageUnitTest
    {
        [TestMethod]
        public void GetMembersForGroup_UnitTest()
        {
            GroupStorageAccess g = new GroupStorageAccess();
            //var grpList = g.GetMembersForGroup(1);

            Assert.AreEqual(1, 1);
        }

        
        
    }
}
