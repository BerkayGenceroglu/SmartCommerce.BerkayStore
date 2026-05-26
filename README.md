# SmartCommerce — Berkay Store

**Tam kapsamlı .NET 8 Mikroservis E-Ticaret Platformu**

> Gerçek dünya e-ticaret senaryolarını karşılamak üzere tasarlanmış, event-driven mikroservis mimarisi üzerine inşa edilmiş modern bir alışveriş platformu. PostgreSQL, Redis, RabbitMQ, Elasticsearch ve Docker ile desteklenmektedir.

> Bu proje portfolyo amaçlı geliştirilmiştir. Gerçek bir ticari işletme değildir.

---

## Proje Tanıtımı

SmartCommerce, modern e-ticaret ihtiyaçlarını karşılamak üzere **mikroservis mimarisi** ile geliştirilmiş kapsamlı bir platformdur. Proje; bağımsız çalışabilen API servisleri, event-driven worker servisleri, gerçek zamanlı arama motoru ve eksiksiz bir admin yönetim panelinden oluşmaktadır.

### Temel Özellikler

- **10 bağımsız proje** tek bir solution altında
- **Event-driven mimari** ile servisler arası gevşek bağlantı
- **Gerçek zamanlı ürün arama** Elasticsearch ile
- **Akıllı cache yönetimi** Redis ile
- **Asenkron iş akışları** RabbitMQ + MassTransit ile
- **JWT tabanlı güvenli kimlik doğrulama**
- **Tam kapsamlı admin paneli** LINQ sorguları ile

---

## Kullanılan Teknolojiler

### Backend & Framework

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **.NET** | 8.0 | Tüm API ve Worker servisleri |
| **ASP.NET Core MVC** | 8.0 | UI katmanı, Razor view'lar |
| **Entity Framework Core** | 8.0 | ORM, code-first migration |
| **MassTransit** | 8.x | RabbitMQ mesaj kuyruğu yönetimi |
| **JWT Bearer Authentication** | - | Stateless token tabanlı auth |
| **BCrypt.Net** | - | Güvenli şifre hashleme |
| **Serilog** | - | Yapılandırılmış, zengin loglama |
| **Newtonsoft.Json** | - | JSON serialization (UI katmanı) |
| **NEST / Elasticsearch.Net** | 8.x | Elasticsearch client |
| **StackExchange.Redis** | - | Redis cache client |
| **Npgsql.EntityFrameworkCore** | - | PostgreSQL EF Core provider |

### Veritabanı & Mesajlaşma

| Teknoloji | Port (Host) | Port (Container) | Kullanım Amacı |
|-----------|-------------|-------------------|----------------|
| **PostgreSQL** | 5433 | 5432 | Ana ilişkisel veritabanı |
| **Redis** | 6381 | 6379 | Ürün listesi cache (TTL: 5 dk) |
| **RabbitMQ** | 5674 (AMQP) | 5672 | Servisler arası mesajlaşma |
| **RabbitMQ Management** | 15673 | 15672 | Kuyruk yönetim arayüzü |
| **Elasticsearch** | 9204 | 9200 | Full-text ürün arama motoru |
| **Kibana** | 5604 | 5601 | Elasticsearch görsel yönetim |

### Frontend

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| **ASP.NET Core MVC + Razor** | Server-side rendering |
| **Plus Jakarta Sans** | Ana font (display + body) |
| **Font Awesome 6.5** | İkon seti |
| **Chart.js 4.4** | Admin dashboard grafikleri (doughnut) |
| **Vanilla JavaScript** | Client-side etkileşimler, fetch API |

### Altyapı & DevOps

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| **Docker** | Tüm altyapı servislerinin containerization |
| **Docker Compose** | Çoklu container orkestrasyon |
| **Gmail SMTP** | Sipariş bildirim e-postaları |

---

## Sistem Mimarisi

```
SmartCommerce Solution (10 Proje)
│
├── 📦 UserApi          → Port: 7038  | JWT Auth, kullanıcı yönetimi
├── 📦 ProductApi       → Port: 7136  | Ürünler, kategoriler, varyantlar
├── 📦 OrderApi         → Port: 7124  | Sepet, sipariş, kupon
├── 🖥️  SmartCommerce.UI → Port: 7050  | MVC frontend + Admin paneli
│
├── ⚙️  NotificationWorker  | E-posta bildirimleri
├── ⚙️  InvoiceWorker       | Fatura oluşturma ve kayıt
├── ⚙️  CargoWorker         | Kargo takip numarası oluşturma
├── ⚙️  StockWorker         | Stok güncelleme
├── ⚙️  PaymentWorker       | Ödeme kaydı işleme
│
└── 📚 Shared           | Ortak entity, enum, event modelleri
```

### Katmanlı Mimari

```
┌─────────────────────────────────────────────────┐
│              Kullanıcı / Tarayıcı               │
└──────────────────────┬──────────────────────────┘
                       │ HTTP
┌──────────────────────▼──────────────────────────┐
│           SmartCommerce.UI (Port 7050)           │
│    MVC · Session Auth · Admin Panel · Razor      │
└─────┬──────────────┬──────────────────┬──────────┘
      │ HTTP         │ HTTP             │ HTTP
┌─────▼──────┐  ┌────▼───────┐  ┌──────▼──────┐
│  UserApi   │  │ ProductApi │  │  OrderApi   │
│  :7038     │  │   :7136    │  │    :7124    │
└─────┬──────┘  └────┬───────┘  └──────┬──────┘
      │              │  ┌──────────────┘
      │              │  │
┌─────▼──────────────▼──▼─────────────────────────┐
│              PostgreSQL (Port: 5433)             │
└──────────────────────────────────────────────────┘
                      │ OrderCreated Event
┌─────────────────────▼────────────────────────────┐
│          RabbitMQ + MassTransit (Port: 5674)     │
└────┬──────────┬──────────┬──────────┬────────────┘
     │          │          │          │
┌────▼───┐ ┌───▼────┐ ┌───▼───┐ ┌────▼────┐
│Notif.  │ │Invoice │ │Cargo  │ │ Stock   │
│Worker  │ │Worker  │ │Worker │ │ Worker  │
└────────┘ └────────┘ └───────┘ └─────────┘
```

---

## Servis Detayları

### UserApi — Kimlik Doğrulama Servisi

Kullanıcı kayıt, giriş, JWT token üretimi ve profil yönetimini sağlar.

**Teknolojiler:** BCrypt.Net, JWT Bearer, Entity Framework Core, Npgsql

**Endpoint'ler:**

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| `POST` | `/api/auth/register` | Yeni kullanıcı kaydı | — |
| `POST` | `/api/auth/login` | JWT token ile giriş | — |
| `GET` | `/api/auth/profile` | Profil bilgilerini getir | Bearer |
| `PUT` | `/api/auth/profile` | Profil güncelle (isim, tel, şehir, cinsiyet) | Bearer |
| `PUT` | `/api/auth/password` | Şifre değiştir (BCrypt doğrulama) | Bearer |
| `GET` | `/api/user/all` | Tüm kullanıcılar (Admin panel için) | — |

**JWT Payload:**
```json
{
  "nameid": "user-uuid",
  "email": "user@example.com",
  "name": "Ad Soyad",
  "role": "Customer | Admin",
  "exp": 1234567890
}
```

---

### ProductApi — Ürün Servisi

Ürün, kategori ve varyant yönetimi ile Elasticsearch tabanlı arama işlemlerini yürütür.

**Teknolojiler:** NEST (Elasticsearch), StackExchange.Redis, EF Core, Npgsql

**Endpoint'ler:**

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| `GET` | `/api/product` | Tüm ürünler (Redis cache'li) | — |
| `GET` | `/api/product/{id}` | Ürün detayı | — |
| `DELETE` | `/api/product/{id}` | Ürün sil | Admin |
| `GET` | `/api/category` | Tüm kategoriler | — |
| `GET` | `/api/product/search?q=` | Elasticsearch full-text arama | — |
| `POST` | `/api/product/reindex` | Elasticsearch re-index | — |

**Redis Cache Stratejisi:**
```
İstek → Redis cache var mı?
          ├── Evet → Cache'den döner (~1ms)
          └── Hayır → PostgreSQL'den çeker
                        → Redis'e yazar (TTL: 5dk)
                        → Kullanıcıya döner
```

**Elasticsearch İndex Yapısı:**
- Ürün adı (full-text, Türkçe analyzer)
- Marka (keyword)
- Açıklama (full-text)
- Kategori adı (keyword)

**Varyant Sistemi:**
```
ProductVariant
├── VariantType: Beden | Renk | Numara | Boyut | Kapasite
├── VariantValue: "XL" | "Kırmızı" | "42" | "15 inç" | "256GB"
└── PriceModifier: +50.00 (fiyat farkı)
```

**Kategoriler:**

| Kategori | ID |
|----------|----|
| Elektronik | `7d396862-4e7a-4272-a60b-6b1d8230836b` |
| Giyim | `c8637c6e-e7a7-4081-b196-2998f8c81626` |
| Spor | `39a916d7-1302-4049-ab98-1fd56e6284d2` |
| Bakım & Kozmetik | `cc486fb6-cece-4167-addd-925638bbeb74` |
| Ev & Yaşam | `520a824a-0ce9-4732-bf8d-bd197156cdb7` |
| Kitap | `223b94c8-e3b8-4a0f-97ae-4e83c8ed3d5e` |
| Evcil Hayvan | `1b496feb-52eb-4b99-a3ba-da4e5f68c3f3` |

---

### OrderApi — Sipariş Servisi

Sepet yönetimi, sipariş oluşturma, kupon sistemi ve event yayınlama işlemlerini yürütür.

**Teknolojiler:** MassTransit, RabbitMQ, EF Core, Npgsql

**Endpoint'ler:**

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| `GET` | `/api/cart` | Sepeti getir | Bearer |
| `POST` | `/api/cart` | Sepete ürün ekle (varyant bilgisi ile) | Bearer |
| `DELETE` | `/api/cart/{productId}` | Sepetten çıkar | Bearer |
| `POST` | `/api/order` | Sipariş oluştur + event yayınla | Bearer |
| `GET` | `/api/order` | Kullanıcının siparişleri | Bearer |
| `GET` | `/api/order/all` | Tüm siparişler (Admin panel) | — |
| `GET` | `/api/coupons` | Tüm kuponlar | — |
| `POST` | `/api/coupons/apply` | Kupon uygula + indirim hesapla | Bearer |

**Kupon Sistemi:**

| DiscountType | Açıklama |
|--------------|----------|
| `1` | Yüzde indirimi (% X) |
| `2` | Sabit tutar indirimi (X ₺) |

---

### Worker Servisler — Event-Driven İş Akışları

Tüm worker servisler `IConsumer<OrderCreated>` interface'ini implement eder ve RabbitMQ'dan mesaj dinler.

#### NotificationWorker
- **Dinlediği Event:** `OrderCreated`
- **İşlem:** Gmail SMTP üzerinden HTML sipariş onay e-postası
- **Yapılandırma:** `Mail:From`, `Mail:Password`, SMTP port 587
- **Loglama:** Serilog ile başarı/hata logları

#### InvoiceWorker
- **Dinlediği Event:** `OrderCreated`
- **İşlem:** Benzersiz fatura numarası üretir (`INV-{Unix timestamp}`)
- **Kayıt:** PostgreSQL `Invoices` tablosuna kaydeder

#### CargoWorker
- **Dinlediği Event:** `OrderCreated`
- **İşlem:** Kargo takip numarası üretir (`CARGO-{YYYYMMDD}-{UUID kısmı}`)
- **Kayıt:** PostgreSQL `Cargos` tablosuna kaydeder
- **Durumlar:** `Preparing (0)` → `Shipped (1)` → `Delivered (2)`

#### StockWorker
- **Dinlediği Event:** `OrderCreated`
- **İşlem:** Her sipariş kalemi için `Stock -= Quantity` günceller

#### PaymentWorker
- **Dinlediği Event:** `OrderCreated`
- **İşlem:** Ödeme kaydını işler ve loglar

---

### SmartCommerce.UI — Frontend & Admin

ASP.NET Core MVC ile geliştirilmiş kullanıcı arayüzü ve tam kapsamlı admin paneli.

**Mimari Kararlar:**

- **HttpClient** ile API'lere HTTP istek (servis katmanı pattern)
- **Session** tabanlı auth (`Session["Token"]`, `Session["AdminToken"]`)
- **`OnActionExecuting` override** ile admin sayfaları korunur
- **Newtonsoft.Json** ile ViewBag serialize (PascalCase korunur)
- **LINQ** ile API verisi memory'de işlenir (Controller katmanında)

**Admin Panel Güvenlik Akışı:**
```
1. /Admin/Login → UserApi'ye POST /api/auth/login
2. JWT payload decode → Role = "Admin" kontrolü
3. sessionStorage'a kaydet (client)
4. POST /Admin/SaveSession → Session["AdminToken"] kaydı (server)
5. OnActionExecuting → her request'te Session kontrolü
6. Token yoksa → /Admin/Login'e redirect
```

---

## Akış Süreçleri

### Kullanıcı Kayıt ve Giriş Akışı

```
Kullanıcı → /Auth/Register (Ad, Email, Şifre, Tel, Cinsiyet, Şehir)
               │
               ▼
         AuthController.Register()
               │
               ▼
         UserApi POST /api/auth/register
               │
               ▼
         BCrypt.HashPassword() → PostgreSQL Users tablosu
               │
               ▼
         Başarılı → /Auth/Login'e yönlendir

Kullanıcı → /Auth/Login (Email, Şifre)
               │
               ▼
         UserApi POST /api/auth/login
               │
               ▼
         BCrypt.Verify() → JWT Token üret
               │
               ▼
         Session["Token"] + Session["UserName"] + Session["UserId"]
               │
               ▼
         /Product'a yönlendir
```

### Ürün Arama Akışı (Elasticsearch)

```
Kullanıcı arama yapar (navbar input)
          │
          ▼
Elasticsearch /api/product/search?q=query
          │
          ▼
Full-text search (ürün adı, marka, açıklama)
          │
          ├── Sonuç var → Ürün listesi döner (JSON)
          └── Sonuç yok → Boş liste, "bulunamadı" mesajı

Re-index (ürün eklenince):
POST /api/product/reindex
          │
          ▼
Tüm ürünler PostgreSQL'den çekilir
          │
          ▼
Elasticsearch'e bulk index
```

### Sepet ve Sipariş Akışı

```
Ürün Detay → Varyant Seç (Beden/Renk/Numara)
               │
               ▼
         POST /api/cart (productId, variantInfo, quantity)
               │
               ▼
         Kupon Kodu Uygula (isteğe bağlı)
         POST /api/coupons/apply → TotalAmount hesapla
               │
               ▼
         POST /api/order (address, items, couponCode)
               │
               ▼
         OrderApi → PostgreSQL'e kaydet
               │
               ▼
         MassTransit → RabbitMQ'ya OrderCreated yayınla
               │
               ├── NotificationWorker → E-posta gönder
               ├── InvoiceWorker      → Fatura kaydet
               ├── CargoWorker        → Kargo kaydet
               ├── StockWorker        → Stok düş
               └── PaymentWorker      → Ödeme işle
```

### Redis Cache Akışı

```
GET /api/product isteği
        │
        ▼
Redis cache kontrol (key: "products")
        │
   Var mı?
   ├── EVET → Direkt cache'den döner (~1ms)
   └── HAYIR → PostgreSQL'den çek (JOIN + Include)
                    │
                    ▼
               Redis'e yaz (TTL: 5 dakika)
                    │
                    ▼
               Response döner
```

---

## Özellikler

### Kullanıcı Arayüzü

| Sayfa | Özellikler |
|-------|------------|
| **Ana Sayfa** | Hero banner, canlı Elasticsearch arama, kategori grid, kampanya bannerları |
| **Ürün Listesi** | Kategori/marka/stok filtresi, URL'den kategori parametresi |
| **Ürün Detay** | Varyant seçici (beden/renk/numara), fiyat modifier, stok uyarısı |
| **Sepet** | Kupon uygulama, varyant bilgisi gösterimi, sipariş özeti |
| **Siparişlerim** | OrderByDescending (son sipariş üstte), durum badge'i |
| **Favorilerim** | Grid layout, favoriye ekle/çıkar |
| **Profil** | Tab yapısı: bilgi güncelleme + şifre değiştirme, BCrypt doğrulama |
| **Kayıt** | Ad, email, şifre + opsiyonel: telefon, cinsiyet, şehir |
| **Yorum Sistemi** | 1-5 yıldız puanlama, yorum metni, kullanıcı adı gösterimi |

### Admin Paneli

| Sayfa | Özellikler |
|-------|------------|
| **Dashboard** | Sipariş/gelir istatistikleri (bugün/toplam), Chart.js doughnut, en çok sipariş edilen ürünler, kritik stok listesi, son siparişler tablosu |
| **Ürünler** | Listeleme, kategori/marka/stok filtresi, sayfalama, silme |
| **Siparişler** | Listeleme, durum filtresi, detay modal (kalemler, adres, toplam) |
| **Kargo** | Durum filtresi (Preparing/Shipped/Delivered), takip numarası arama |
| **Kuponlar** | Listeleme, yeni kupon oluşturma (tip, değer, limit, son kullanma), silme |
| **Kullanıcılar** | Listeleme, rol filtresi (Admin/Kullanıcı), renkli avatar |
| **Faturalar** | Listeleme, fatura detay modal, yazdırma desteği |

---

## Ekran Görüntüleri

### Kullanıcı Arayüzü

#### Ana Sayfa
![Ana Sayfa](screenshots/home.png)

#### Ürün Listesi
![Ürün Listesi](screenshots/products.png)

#### Ürün Detay
![Ürün Detay](screenshots/product-detail.png)

#### Sepet
![Sepet](screenshots/cart.png)

#### Siparişlerim
![Siparişlerim](screenshots/orders.png)

#### Favorilerim
![Favorilerim](screenshots/favorites.png)

#### Profil
![Profil](screenshots/profile.png)

#### Giriş
![Giriş](screenshots/login.png)

#### Kayıt
![Kayıt](screenshots/register.png)

---

### Admin Paneli

#### Admin Giriş
![Admin Giriş](screenshots/admin-login.png)

#### Dashboard
![Dashboard](screenshots/admin-dashboard.png)

#### Ürün Yönetimi
![Ürün Yönetimi](screenshots/admin-products.png)

#### Sipariş Yönetimi
![Sipariş Yönetimi](screenshots/admin-orders.png)

#### Kargo Takibi
![Kargo Takibi](screenshots/admin-cargo.png)

#### Kupon Yönetimi
![Kupon Yönetimi](screenshots/admin-coupons.png)

#### Kullanıcı Yönetimi
![Kullanıcı Yönetimi](screenshots/admin-users.png)

#### Fatura Yönetimi
![Fatura Yönetimi](screenshots/admin-invoices.png)

---

## Kurulum

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Visual Studio 2022 veya JetBrains Rider

### 1. Repoyu Klonla

```bash
git clone https://github.com/BerkayGenceroğlu/smartcommerce.git
cd smartcommerce
```

### 2. Docker Altyapısını Başlat

```bash
docker-compose up -d
```

Bu komut şu servisleri başlatır:

| Servis | Host Port | Amaç |
|--------|-----------|------|
| PostgreSQL | 5433 | Ana veritabanı |
| Redis | 6381 | Cache |
| RabbitMQ AMQP | 5674 | Mesaj kuyruğu |
| RabbitMQ UI | 15673 | Yönetim arayüzü |
| Elasticsearch | 9204 | Arama motoru |
| Kibana | 5604 | Elastic yönetim UI |

### 3. Veritabanı Migration

```bash
# Her API için ayrı ayrı çalıştır
cd UserApi && dotnet ef database update
cd ../ProductApi && dotnet ef database update
cd ../OrderApi && dotnet ef database update
```

### 4. Seed Verisi

DBeaver, pgAdmin veya psql ile:

```sql
-- Kategoriler ve 200+ ürün ekle
\i seed_products.sql
```

### 5. Elasticsearch Re-Index

Ürünler yüklendikten sonra:

```bash
POST https://localhost:7136/api/product/reindex
```

### 6. Admin Kullanıcısı Oluştur

1. Normal kayıt ekranından kayıt ol
2. DBeaver'dan `Role` değerini `1` yap:

```sql
UPDATE "Users" SET "Role" = 1 WHERE "Email" = 'admin@berkaystore.com';
```

### 7. Projeleri Başlat

Visual Studio'da `Multiple Startup Projects` ayarla:

```
UserApi          ✓
ProductApi       ✓
OrderApi         ✓
SmartCommerce.UI ✓
NotificationWorker ✓
InvoiceWorker    ✓
CargoWorker      ✓
StockWorker      ✓
PaymentWorker    ✓
```

### 8. Erişim URL'leri

| Servis | URL |
|--------|-----|
| Uygulama | https://localhost:7050 |
| Admin Paneli | https://localhost:7050/Admin/Login |
| UserApi Swagger | https://localhost:7038/swagger |
| ProductApi Swagger | https://localhost:7136/swagger |
| OrderApi Swagger | https://localhost:7124/swagger |
| RabbitMQ Management | http://localhost:15673 (guest/guest) |
| Kibana | http://localhost:5604 |

---

## API Referansı

### Kimlik Doğrulama

Tüm korunan endpoint'ler için `Authorization` header'ı gereklidir:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Örnek İstekler

**Kayıt:**
```http
POST https://localhost:7038/api/auth/register
Content-Type: application/json

{
  "fullName": "Berkay Genceroğlu",
  "email": "berkay@example.com",
  "password": "Sifre123!",
  "phoneNumber": "+905001234567",
  "gender": "Erkek",
  "city": "İstanbul"
}
```

**Ürün Arama:**
```http
GET https://localhost:7136/api/product/search?q=laptop
```

**Sipariş Oluşturma:**
```http
POST https://localhost:7124/api/order
Authorization: Bearer {token}
Content-Type: application/json

{
  "address": "Kadıköy, İstanbul",
  "couponCode": "BERKAY20"
}
```

---

## Veritabanı Şeması

### Users (UserApi)
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `FullName` | text | Ad soyad |
| `Email` | text | E-posta (unique) |
| `PasswordHash` | text | BCrypt hash |
| `Role` | int4 | 0=Customer, 1=Admin |
| `PhoneNumber` | text | Telefon (nullable) |
| `Gender` | text | Cinsiyet (nullable) |
| `City` | text | Şehir (nullable) |
| `Country` | text | Ülke (nullable) |
| `CreatedAt` | timestamptz | Kayıt tarihi |

### Products (ProductApi)
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `Name` | text | Ürün adı |
| `Brand` | text | Marka |
| `Description` | text | Açıklama |
| `Price` | numeric | Fiyat |
| `Stock` | int4 | Stok adedi |
| `CategoryId` | uuid | FK → Categories |
| `IsActive` | bool | Satışta mı |
| `ImageUrl` | text | Görsel URL |
| `CreatedAt` | timestamptz | Oluşturma tarihi |

### Orders (OrderApi)
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `UserId` | uuid | Kullanıcı |
| `TotalAmount` | numeric | Toplam tutar |
| `Address` | text | Teslimat adresi |
| `Status` | int4 | 0=Bekliyor, 1=Tamamlandı, 2=İptal |
| `CouponCode` | text | Uygulanan kupon (nullable) |
| `CreatedAt` | timestamptz | Sipariş tarihi |

### Cargos
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `OrderId` | uuid | Sipariş |
| `UserId` | uuid | Kullanıcı |
| `TrackingNumber` | text | Takip numarası |
| `Status` | int4 | 0=Preparing, 1=Shipped, 2=Delivered |
| `CreatedAt` | timestamptz | Oluşturma tarihi |
| `DeliveredAt` | timestamptz | Teslim tarihi (nullable) |

### Invoices
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `OrderId` | uuid | Sipariş |
| `UserId` | uuid | Kullanıcı |
| `InvoiceNumber` | text | Fatura numarası (INV-xxx) |
| `TotalAmount` | numeric | Fatura tutarı |
| `CreatedAt` | timestamptz | Fatura tarihi |

### Coupons (OrderApi)
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `Id` | uuid | Primary key |
| `Code` | text | Kupon kodu |
| `DiscountType` | int4 | 1=Yüzde, 2=Sabit tutar |
| `DiscountValue` | numeric | İndirim değeri |
| `MinimumAmount` | numeric | Min. sipariş tutarı |
| `UsageLimit` | int4 | Kullanım limiti |
| `UsageCount` | int4 | Kullanım sayısı |
| `IsActive` | bool | Aktif mi |
| `ExpiresAt` | timestamptz | Son kullanma tarihi |
| `CreatedAt` | timestamptz | Oluşturma tarihi |

---

## Proje Yapısı

```
SmartCommerce/
├── UserApi/
│   ├── Controllers/          AuthController
│   ├── Services/             AuthService, IAuthService
│   ├── Entities/             ApiUser
│   ├── Dtos/                 LoginDto, RegisterDto, ProfileDto, ChangePasswordDto, UpdateProfileDto
│   └── Context/              UserDbContext
│
├── ProductApi/
│   ├── Controllers/          ProductController, CategoryController
│   ├── Services/             ProductService, ElasticsearchService, RedisService
│   ├── Entities/             Product, Category, ProductVariant
│   └── Context/              ProductDbContext
│
├── OrderApi/
│   ├── Controllers/          CartController, OrderController, CouponController
│   ├── Controllers/Admin/    OrderDashboardController, UserDashboardController
│   ├── Services/             CartService, OrderService, CouponService
│   ├── Entities/             Order, OrderItem, CartItem, AppUser, Coupon
│   └── Context/              OrderContext
│
├── SmartCommerce.UI/
│   ├── Controllers/          HomeController, ProductController, CartController,
│   │                         OrderController, FavoriteController, ProfileController,
│   │                         AuthController, ReviewController
│   ├── Areas/Admin/
│   │   ├── Controllers/      AdminController
│   │   ├── Services/         AdminProductService, AdminOrderService, UserService,
│   │   │                     CargoService, CouponService, InvoiceService
│   │   ├── Abstract/         IAdminProductService, IAdminOrderService, IUserService,
│   │   │                     ICargoService, ICouponService, IInvoiceService
│   │   ├── Dtos/             ProductDto, OrderDto, UserDto, CargoDto, CouponDto, InvoiceDto
│   │   ├── Context/          AdminDbContext, CargoEntity, InvoiceEntity
│   │   └── Views/Admin/      Login, Dashboard, Products, Orders, Cargo, Coupons, Users, Invoices
│   ├── Views/                Home/Index, Product/Index+Detail, Cart/Index,
│   │                         Order/Index, Favorite/Index, Profile/Index,
│   │                         Auth/Login+Register
│   └── Services/             (HttpClient tabanlı API servisleri)
│
├── NotificationWorker/       OrderCreatedConsumer, EmailService
├── InvoiceWorker/            OrderCreatedConsumer, InvoiceDbContext
├── CargoWorker/              OrderCreatedConsumer, CargoDbContext
├── StockWorker/              OrderCreatedConsumer
├── PaymentWorker/            OrderCreatedConsumer
│
├── Shared/
│   ├── Entities/             Cargo, OrderCreated, OrderCreatedItem
│   └── Enums/                CargoStatus, UserRole, DiscountType
│
├── docker-compose.yml
└── seed_products.sql
```

---

## Geliştirici

**Berkay Genceroğlu**

Bu proje portfolyo amaçlı geliştirilmiştir. Gerçek bir ticari işletme değildir.
