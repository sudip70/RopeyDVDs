using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RopeyDVD.Models
{
    public class DVDCopy
    {
        [Key]
        public int CopyNumber { get; set; }
        public DateTime DatePurchased { get; set; }
        public int DVDNumber { get; set; }
        [ForeignKey("DVDNumber")]
        public DVDTitle? DVDTitle { get; set; }
        public ICollection<Loan>? Loan { get; set; }
    }
}