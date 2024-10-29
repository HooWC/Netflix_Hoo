using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string RoleID { get; set; }
        public string UserName { get; set; }
        public string MovieImg { get; set; }
        public string MovieName { get; set; }
        public string MovieID { get; set; }
        public int Quantity { get; set; }
        public int LastQuantity { get; set; }
        public double MoviePrice { get; set; }
        public bool Buy { get; set; }
        public bool IsSelected { get; set; }
        public int InstallmentTime { get; set; }
        public bool IsInstallmentProduct { get; set; }
        public int InstallmentLastTime { get; set; }
        public bool InstallmentLastBuy { get; set; }
    }
}
