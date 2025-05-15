using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace nguyenvanlai_2122110481_asp.net.Model
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
