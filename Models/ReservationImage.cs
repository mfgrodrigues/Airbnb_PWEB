using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airbnb_PWEB.Models
{
    public class ReservationImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte[] Data { get; set; }
        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; }
    }
}
