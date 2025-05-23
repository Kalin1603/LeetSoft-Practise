﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Models
{
    public class UserEvent
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "User name")]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Event title")]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public string Status { get; set; } = "Invited";
    }
}
