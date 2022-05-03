using System.ComponentModel.DataAnnotations;

namespace RopeyDVD.Models
{
    public class Actor
    {
        [Key]
        public int ActorNumber { get; set; }
        public string? ActorSurName { get; set; }
        public string? ActorFirstName { get; set; }

        public ICollection<CastMember>? CastMember { get; set; }
    }
}
