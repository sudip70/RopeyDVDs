using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class UserLoginModel
    {

        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? UserPassword { get; set; }
    }
}
