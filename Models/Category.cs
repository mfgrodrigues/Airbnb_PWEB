using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
