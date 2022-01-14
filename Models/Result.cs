using System.Collections.Generic;

namespace Airbnb_PWEB.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public ICollection<CheckItem> Itens { get; set; }

    }
}
