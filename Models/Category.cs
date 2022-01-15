using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 characters, maximum 50!")]
        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
