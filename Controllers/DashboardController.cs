using FaturaMuhasebe.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturaMuhasebe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalSales = await _context.Invoices
                .Where(i => i.InvoiceType == "Sales" && i.Status != "Cancelled")
                .SumAsync(i => (decimal?)i.GrandTotal) ?? 0;

            var totalPurchases = await _context.Invoices
                .Where(i => i.InvoiceType == "Purchase" && i.Status != "Cancelled")
                .SumAsync(i => (decimal?)i.GrandTotal) ?? 0;

            var totalVatCollected = await _context.Invoices
                .Where(i => i.InvoiceType == "Sales" && i.Status != "Cancelled")
                .SumAsync(i => (decimal?)i.TotalVat) ?? 0;

            var totalVatPaid = await _context.Invoices
                .Where(i => i.InvoiceType == "Purchase" && i.Status != "Cancelled")
                .SumAsync(i => (decimal?)i.TotalVat) ?? 0;

            var customerCount = await _context.Customers.CountAsync();
            var productCount = await _context.Products.CountAsync();
            var pendingInvoicesCount = await _context.Invoices.Where(i => i.Status == "Approved").CountAsync();

            var recentInvoices = await _context.Invoices
                .Include(i => i.Customer)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .Select(i => new {
                    i.Id,
                    i.InvoiceNumber,
                    CustomerName = i.Customer != null ? i.Customer.Title : "Bilinmiyor",
                    i.GrandTotal,
                    i.Status,
                    i.IssueDate
                })
                .ToListAsync();

            return Ok(new
            {
                totalSales,
                totalPurchases,
                netVat = totalVatCollected - totalVatPaid,
                customerCount,
                productCount,
                pendingInvoicesCount,
                recentInvoices
            });
        }
    }
}
