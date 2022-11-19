using com.barghgir.plc.data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace com.barghgir.plc.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        [HttpGet]
        [Route("admin/member/list", Name = "GetMemberList")]
        public IEnumerable<Member>? GetMemberList()
        {
            return new List<Member>();
        }

        [HttpPost]
        [Route("member/create", Name = "CreateMember")]
        public Member? CreateMember(Member member)
        {
            // add to database

            return member;
        }

        [HttpPut]
        [Route("member/verify", Name = "VerifyMember")]
        public Member? VerifyMember(Member member)
        {
            // add to database

            return member;
        }
    }
}
