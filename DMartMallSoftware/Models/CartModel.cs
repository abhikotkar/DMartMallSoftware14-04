using System.ComponentModel.DataAnnotations.Schema;

namespace DMartMallSoftware.Models
{
    public class CartModel:BaseModel
    {

        public int SrNo { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        public string? Name { get; set; }

        [NotMapped]
        public float Price { get; set; }
        public int Quentity { get; set; }
        public int DiscountId { get; set; }

        [NotMapped]
        public float DiscountPerc { get; set; }
        public float Discount { get; set; }

        [NotMapped]
        public float Amount { get; set; }
        public float TotalAmount { get; set; }
        public float TotalDiscount { get; set; }
        public float NetAmount { get; set; }
    }
}