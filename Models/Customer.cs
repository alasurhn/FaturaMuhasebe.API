using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaMuhasebe.API.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty; // Unvan / Ad Soyad

        [Required]
        [StringLength(20)]
        public string CustomerType { get; set; } = "Customer"; // Customer (Müşteri) veya Supplier (Tedarikçi)

        [StringLength(20)]
        public string TaxNumber { get; set; } = string.Empty; // VKN / TCKN

        [StringLength(100)]
        public string TaxOffice { get; set; } = string.Empty; // Vergi Dairesi

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0; // + Alacaklı (Biz borçluyuz), - Borçlu (Bize borcu var)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
