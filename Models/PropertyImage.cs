using System;

namespace Airbnb_PWEB.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte[] Data { get; set; }
    }
}
