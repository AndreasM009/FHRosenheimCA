using System;

namespace Fhr.Contacts.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
