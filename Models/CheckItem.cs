using System.Collections.Generic;

namespace Airbnb_PWEB.Models
{
    
    public class CheckItem
    {
        public int CheckItemId { get; set; }
        public string Name { get; set; }
        public bool IsCheck { get; set; }
        public int CheckListId { get; set; }
        public CheckList CheckList { get; set; }
    }

}
