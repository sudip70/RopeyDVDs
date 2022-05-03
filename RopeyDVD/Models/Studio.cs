
using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class Studio
    {
        [Key]
        public int StudioNumber { get; set; }
        public string? StudioName { get; set; }
        public ICollection<DVDTitle>? DVDTitle { get; set; }
    }
}
