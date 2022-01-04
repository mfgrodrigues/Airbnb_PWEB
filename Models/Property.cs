using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_PWEB.Models
{
    public class Property
    {
        public int Id { get; set; }
        [Display(Name = "Título")]
        public string Tittle { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Preço/Noite")]
        public double pricePerNigth { get; set; }
        [Display(Name = "Morada")]
        public string Address { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "Comodidades")]
        public string Amenities { get; set; }
        public List<PropertyImage> Images { get; set; }
    }

}
