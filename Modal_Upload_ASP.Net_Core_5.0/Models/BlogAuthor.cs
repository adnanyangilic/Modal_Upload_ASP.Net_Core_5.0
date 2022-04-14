using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modal_Upload_ASP.Net_Core_5._0.Models
{
    public class BlogAuthor
    {
       [Key]
        public int BlogAuthorId { get; set; }



        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Blog Author Name")]
        [Required(ErrorMessage = "This Field is required.")]
        public string BlogAuthorName { get; set; }


        [DisplayName("Image")]
        public string Imagepath { get; set; }



    }
}
