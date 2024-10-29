using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class MovieProduct
    {
        [Key]
        public int Id { get; set; }
        public string Movie_ID { get; set; }
        public string RoleId { get; set; }
        public string MovieImg { get; set; }
        public string MovieName { get; set; }
        public string MovieInfo { get; set; }
        public double MoviePrice { get; set; }
        public double MovieDelPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public long Like { get; set; }
        public long BuyCount { get; set; }
        public bool IsInstallmentProduct { get; set; }
        public string ImgList { get; set; }
    }
}
