using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string? CreatorId { get; set; }
        public User? Creator { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }
    }
}
