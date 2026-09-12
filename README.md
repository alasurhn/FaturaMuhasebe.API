# 🧾 FaturaMuhasebe.API - Ön Muhasebe & Fatura Yönetim Portalı

Bu proje, KOBİ'ler ve serbest çalışanlar için geliştirilmiş **Cari (Müşteri/Tedarikçi) Takibi, Stok Yönetimi, Fatura Düzenleme, Otomatik KDV/İskonto Hesaplama ve PDF Fatura Üretimi** özelliklerine sahip Full-Stack ön muhasebe uygulamasıdır.

.NET 8/10 Web API backend ve modern Tailwind CSS destekli Single Page Application (SPA) arayüzü ile hazırlanmıştır.

---

## 🚀 Öne Çıkan Özellikler

- 📊 **Finansal Özet Dashboard**: Toplam satış cirosu, net KDV özeti, aktif müşteri ve stok istatistikleri.
- 👥 **Cari (Müşteri & Tedarikçi) Takibi**: Borç/Alacak bakiyelerinin otomatik güncellenmesi.
- 📦 **Stok ve Hizmet Kataloğu**: Ürün fiyatlandırması, stok miktar takibi ve KDV oranları (%1, %10, %20).
- 📝 **Canlı Fatura Oluşturucu**: 
  - Dinamik satır ekleme/çıkarma.
  - Matrah, İskonto ve KDV tutarlarının real-time hesaplanması.
  - Fatura onaylandığında stoktan otomatik düşüş ve cari bakiyeye borç kaydı (Database Transaction).
- 🖨️ **PDF & Yazdırılabilir Fatura Dökümü**: Resmi fatura şablonuna uygun yazdırma ve PDF indirme.
- 💰 **Tahsilat ve Ödeme Yönetimi**: Faturalara bağlı ödeme kaydı alma ve bakiye kapatma.

---

## 🛠️ Kullanılan Teknolojiler

- **Backend**: C# .NET 8 / 10 Web API
- **ORM & Veritabanı**: Entity Framework Core + SQLite (`app.db`)
- **API Dokümantasyonu**: Swagger / Swashbuckle
- **Frontend**: HTML5, Vanilla JavaScript (ES6+), Tailwind CSS
- **Kütüphaneler**: Chart.js, SweetAlert2, FontAwesome Icons

---

## 📂 Proje Yapısı

```
FaturaMuhasebe.API/
├── Controllers/
│   ├── CustomersController.cs    # Cari CRUD İşlemleri
│   ├── ProductsController.cs     # Stok / Hizmet CRUD İşlemleri
│   ├── InvoicesController.cs     # Fatura Oluşturma & KDV Mantığı & Tahsilat
│   └── DashboardController.cs    # Finansal Özet Raporlama
├── Data/
│   └── AppDbContext.cs           # EF Core Veritabanı Bağlamı
├── Models/
│   ├── Customer.cs               # Müşteri / Tedarikçi Modeli
│   ├── Product.cs                # Ürün / Hizmet Modeli
│   ├── Invoice.cs                # Fatura Başlığı Modeli
│   ├── InvoiceItem.cs            # Fatura Kalemi Modeli
│   └── Transaction.cs            # Kasa / Banka Hareket Modeli
├── DTOs/
│   └── InvoiceCreateDto.cs       # Fatura Veri Transfer Nesneleri
├── wwwroot/
│   └── index.html                # Modern Frontend SPA Arayüzü
└── Program.cs                    # Uygulama Başı & Seed Verileri
```

---

## 💻 Çalıştırma Talimatı

Proje klasöründe terminal açıp aşağıdaki komutu çalıştırmanız yeterlidir:

```bash
dotnet run
```

Uygulama başladığında:
- **Arayüz (SPA)**: `http://localhost:5000` (veya gösterilen HTTPS portu)
- **Swagger API Dokümantasyonu**: `http://localhost:5000/swagger`

*(Veritabanı `app.db` dosyası olarak otomatik oluşturulur ve test verileriyle doldurulur).*
