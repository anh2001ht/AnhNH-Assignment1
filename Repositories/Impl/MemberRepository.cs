using BusinessObjects;
using DataAccess;

namespace Repositories.Impl
{
    public class MemberRepository : IMemberRepository
    {
        public void DeleteMember(Member member) => MemberDAO.DeleteMember(member);
        public Member GetMemberByEmail(string email) => MemberDAO.FindMemberByEmail(email);
        public Member GetMemberById(int id) => MemberDAO.FindMemberById(id);
        public List<Member> GetMembers() => MemberDAO.GetMembers();
        public List<Order> GetOrders(int memberId) => OrderDAO.FindAllOrdersByMemberId(memberId);
        public void SaveMember(Member member) => MemberDAO.SaveMember(member);
        public void UpdateMember(Member member) => MemberDAO.UpdateMember(member);
    }
}
