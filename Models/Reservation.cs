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

        public int PropertyId { get; set; }

        public Property Property { get; set; }

        public Evaluation Evaluation { get; set; }

        public StatusReservation Status { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ClientEvaluation ClientEvaluation { get; set; }

        [DataType(DataType.MultilineText)]
        public string ApprovalComment { get; set; }

        public Result ResultEntry { get; set; }

        public Result ResultExit { get; set; }

        public List<ReservationImage> ImagesReservation { get; set; }
    }

        public enum StatusReservation { Pending, Canceled, Approved, Checkin, Checkout }
}
