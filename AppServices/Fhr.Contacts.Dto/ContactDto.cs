using System;

namespace Fhr.Contacts.Dto
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }        
        public string Name { get; set; }        
        public string Surename { get; set; }        
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}