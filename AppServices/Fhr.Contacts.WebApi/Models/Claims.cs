using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fhr.Contacts.WebApi.Models
{
    public class Claims
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string Surename { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}