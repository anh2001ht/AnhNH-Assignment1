using BusinessObjects;

namespace DataAccess
{
    public class MemberDAO
    {
        public static List<Member> GetMembers()
        {
            var listMembers = new List<Member>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listMembers = context.Members.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listMembers;
        }

        public static Member FindMemberById(int customerId)
        {
            var member = new Member();
            try
            {
                using (var context = new MyDBContext())
                {
                    member = context.Members.SingleOrDefault(c => c.MemberID == customerId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public static Member FindMemberByEmail(string email)
        {
            var member = new Member();
            try
            {
                using (var context = new MyDBContext())
                {
                    member = context.Members.FirstOrDefault(c => c.Email == email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public static void SaveMember(Member member)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Members.Add(member);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateMember(Member member)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Entry(member).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteMember(Member member)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var mb = context
                        .Members
                        .SingleOrDefault(c => c.MemberID == member.MemberID);
                    context.Members.Remove(mb);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
