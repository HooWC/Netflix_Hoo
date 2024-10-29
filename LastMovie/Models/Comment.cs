using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Movie_Id { get; set; }
        public string User_Id { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public string CreatedDate { get; set; }
        public string CommentWord { get; set; }
        public string Head { get; set; }
        public string User_Name { get; set; }
        public string ConnID { get; set; }
    }
}
