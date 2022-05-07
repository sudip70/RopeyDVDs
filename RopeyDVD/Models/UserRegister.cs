using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class UserRegister
    {
        [Key]
        public int UserNumber { get; set; }
        [Required(ErrorMessage ="User Name is empty")]
        public string? UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Email is empty")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "User Password is empty")]
        public string? UserPassword { get; set; }
    }
}