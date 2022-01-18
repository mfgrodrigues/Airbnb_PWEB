using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_PWEB.Models
{
    
    public class CheckItem
    {
        public int CheckItemId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Maximum 200 characters")]
        public string Name { get; set; }
        public int CheckListId { get; set; }
        public CheckList CheckList { get; set; }

        public bool isCheck { get; set; }
    }

}
