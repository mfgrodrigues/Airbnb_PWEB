using System.Collections.Generic;

namespace Airbnb_PWEB.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool isCheckEntry { get; set; }

        public bool isCheckExit { get; set; }

        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; }
    }

    public class ItemViewModel
    {
        public List<Item> Itens { get; set; }
    }
}
