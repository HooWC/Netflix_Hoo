using System.ComponentModel.DataAnnotations;

namespace LastMovie.Models
{
    public class Tr
    {
        [Key]
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string RoleID { get; set; }
        public string Name { get; set; }
        public double BeforeTotal { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public string PaidTime { get; set; }//付费时间
        public string TransactionStatus { get; set; }//状态  pending
        public string CartList { get; set; }
        public string BillingAddress { get; set; }//  地址
        public double Tax { get; set; }
        public bool InstallmentStatus { get; set; }
    }
}
