using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        [Required, StringLength(40)]
        public string CategoryName { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Product { get; set; }
    }
}
