using BusinessObjects;


namespace Repositories
{
    public interface IMemberRepository
    {
        void SaveMember(Member member);
        Member GetMemberById(int id);
        Member GetMemberByEmail(string email);
        List<Member> GetMembers();
        void UpdateMember(Member member);
        void DeleteMember(Member member);
        List<Order> GetOrders(int memberId);
    }
}
