using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackEndPT.models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public int RequestId { get; set; }
        public Request? Request { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public decimal Price { get; set; }

    }
}
