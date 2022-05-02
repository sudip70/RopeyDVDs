using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RopeyDVD.Models
{
    public class DVDTitle
    {

        [Key]
        public int DVDNumber { get; set; }
        public string? DVDTitles { get; set; }
        public DateTime DateReleased { get; set; }
        public float StandardCharge { get; set; }
        public float PenaltyCharge { get; set; }
        public int CategoryNumber { get; set; }
        [ForeignKey("CategoryNumber")]
        public DVDCategory? DVDCategory { get; set; }
        public int StudioNumber { get; set; }
        [ForeignKey("StudioNumber")]
        public Studio? Studio { get; set; }
        public int ProducerNumber { get; set; }
        [ForeignKey("ProducerNumber")]
        public Producer? Producer { get; set; }
        public ICollection<DVDCopy>? DVDCopy { get; set; }
        public ICollection<CastMember>? CastMember { get; set; }

    }
}