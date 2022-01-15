using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_PWEB.Models
{
    public class Property
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tittle")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Minimum 8 characters, maximum 50")]
        public string Tittle { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Price/Night")]
        [Required]
        [Range(0,double.MaxValue,ErrorMessage ="Please enter positive or null value")]
        public double pricePerNigth { get; set; }
        [Display(Name = "Address")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Minimum 8 characters, maximum 100")]
        [Required]
        public string Address { get; set; }
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Maximum 50")]
        public string City { get; set; }
        [Display(Name = "Amenities")]
        [StringLength(100, ErrorMessage = "Maximum 100")]
        [DataType(DataType.MultilineText)]
        public string Amenities { get; set; }
        [Display(Name = "Images")]

        [Required]
        public List<PropertyImage> Images { get; set; }

        public List<Reservation> Reservations { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Company Company { get; set; }
    }

}
