using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
        public string CreatedDate { get; set; }
    }
}
