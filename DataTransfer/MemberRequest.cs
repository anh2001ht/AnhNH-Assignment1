using System.ComponentModel.DataAnnotations;

namespace DataTransfer
{
    public class MemberRequest
    {
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
    }
}
