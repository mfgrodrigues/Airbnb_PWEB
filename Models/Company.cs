using System.Collections.Generic;

namespace Airbnb_PWEB.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        public ApplicationUser Owner { get; set; }

        public virtual ICollection<ApplicationUser> Employeers { get; set; }

        //public virtual ICollection<Property> Properties { get; set; }
    }
}
