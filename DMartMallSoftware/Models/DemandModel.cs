using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMartMallSoftware.Models
{
    public class DemandModel:BaseModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string Name { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }
        public int PayStatusId { get; set; }
        public int StatusId { get; set; }
        public int DealerId { get; set; }
        [NotMapped]
        public string Status { get; set; }
        [NotMapped]
        public string PayStatus { get; set; }
        [NotMapped]
        public string Unit { get; set; }
        [NotMapped]
        public string Dealer { get; set; }
    }
}
