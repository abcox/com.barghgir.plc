using com.barghgir.plc.common.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.common.test
{
    [TestClass]
    public class JwtTokenHelperTests
    {
        [TestMethod]    // **** requires valid token NOT HAVING admin role ****
        [DataRow("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InNvbG1hekBiYXJnaGdpciIsInN1YiI6IjEiLCJqdGkiOiI2ZTNiODUxMS1iZWJjLTQ2MWQtYWM5NC1hYzM0NjQxNTMzNzYiLCJpYXQiOjE2NjkzMTM2MTQsImNvbnRhY3QiOiJTb2xtYXogQmFyZ2hnaXIiLCJuYmYiOjE2NjkzMTM2MTQsImV4cCI6MTY2OTMxNTQxNCwiaXNzIjoiY29tLmJhcmdoZ2lyLnBsYy5hcGkiLCJhdWQiOiJhZGFtQGFkYW1jb3gubmV0In0.sw35uO72S5k83rv3k9mMrKqZ38IrZjHLLz96Qee_KD8")]
        public void JwtTokenHelperTest_Valid_No_Admin(string tokenString)
        {
            var isValidToken = JwtTokenHelper.IsTokenValid(tokenString);
            Assert.IsTrue(isValidToken);
            var claims = JwtTokenHelper.GetJwtTokenClaims(tokenString);
            Assert.IsNotNull(claims);
            var roles = JwtTokenHelper.GetRoles(claims);
            Assert.IsNotNull(roles);
            var isAdmin = roles.Contains("admin");
            Assert.IsFalse(isAdmin);
        }

        [TestMethod]    // **** requires valid token with admin role ****
        [DataRow("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InNvbG1hekBiYXJnaGdpciIsInN1YiI6IjEiLCJqdGkiOiIwZjIxNTNkNC01MDFkLTQ0MmYtOGIwYi1hZGQ3NmVhYTM0MTciLCJpYXQiOjE2NjkzMTMyMTEsImNvbnRhY3QiOiJTb2xtYXogQmFyZ2hnaXIiLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE2NjkzMTMyMTEsImV4cCI6MTY2OTMxNTAxMSwiaXNzIjoiY29tLmJhcmdoZ2lyLnBsYy5hcGkiLCJhdWQiOiJhZGFtQGFkYW1jb3gubmV0In0.SeCseEcmeH0A_9Qd6HeSzqYNM4_ks_BNc-WFghDfZMY")]
        public void JwtTokenHelperTest_Valid_Admin(string tokenString)
        {
            var isValidToken = JwtTokenHelper.IsTokenValid(tokenString);
            Assert.IsTrue(isValidToken);
            var claims = JwtTokenHelper.GetJwtTokenClaims(tokenString);
            Assert.IsNotNull(claims);
            var roles = JwtTokenHelper.GetRoles(claims);
            Assert.IsNotNull(roles);
            var isAdmin = roles.Contains("admin");
            Assert.IsTrue(isAdmin);
        }

        [TestMethod]    // **** requires valid (expired) token ****
        [DataRow("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InNvbG1hekBiYXJnaGdpciIsInN1YiI6IjEiLCJqdGkiOiIzODljYmY5My1iMjdkLTQ4ZTItODAyNi03NjU0Njg4Yjc5Y2EiLCJpYXQiOjE2NjkzMTA0ODEsImNvbnRhY3QiOiJTb2xtYXogQmFyZ2hnaXIiLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE2NjkzMTA0ODEsImV4cCI6MTY2OTMxMjI4MSwiaXNzIjoiY29tLmJhcmdoZ2lyLnBsYy5hcGkiLCJhdWQiOiJhZGFtQGFkYW1jb3gubmV0In0.J0x7pcS7B1n6HTsfJbfcQzlENM4RuufJ3BVa1b73bkY")]
        public void JwtTokenHelperTest_Invalid_Expired(string tokenString)
        {
            var isValidToken = JwtTokenHelper.IsTokenValid(tokenString);
            Assert.IsFalse(isValidToken);
        }
    }
}
