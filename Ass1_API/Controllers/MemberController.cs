using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Impl;

namespace Ass1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private IMemberRepository repository = new MemberRepository();

        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetMembers() => repository.GetMembers();
        [HttpGet("{id}")]
        public ActionResult<Member> GetMemberById(int id) => repository.GetMemberById(id);
        [HttpGet("Email/{email}")]
        public ActionResult<Member> GetMemberByEmail(string email) => repository.GetMemberByEmail(email);
        [HttpPost]
        public IActionResult PostMember(MemberRequest memberRequest)
        {
            var member = new Member
            {
                Email = memberRequest.Email,
                City = memberRequest.City,
                Country = memberRequest.Country,
                Password = memberRequest.Password,
            };
            repository.SaveMember(member);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMember(int id)
        {
            var c = repository.GetMemberById(id);
            if (c == null)
            {
                return NotFound();
            }
            repository.DeleteMember(c);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult PutMember(int id, MemberRequest memberRequest)
        {
            var member = repository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }

            member.Email = memberRequest.Email;
            member.City = memberRequest.City;
            member.Country = memberRequest.Country;

            if (memberRequest.Password != null && member.Password != memberRequest.Password)
            {
                member.Password = memberRequest.Password;
            }

            repository.UpdateMember(member);
            return NoContent();
        }
    }
}
