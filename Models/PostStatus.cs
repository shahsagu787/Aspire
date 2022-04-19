using System;
using System.ComponentModel.DataAnnotations;

namespace Aspire.Models
{
 // store the informaion of post status
    public class PostStatus
    {
        [Key]
        public int PostStatusId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Descriptiion { get; set; }
        [DataType(DataType.Date)]
      //  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
