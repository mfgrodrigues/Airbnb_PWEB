using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }

        public string Nome { get; set; }

        public IEnumerable<ApplicationUser> Utilizadores { get; set; }
    }
}
