using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackEndPT.models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }

        public int RequestId { get; set; }
        
        public int ProductId { get; set; }
        
        public int Quantity { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
        public Request? Request { get; set; }
    }
}
