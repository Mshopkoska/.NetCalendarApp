using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CALENDAR_Version_3._0.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Event> Events { get; set; }
    }
}
