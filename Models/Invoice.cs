using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaMuhasebe.API.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty; // FAT2026000001

        [Required]
        [StringLength(20)]
        public string InvoiceType { get; set; } = "Sales"; // Sales (Satış) veya Purchase (Alış)

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow; // Fatura Tarihi
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14); // Vade Tarihi

        [StringLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Approved, Paid, Cancelled

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; } = 0; // Ara Toplam (Matrah)

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscount { get; set; } = 0; // Toplam İskonto

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVat { get; set; } = 0; // Toplam KDV

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrandTotal { get; set; } = 0; // Genel Toplam

        public string Notes { get; set; } = string.Empty;

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
