using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RopeyDVD.Models
{
    public class Loan
    {
        [Key]
        public int LoanNumber { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime? DateReturned { get; set; }
        public int LoanTypeNumber { get; set; }
        [ForeignKey("LoanTypeNumber")]
        public LoanType? LoanType { get; set; }
        public int CopyNumber { get; set; }
        [ForeignKey("CopyNumber")]
        public DVDCopy? DVDCopy { get; set; }
        public int MemberNumber { get; set; }
        [ForeignKey("MemberNumber")]
        public Member? Member { get; set; }
    }
}