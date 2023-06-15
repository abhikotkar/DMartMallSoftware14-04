using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMartMallSoftware.Models
{
    public class CartModel:BaseModel
    {

        public int SrNo { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustId { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Product is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Unit is required")]
        public string? Unit { get; set; }

        [NotMapped]
        public float Price { get; set; }
        public int Quentity { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        public int DiscountId { get; set; }

        [NotMapped]
        public float DiscountPerc { get; set; }
        public float Discount { get; set; }

        [NotMapped]
        public float Amount { get; set; }
        public float TotalAmount { get; set; }
        public float TotalAmt { get; set; }
        public float TotalDiscount { get; set; }
        public float NetAmount { get; set; }
        public float NetAmt { get; set; }
    }

    public class OrderItemModel : BaseModel
    {

        public int SrNo { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Product is required")]
        public string? Name { get; set; }
        public int UnitId { get; set; }
        [Required(ErrorMessage = "Unit is required")]
        public string? Unit { get; set; }

        [NotMapped]
        public float Price { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        public int DiscountId { get; set; }
        [NotMapped]
        public float DiscountPerc { get; set; }
        public float Discount { get; set; }
        [NotMapped]
        public float Amount { get; set; }
        public float TotalAmount { get; set; }
        public float TotalAmt { get; set; }
        public float TotalDiscount { get; set; }
        public float NetAmount { get; set; }
        public float NetAmt { get; set; }
    }
}