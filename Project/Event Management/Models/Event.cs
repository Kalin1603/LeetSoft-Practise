using System.ComponentModel.DataAnnotations;

namespace Event_Management.Models
{
    public class Event
    {
        public int Id { get; set; }

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
