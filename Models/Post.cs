using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspire.Models
{
    // stores the information of a Post
    public class Post
    {
        [Key] // Primery Key
        public int PostId { get; set; } // post id
        //public string Type { get; set; } = "New Application";// Type of Post (New Application / Updation in existing one)
        //public string  { get; set; } = "Open"; // status of the Post (Open, In Progress, Close)

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; } // Title of the Post

        [Required]
        [DisplayName("Discription")]
        public string Discription { get; set; } // Post description contains detail information (features, technologies, etc)

        public int Support { get; set; } = 0; // number of supports to the post

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PostedOn { get; set; } = DateTime.Now; // date on which it was posted.

        [ForeignKey("PostStatusId")]
        [DisplayName("Post Status")]
        public int PostStatusId { get; set; }
        public PostStatus? PostStatus { get; set; } // status of the Post (Open, In Progress, Close)

        [ForeignKey("PostTypeId")]
        [DisplayName("Post Type")]
        public int PostTypeId { get; set; }
        public PostType? PostType { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
