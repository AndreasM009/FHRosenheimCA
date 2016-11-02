using System;
using System.ComponentModel.DataAnnotations;

namespace Fhr.Contacts.WebApi.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        [StringLength(512)]
        public string Surename { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}