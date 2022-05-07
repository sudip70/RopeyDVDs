using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class Producer
    {
        [Key]
        public int ProducerNumber { get; set; }
        public string? ProducerName { get; set; }
        public ICollection<DVDTitle>? DVDTitle { get; set; }
    }
}