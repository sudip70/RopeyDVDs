using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class Member
    {
        [Key]
        public int MemberNumber { get; set; }
        public string? MemberLastName { get; set; }
        public int MemberFirstName { get; set; }
        public string? MemberAddress { get; set; }
        public DateTime MemberBirthOfDate { get; set; }
        public MembershipCategory? MembershipCategory { get; set; }
    }
}