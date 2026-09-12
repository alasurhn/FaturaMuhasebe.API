using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FaturaMuhasebe.API.Models
{
    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountRate { get; set; } = 0; // % İskonto

        public int VatRate { get; set; } = 20; // % KDV

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; } = 0; // Satır Toplamı (KDV Dahil)
    }
}
