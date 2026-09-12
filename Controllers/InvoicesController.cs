using FaturaMuhasebe.API.Data;
using FaturaMuhasebe.API.DTOs;
using FaturaMuhasebe.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturaMuhasebe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();
            return invoice;
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoice(InvoiceCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var customer = await _context.Customers.FindAsync(dto.CustomerId);
                if (customer == null) return BadRequest("Müşteri bulunamadı.");

                // Otomatik Fatura Numarası Oluşturma (FAT20260001)
                var count = await _context.Invoices.CountAsync() + 1;
                var invoiceNumber = $"FAT{DateTime.UtcNow.Year}{count:D5}";

                var invoice = new Invoice
                {
                    InvoiceNumber = invoiceNumber,
                    CustomerId = dto.CustomerId,
                    InvoiceType = dto.InvoiceType,
                    IssueDate = dto.IssueDate,
                    DueDate = dto.DueDate,
                    Notes = dto.Notes,
                    Status = "Approved", // Onaylandı durumunda kaydedilir
                    CreatedAt = DateTime.UtcNow
                };

                decimal subTotal = 0;
                decimal totalDiscount = 0;
                decimal totalVat = 0;
                decimal grandTotal = 0;

                foreach (var itemDto in dto.Items)
                {
                    var product = await _context.Products.FindAsync(itemDto.ProductId);
                    if (product == null) return BadRequest($"Ürün id={itemDto.ProductId} bulunamadı.");

                    decimal lineMatrah = (itemDto.Quantity * itemDto.UnitPrice);
                    decimal lineDiscount = lineMatrah * (itemDto.DiscountRate / 100m);
                    decimal netMatrah = lineMatrah - lineDiscount;
                    decimal lineVat = netMatrah * (itemDto.VatRate / 100m);
                    decimal lineTotal = netMatrah + lineVat;

                    subTotal += lineMatrah;
                    totalDiscount += lineDiscount;
                    totalVat += lineVat;
                    grandTotal += lineTotal;

                    // Stok Miktarını Güncelleme
                    if (dto.InvoiceType == "Sales")
                    {
                        product.StockQuantity -= (int)itemDto.Quantity;
                    }
                    else if (dto.InvoiceType == "Purchase")
                    {
                        product.StockQuantity += (int)itemDto.Quantity;
                    }

                    invoice.Items.Add(new InvoiceItem
                    {
                        ProductId = itemDto.ProductId,
                        Description = string.IsNullOrWhiteSpace(itemDto.Description) ? product.Name : itemDto.Description,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        DiscountRate = itemDto.DiscountRate,
                        VatRate = itemDto.VatRate,
                        LineTotal = lineTotal
                    });
                }

                invoice.SubTotal = subTotal;
                invoice.TotalDiscount = totalDiscount;
                invoice.TotalVat = totalVat;
                invoice.GrandTotal = grandTotal;

                // Cari Bakiye Güncellemesi (Satış ise müşteri borçlanır (-), Alış ise biz borçlanırız (+))
                if (dto.InvoiceType == "Sales")
                {
                    customer.Balance -= grandTotal;
                }
                else
                {
                    customer.Balance += grandTotal;
                }

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Fatura kaydederken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] decimal amount)
        {
            var invoice = await _context.Invoices.Include(i => i.Customer).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return NotFound();

            if (invoice.Status == "Paid") return BadRequest("Bu fatura zaten ödenmiş.");

            invoice.Status = "Paid";

            // Tahsilat Kaydı Ekleme
            var trans = new Transaction
            {
                InvoiceId = invoice.Id,
                CustomerId = invoice.CustomerId,
                Type = invoice.InvoiceType == "Sales" ? "Collection" : "Payment",
                Amount = amount > 0 ? amount : invoice.GrandTotal,
                Description = $"{invoice.InvoiceNumber} nolu faturanın tahsilat/ödemesi",
                Date = DateTime.UtcNow
            };

            // Cari Bakiyeyi Güncelleme
            if (invoice.Customer != null)
            {
                if (invoice.InvoiceType == "Sales")
                {
                    invoice.Customer.Balance += trans.Amount; // Borç kapatıldı
                }
                else
                {
                    invoice.Customer.Balance -= trans.Amount;
                }
            }

            _context.Transactions.Add(trans);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ödeme/Tahsilat başarıyla kaydedildi.", invoice });
        }
    }
}
