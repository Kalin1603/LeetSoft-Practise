using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Event_Management.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(150)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(10, 18)]
        public int Age { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
