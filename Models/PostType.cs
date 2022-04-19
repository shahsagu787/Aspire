using System;
using System.ComponentModel.DataAnnotations;

namespace Aspire.Models
{

    // stores information of Post Type
    public class PostType
    {
        [Key]
        public int PostTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Descriptiion { get; set; }

        [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
