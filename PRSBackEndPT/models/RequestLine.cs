using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis;

namespace PRSBackEndPT.models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }

        public int RequestId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // foreign key -- prevents loop between req and reqline
        [ForeignKey(nameof(RequestId))]
        public Request? Request { get; set; }

        // foreign key
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

    }
}
