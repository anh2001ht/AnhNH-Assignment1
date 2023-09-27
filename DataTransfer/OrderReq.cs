using System.ComponentModel.DataAnnotations;

namespace DataTransfer
{
    public class OrderReq
    {
        public int OrderID { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string Freight { get; set; }
        [Required]
        public int MemberID { get; set; }
    }
}
