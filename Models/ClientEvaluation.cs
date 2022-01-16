using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class ClientEvaluation
    {
        public int ClientEvaluationId { get; set; }

        public Company Company { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Maximum 200 characters")]
        public string Comment { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
