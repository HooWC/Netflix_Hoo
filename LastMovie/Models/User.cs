using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string AddressZip { get; set; }
        public string City { get; set; }
        public DateTime CreateDate { get; set; }
        public string Head { get; set; }
        public double Money { get; set; }
    }
}
