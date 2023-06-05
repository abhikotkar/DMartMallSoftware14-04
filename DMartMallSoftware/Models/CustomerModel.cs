
using System.ComponentModel.DataAnnotations;

namespace DMartMallSoftware.Models
{
    public class CustomerModel:BaseModel
    {
        [Key]
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }

        public List<CartModel>? Cartdetails { get; set; }
        public int TotalQuantity { get; set; }
        public float SubTotal { get; set; }
        public float TotalDiscount { get; set; }
        public float TotalAmt { get; set; }
        public float PayAmt { get; set; }
        public float GrandTotal { get; set; }
        public string? Remark { get; set; }
        public int RemarkId { get; set; }
    }

    public class CustomerModelV1 : BaseModel
    {
        [Key]
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }

    }
}
