using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaMuhasebe.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // Stok Kodu (STK-001)

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Ürün/Hizmet Adı

        [StringLength(20)]
        public string Unit { get; set; } = "Adet"; // Adet, Paket, Saat, Kg

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0; // Birim Satış Fiyatı (KDV Hariç)

        public int VatRate { get; set; } = 20; // KDV Oranı (%1, %10, %20)

        public int StockQuantity { get; set; } = 0; // Stok Miktarı

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
