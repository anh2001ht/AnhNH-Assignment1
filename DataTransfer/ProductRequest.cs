using System.ComponentModel.DataAnnotations;

namespace DataTransfer
{
    public class ProductRequest
    {
        public int ProductID { get; set; }
        [Required, StringLength(40)]
        public string ProductName { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int UnitPrice { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int UnitsInStock { get; set; }
        public int CategoryID { get; set; }
        public double Weight { get; set; }
    }
}
