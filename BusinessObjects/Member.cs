using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class Member
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberID { get; set; }
        [Required, StringLength(40)]
        public string Email { get; set; }
        [Required, StringLength(40)]
        public string City { get; set; }
        [Required, StringLength(40)]
        public string Country { get; set; }
        [MinLength(5)]
        [MaxLength(255)]
        public string? Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
