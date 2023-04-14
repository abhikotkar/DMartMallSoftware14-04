namespace DMartMallSoftware.Models
{
    public class DiscountModel:BaseModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string AddedBy { get; set; }
        public float DiscountPerc { get; set; }
    }
}
