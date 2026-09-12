using FaturaMuhasebe.API.Data;
using FaturaMuhasebe.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Serve Static Files for Frontend SPA
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Auto Database Migration & Seed Initial Demo Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Customers.Any())
    {
        var cust1 = new Customer { Title = "TeknoSoft A.Ş.", TaxNumber = "1234567890", TaxOffice = "Kadıköy V.D.", Email = "info@teknosoft.com", Phone = "02165550011", Address = "İstanbul", CustomerType = "Customer", Balance = -15400.00m };
        var cust2 = new Customer { Title = "Lojistik Ticaret Ltd.", TaxNumber = "9876543210", TaxOffice = "Beşiktaş V.D.", Email = "muhasebe@lojistik.com", Phone = "02124440022", Address = "İstanbul", CustomerType = "Customer", Balance = 0.00m };
        var supplier = new Customer { Title = "Donanım Market Tedarik A.Ş.", TaxNumber = "5556667770", TaxOffice = "Mecidiyeköy V.D.", Email = "satis@donanim.com", Phone = "02123330044", Address = "İstanbul", CustomerType = "Supplier", Balance = 8500.00m };

        db.Customers.AddRange(cust1, cust2, supplier);
        db.SaveChanges();

        var prod1 = new Product { Code = "YAZ-001", Name = "Özel Web Yazılım Hizmeti", Unit = "Saat", UnitPrice = 1200.00m, VatRate = 20, StockQuantity = 500 };
        var prod2 = new Product { Code = "SUN-002", Name = "Bulut Sunucu Kiralama (Aylık)", Unit = "Adet", UnitPrice = 2500.00m, VatRate = 20, StockQuantity = 100 };
        var prod3 = new Product { Code = "DAN-003", Name = "Siber Güvenlik Danışmanlığı", Unit = "Saat", UnitPrice = 1800.00m, VatRate = 20, StockQuantity = 200 };

        db.Products.AddRange(prod1, prod2, prod3);
        db.SaveChanges();

        // Seed Sample Invoice
        var sampleInvoice = new Invoice
        {
            InvoiceNumber = "FAT202600001",
            CustomerId = cust1.Id,
            InvoiceType = "Sales",
            IssueDate = DateTime.UtcNow.AddDays(-2),
            DueDate = DateTime.UtcNow.AddDays(12),
            Status = "Approved",
            SubTotal = 14000.00m,
            TotalDiscount = 1000.00m,
            TotalVat = 2600.00m,
            GrandTotal = 15600.00m,
            Notes = "Web yazılım ve sunucu kurulum bedeli.",
            Items = new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = prod1.Id, Description = "10 Saat Yazılım Geliştirme", Quantity = 10, UnitPrice = 1200.00m, DiscountRate = 0, VatRate = 20, LineTotal = 14400.00m },
                new InvoiceItem { ProductId = prod2.Id, Description = "1 Adet Sunucu Kiralama", Quantity = 1, UnitPrice = 2000.00m, DiscountRate = 50, VatRate = 20, LineTotal = 1200.00m }
            }
        };

        db.Invoices.Add(sampleInvoice);
        db.SaveChanges();
    }
}

app.Run();
