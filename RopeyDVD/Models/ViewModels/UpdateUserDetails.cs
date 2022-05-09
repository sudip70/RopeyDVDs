using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models.ViewModels
{
    public class UpdateUserDetails
    {
        public IdentityUser? User;
        public string? Id { get; set; }
        [Required, Display(Name = "Username")]
        public string? UserName { get; set; }
        [Required, Display(Name = "Email")]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public String? NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not macth!")]
        public string? ConfirmPassword { get; set; }

    }
}
