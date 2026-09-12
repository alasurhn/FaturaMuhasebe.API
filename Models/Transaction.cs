using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaMuhasebe.API.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = "Collection"; // Collection (Tahsilat) veya Payment (Ödeme)

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        [StringLength(20)]
        public string PaymentMethod { get; set; } = "Kasa"; // Kasa, Banka Transferi, Kredi Kartı

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
