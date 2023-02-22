using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackEndPT.models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Justitification { get; set; }

        [StringLength(80)]
        public string? RejectionReason { get; set; }

        [StringLength(20)]
        public string DeliveryMode { get; set; }

        public DateTime SubmittedDate { get; set; }

        public DateTime DateNeeded { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [Column(TypeName = "Decimal (11,2)")]
        public decimal Total { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
