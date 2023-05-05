using System.ComponentModel.DataAnnotations.Schema;

namespace DMartMallSoftware.Models
{
    public class ProductModel:BaseModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quentity { get; set; }
        public int Quantity { get; set; }
        public int DiscountId { get; set; }
        public int UnitId { get; set; }
        [NotMapped]
        public float DiscountPerc { get; set; }
    }

}
