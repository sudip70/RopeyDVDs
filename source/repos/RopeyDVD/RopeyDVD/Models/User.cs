using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class User
    {
        [Key]
        public int UserNumber { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? UserPassword { get; set; }
    }
}