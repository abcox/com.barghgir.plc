using com.barghgir.plc.infra.common.Encryption;

namespace com.barghgir.plc.infra.test
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        [DataRow("unprotectedString", null, null, null)]
        [DataRow("unprotectedString", "copy-from-keyvault-secret", "copy-from-keyvault-secret", "copy-from-keyvault-secret")]
        public void EncryptDataWithAes_Test(string unprotectedString,
            ref string? key, ref string? vector,string? expectedProtectedString)
        {
            string protectedString = DataProtectionHelper.EncryptDataWithAes(
                unprotectedString, ref key, ref vector);
            Assert.IsNotNull(key);
            Assert.IsNotNull(vector);
            if (expectedProtectedString != null)
                Assert.AreEqual(expectedProtectedString, protectedString);
            string actual = DataProtectionHelper.DecryptDataWithAes(protectedString, key, vector);
            Assert.AreEqual(unprotectedString, actual);
        }
    }
}