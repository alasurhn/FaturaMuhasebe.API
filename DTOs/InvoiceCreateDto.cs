namespace FaturaMuhasebe.API.DTOs
{
    public class InvoiceCreateDto
    {
        public int CustomerId { get; set; }
        public string InvoiceType { get; set; } = "Sales"; // Sales veya Purchase
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);
        public string Notes { get; set; } = string.Empty;
        public List<InvoiceItemCreateDto> Items { get; set; } = new List<InvoiceItemCreateDto>();
    }

    public class InvoiceItemCreateDto
    {
        public int ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; } = 0;
        public decimal DiscountRate { get; set; } = 0;
        public int VatRate { get; set; } = 20;
    }
}
