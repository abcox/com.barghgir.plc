using com.barghgir.plc.data.Helpers;

namespace com.barghgir.plc.data.test
{
    [TestClass]
    public class SqlHelperTests
    {
        [TestMethod]
        public void SeedTablesFromJsonFilesTest()
        {
            SqlHelper.SeedTablesFromJsonFiles().Wait();
        }
    }
}