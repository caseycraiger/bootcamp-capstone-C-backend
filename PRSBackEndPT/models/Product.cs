using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackEndPT.models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string PartNbr { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "Decimal (11,2)")]
        public decimal Price { get; set; }

        [StringLength(30)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; }
        
        [JsonIgnore]
        public int VendorId { get; set; }
        public Vendor? Vendor { get; set; }

    }
}
