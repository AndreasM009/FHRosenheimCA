using System;
using System.ComponentModel.DataAnnotations;

namespace Fhr.Contacts.WebApi.Models
{
    public class ContactSettings
    {
        public Guid UserId { get; set; }
        [Required]
        public bool SortBySurename { get; set; } = true;
        [Required]
        public bool DisplaySurenameFirst { get; set; } = true;
    }
}