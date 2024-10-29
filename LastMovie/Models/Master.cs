using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Master
    {
        [Key]
        public int Id { get; set; }
        public string Master_Id { get; set; }
        public string MasterName { get; set; }
        public string Master_Info { get; set; }
        public string MasterImg { get; set; }
    }
}
