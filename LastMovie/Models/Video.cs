using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Movie_ID { get; set; }
        public string Youtube { get; set; }
        public string MovieImg { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public string ImgList { get; set; }
    }
}
