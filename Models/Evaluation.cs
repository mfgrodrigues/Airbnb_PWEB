namespace Airbnb_PWEB.Models
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }
        //[Required]
        public string Comment { get; set; }
        //[Required]
        public int Classification { get; set; }

        public string UserId { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } 

    }
}
