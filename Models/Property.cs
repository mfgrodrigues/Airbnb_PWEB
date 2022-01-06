using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_PWEB.Models
{
    public class Property
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Título")]
        public string Tittle { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Preço/Noite")]
        [Required]
        [Range(0,double.MaxValue,ErrorMessage ="Please enter positive or null value")]
        public double pricePerNigth { get; set; }
        [Display(Name = "Morada")]
        [Required]
        public string Address { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "Comodidades")]
        public string Amenities { get; set; }
        [Display(Name = "Imagens")]

        [Required]
        public List<PropertyImage> Images { get; set; }

        public List<Reservation> Reservations { get; set; }
    }

}
