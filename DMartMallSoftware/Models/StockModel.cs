using System.ComponentModel.DataAnnotations.Schema;

namespace DMartMallSoftware.Models
{
    public class StockModel : BaseModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public int DiscountId { get; set; }
        [NotMapped]
        public float DiscountPerc { get; set; }
        [NotMapped]
        public string Unit { get; set; }
    }
}
