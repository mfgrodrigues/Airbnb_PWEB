using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_PWEB.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; } 
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]

        public DateTime CheckOut { get; set; }

        public string UserId { get; set; }

        public int PropertyId { get; set; }

        public Property Property { get; set; }

        public Evaluation Evaluation { get; set; }

        public virtual List<Item> ItemsChecked { get; set; }

        public bool statusReservation { get; set; }
    }
}
