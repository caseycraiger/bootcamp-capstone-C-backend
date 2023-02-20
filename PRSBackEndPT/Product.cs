using System.ComponentModel.DataAnnotations;

namespace PRSBackEndPT
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string PartNbr { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [StringLength(30)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; }

        public int VendorId { get; set; }

    }
}
