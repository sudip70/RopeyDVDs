using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeNumber { get; set; }
        public string? LoanTypes { get; set; }
        public int LoanDuration { get; set; }
        public ICollection<Loan>? Loan { get; set; }
    }
}