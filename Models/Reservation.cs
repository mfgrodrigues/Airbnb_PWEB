using System;
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

        public int PropertyId { get; set; }

        public string UserId { get; set; }    
    }
}
