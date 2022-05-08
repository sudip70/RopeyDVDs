using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int MembershipCategoryNumber { get; set; }
        [ForeignKey("MembershipCategoryNumber")]
        public MembershipCategory? MembershipCategory { get; set; }
        public ICollection<Loan>? Loan { get; set; }
    }
}