using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class InstallmentRecord
    {
        [Key]
        public int Id { get; set; }
        public string User_ID { get; set; }
        public string Tr_ID { get; set; }
        public string PaymentStartDate { get; set; }
        public string PaymentDueDate { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public string PaidTime { get; set; }
        public string Movie_ID { get; set; }
    }
}
