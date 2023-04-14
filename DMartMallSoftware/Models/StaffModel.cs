
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace DMartMallSoftware.Models
{
    public class StaffModel:BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int SrNo { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
                          
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public string AprovedBy { get; set; }
        public string Address { get; set; }

        [StringLength(12)]
        public string MobileNo { get; set; }
        public float Salary { get; set; }
        public int UserTypeId { get; set; }


        public bool IsConfirmed { get; set; }

        public int QuetionId { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public DateTime JoinDate { get; set; }

        [NotMapped]
        public string UserType { get; set; }
        [NotMapped]
        public int Quetion { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "AdminUniqueId")]
        public string AdminUniqueId { get; set; }

    }
}
