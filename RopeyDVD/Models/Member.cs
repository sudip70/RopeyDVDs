using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class Member
    {
        [Key]
        public int MemberNumber { get; set; }
        public string? MemberLastName { get; set; }
        public String? MemberFirstName { get; set; }
        public string? MemberAddress { get; set; }
        public DateTime MemberDOB { get; set; }
        public MembershipCategory? MembershipCategory { get; set; }
        public ICollection<Loan>? Loan { get; set; }
    }
}