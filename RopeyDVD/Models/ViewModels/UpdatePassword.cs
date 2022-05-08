using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models.ViewModels
{
    public class UpdatePassword
    {
        [Required, DataType(DataType.Password), Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public String? NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not macth!")]
        public string? ConfirmPassword { get; set; }
    }
}
