using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class ClientEvaluation
    {
        public int ClientEvaluationId { get; set; }

        public string Comment { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
