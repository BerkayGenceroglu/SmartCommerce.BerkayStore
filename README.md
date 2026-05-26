<h1 align="center">🛒 SmartCommerce.BerkayStore</h1>

<p align="center">
  <b>Modern Mikroservis E-Ticaret Platformu</b><br/>
  ASP.NET Core 8.0 · PostgreSQL · Redis · RabbitMQ · MassTransit · Elasticsearch · Serilog · JWT · Docker
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white"/>
  <img src="https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white"/>
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white"/>
  <img src="https://img.shields.io/badge/Elasticsearch-005571?style=for-the-badge&logo=elasticsearch&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white"/>
  <img src="https://img.shields.io/badge/Serilog-333333?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white"/>
</p>

---

## 📌 Proje Tanıtımı

**SmartCommerce.BerkayStore**, gerçek dünya üretim senaryolarını yansıtmak amacıyla tasarlanmış, tam kapsamlı bir **mikroservis e-ticaret altyapısıdır**. Proje; bağımsız API servisleri, event-driven arka plan işçileri, merkezi loglama, önbellekleme ve mesaj kuyruğu gibi modern yazılım mühendisliği pratiklerini bir arada uygular.

### Kullanıcılar için:
- Ürün listeleme, arama ve detay görüntüleme
- Sepete ürün ekleme, kupon kodu uygulama
- Sipariş oluşturma ve geçmiş takibi
- Profil yönetimi ve şifre değiştirme
- Ürünlere yorum ve değerlendirme bırakma
- Favori ürün listesi oluşturma

### Geliştiriciler için:
- Mikroservis mimarisinin katmanlı, gerçekçi uygulaması
- Event-Driven Architecture ile asenkron iş akışları
- Serilog + Elasticsearch + Kibana ile merkezi log yönetimi
- Redis ile cache ve JWT token yönetimi
- RabbitMQ + MassTransit ile Publish/Subscribe pattern
- PostgreSQL ile çok tablolu ilişkisel veri modeli

---

## 🚀 Kullanılan Teknolojiler

| Katman | Teknolojiler |
|--------|-------------|
| **Backend API** | `ASP.NET Core 8.0 Web API`, `C#`, `Entity Framework Core 8`, `Dapper` |
| **Frontend UI** | `ASP.NET Core MVC`, `Razor Views`, `Bootstrap 5`, `JavaScript`, `SCSS` |
| **Kimlik Doğrulama** | `JWT (JSON Web Token)`, `BCrypt`, `ASP.NET Identity`, `OAuth 2.0` |
| **Mesajlaşma** | `RabbitMQ`, `MassTransit 8`, `Publish/Subscribe`, `Event-Driven Architecture` |
| **Arama & Loglama** | `Elasticsearch 8.11`, `Kibana 8.11`, `Serilog`, `Serilog.Sinks.Elasticsearch` |
| **Önbellekleme** | `Redis (alpine)`, `StackExchange.Redis` |
| **Veritabanı** | `PostgreSQL 16` |
| **Konteynerleştirme** | `Docker`, `Docker Compose` |
| **Dökümantasyon** | `Swagger / OpenAPI`, `JWT Bearer Auth` |
| **Mimari Desenler** | `Mikroservis`, `Repository Pattern`, `Clean Architecture`, `Event-Driven` |

---

## 🧱 Sistem Mimarisi

```
                         ┌─────────────────────────────────────────┐
                         │           SmartCommerce.UI              │
                         │         (ASP.NET Core MVC)              │
                         │   Admin Panel  |  Kullanıcı Arayüzü     │
                         └────────────┬────────────────────────────┘
                                      │ HTTP (HttpClient)
              ┌───────────────────────┼─────────────────────┐
              ▼                       ▼                      ▼
      ┌──────────────┐     ┌──────────────────┐    ┌──────────────┐
      │   UserApi    │     │   ProductApi     │    │   OrderApi   │
      │  JWT Auth    │     │ Elasticsearch    │    │  Cart, Order │
      │  BCrypt      │     │ Redis Cache      │    │  Coupon      │
      │  PostgreSQL  │     │ PostgreSQL       │    │  PostgreSQL  │
      └──────────────┘     └──────────────────┘    └──────┬───────┘
                                                          │
                                              ┌───────────▼────────────┐
                                              │       RabbitMQ         │
                                              │  (order.created event) │
                                              └───┬───────┬────────────┘
                          ┌──────────────────┐    │       │    ┌──────────────────┐
                          │ NotificationWorker│◄──┘       └───►│  InvoiceWorker   │
                          │  Email / Bildirim │                 │  Fatura Oluştur  │
                          └──────────────────┘                 └──────────────────┘
                          ┌──────────────────┐    ┌──────────────────┐  ┌──────────────────┐
                          │   StockWorker    │    │   CargoWorker    │  │  PaymentWorker   │
                          │   Stok Düş       │    │  Kargo Oluştur   │  │  Ödeme İşle      │
                          └──────────────────┘    └──────────────────┘  └──────────────────┘

                    ┌────────────────────────────────────────────────────────┐
                    │              Ortak Altyapı (Docker Network)            │
                    │  PostgreSQL  │  Redis  │  RabbitMQ  │  Elasticsearch  │
                    │              │         │            │  + Kibana        │
                    └────────────────────────────────────────────────────────┘
```

---

## 📦 Servis Yapısı

```
SmartCommerce/
│
├── src/
│   ├── UserApi/                       # Kullanıcı kayıt, giriş, profil, JWT üretimi
│   │   ├── Controllers/               └─ AuthController
│   │   ├── Services/                  └─ AuthService, IAuthService
│   │   ├── Entities/                  └─ ApiUser
│   │   ├── Dtos/                      └─ LoginDto, RegisterDto, ProfileDto, ChangePasswordDto
│   │   └── Context/                   └─ UserDbContext
│   │
│   ├── ProductApi/                    # Ürün ve kategori CRUD, Elasticsearch arama, Redis cache
│   │   ├── Controllers/               └─ ProductController, CategoryController
│   │   ├── Services/                  └─ ProductService, ElasticsearchService, RedisService
│   │   ├── Entities/                  └─ Product, Category, ProductVariant
│   │   └── Context/                   └─ ProductDbContext
│   │
│   ├── OrderApi/                      # Sepet, sipariş, kupon; RabbitMQ event yayını
│   │   ├── Controllers/               └─ CartController, OrderController, CouponController
│   │   ├── Controllers/Admin/         └─ OrderDashboardController, UserDashboardController
│   │   ├── Services/                  └─ CartService, OrderService, CouponService
│   │   ├── Entities/                  └─ Order, OrderItem, CartItem, AppUser, Coupon
│   │   └── Context/                   └─ OrderContext
│   │
│   ├── SmartCommerce.UI/              # MVC frontend, kullanıcı arayüzü ve admin panel
│   │   ├── Controllers/               └─ HomeController, ProductController, CartController,
│   │   │                                  OrderController, FavoriteController,
│   │   │                                  ProfileController, AuthController, ReviewController
│   │   ├── Areas/Admin/
│   │   │   ├── Controllers/           └─ AdminController
│   │   │   ├── Services/              └─ AdminProductService, AdminOrderService,
│   │   │   │                              UserService, CargoService, CouponService, InvoiceService
│   │   │   ├── Abstract/              └─ Interface tanımları
│   │   │   ├── Dtos/                  └─ ProductDto, OrderDto, UserDto, CargoDto,
│   │   │   │                              CouponDto, InvoiceDto
│   │   │   └── Context/               └─ AdminDbContext, CargoEntity, InvoiceEntity
│   │   └── Views/                     └─ Home, Product, Cart, Order, Favorite,
│   │                                      Profile, Auth, Admin sayfaları
│   │
│   ├── NotificationWorker/            # RabbitMQ'dan order.created dinler → bildirim/e-posta
│   ├── InvoiceWorker/                 # order.created → fatura oluşturur, PostgreSQL'e kaydeder
│   ├── CargoWorker/                   # order.created → kargo kaydı oluşturur
│   ├── StockWorker/                   # order.created → ürün stoğunu düşürür
│   └── PaymentWorker/                 # order.created → ödeme işlemi gerçekleştirir
│
├── shared/
│   └── SmartCommerce.Shared/          # Tüm servislerce paylaşılan ortak tipler
│       ├── Entities/                  └─ Cargo, OrderCreated, OrderCreatedItem
│       └── Enums/                     └─ CargoStatus, UserRole, DiscountType
│
├── docker-compose.yml
└── seed_products.sql
```

---

## 🔁 Event-Driven Akış — Sipariş Yaşam Döngüsü

```
Kullanıcı "Sipariş Ver" butonuna basar
              │
              ▼
         OrderApi
   1. Validasyon (ürün/stok kontrolü)
   2. PostgreSQL'e sipariş kaydeder
   3. Redis cache günceller
   4. RabbitMQ'ya "order.created" event yayınlar
   5. 201 Created döner
              │
              ▼
         RabbitMQ
    order.created event
    (Fanout / Pub-Sub)
    ┌──────────┬──────────┬──────────┬──────────┐
    ▼          ▼          ▼          ▼          ▼
Notification  Invoice  Cargo    Stock     Payment
 Worker       Worker   Worker   Worker    Worker
  │             │        │        │          │
Bildirim     Fatura   Kargo    Stoku       Ödeme
oluştur      kaydet   oluştur   düş        işle
```

Her worker **bağımsız** çalışır; birbirini beklemez, birbirini etkilemez. Tek bir event 5 kuyruğa düşer.

---

## 🐳 Docker — Altyapı Servisleri

Tüm bağımlılıklar Docker Compose ile tek komutta ayağa kalkar:

```yaml
services:
  rabbitmq:
    image: rabbitmq:3-management
    container_name: smartcommerce-rabbitmq
    ports:
      - "5674:5672"      # AMQP
      - "15674:15672"    # Management UI → http://localhost:15674
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest

  redis:
    image: redis:alpine
    container_name: smartcommerce-redis
    ports:
      - "6381:6379"

  postgres:
    image: postgres:16
    container_name: smartcommerce-postgres
    ports:
      - "5433:5432"
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: admin123
      POSTGRES_DB: SmartCommerce

  elasticsearch:
    image: elasticsearch:8.11.0
    container_name: smartcommerce-elasticsearch
    environment:
      - discovery.type=single-node
      - xpack.security.enabled=false
      - ES_JAVA_OPTS=-Xms512m -Xmx512m
    ports:
      - "9204:9200"

  kibana:
    image: kibana:8.11.0
    container_name: smartcommerce-kibana
    ports:
      - "5604:5601"     # Kibana UI → http://localhost:5604
    depends_on:
      - elasticsearch

networks:
  smartcommerce-net:
    driver: bridge
```

### Konteyner Port Özeti

| Servis | Container Adı | Dış Port | İç Port |
|--------|---------------|----------|---------|
| RabbitMQ (AMQP) | smartcommerce-rabbitmq | 5674 | 5672 |
| RabbitMQ (UI) | smartcommerce-rabbitmq | 15674 | 15672 |
| Redis | smartcommerce-redis | 6381 | 6379 |
| PostgreSQL | smartcommerce-postgres | 5433 | 5432 |
| Elasticsearch | smartcommerce-elasticsearch | 9204 | 9200 |
| Kibana | smartcommerce-kibana | 5604 | 5601 |

---

## 🔐 JWT Kimlik Doğrulama

UserApi, kullanıcı girişinde `JWT Access Token` üretir. Tüm korumalı endpoint'ler bu token'ı `Authorization: Bearer <token>` header'ı ile doğrular.

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
        };
    });
```

### JWT Akışı

```
1. POST /api/auth/login   → Credentials gönder
2. UserApi → BCrypt ile şifre doğrula
3. JWT token üret (HS256, 7 günlük)
4. Token döner
5. Sonraki isteklerde: Authorization: Bearer <token>
6. Swagger UI'da 🔒 Authorize → "Bearer eyJhbGci..."
```

---

## 📊 Serilog + Elasticsearch + Kibana — Merkezi Loglama

Her servis (API + Worker), loglarını hem konsola hem de **Elasticsearch'e** yazar. Kibana üzerinden tüm loglar tek panelde izlenebilir.

```csharp
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
        new Uri(configuration["Elasticsearch:Uri"]!))
    {
        IndexFormat = "productapi-logs-{0:yyyy.MM}",
        AutoRegisterTemplate = true
    })
    .CreateLogger();
```

### Index Formatları

| Servis | Elasticsearch Index |
|--------|-------------------|
| UserApi | `userapi-logs-yyyy.MM` |
| ProductApi | `productapi-logs-yyyy.MM` |
| OrderApi | `orderapi-logs-yyyy.MM` |
| NotificationWorker | `notificationworker-logs-yyyy.MM` |
| InvoiceWorker | `invoiceworker-logs-yyyy.MM` |
| CargoWorker | `cargoworker-logs-yyyy.MM` |
| StockWorker | `stockworker-logs-yyyy.MM` |
| PaymentWorker | `paymentworker-logs-yyyy.MM` |

Kibana'ya erişim: `http://localhost:5604`

---

## ⚡ Redis — Önbellekleme & Session

Redis iki amaçla kullanılır:

**1. Ürün Cache (ProductApi)**
```csharp
// Ürün listesi cache'e alınır
await _redis.SetStringAsync($"product:{id}", JsonSerializer.Serialize(product));

// Cache'den oku, yoksa DB'den çek
var cached = await _redis.GetStringAsync($"product:{id}");
```

**2. Sepet Yönetimi (OrderApi)**  
Kullanıcının sepeti Redis'te `cart:{userId}` anahtarıyla tutulur. Hızlı okuma/yazma sağlar, sipariş onaylandığında temizlenir.

**Bağlantı:** `localhost:6381`

---

## 🐇 RabbitMQ + MassTransit — Mesaj Kuyruğu

MassTransit, RabbitMQ üzerinde Publish/Subscribe pattern'ını soyutlar. Manuel kuyruk tanımına gerek yoktur; `ConfigureEndpoints` ile otomatik oluşturulur.

```csharp
// OrderApi — Event Yayınla
await _publishEndpoint.Publish(new OrderCreatedEvent
{
    OrderId = order.Id,
    UserId = order.UserId,
    TotalAmount = order.TotalAmount,
    Items = order.Items.Select(i => new OrderCreatedItem
    {
        ProductId = i.ProductId,
        Quantity = i.Quantity,
        Price = i.Price
    }).ToList()
});
```

```csharp
// Worker — Event Tüket
public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var order = context.Message;
        // İş mantığı burada
    }
}
```

RabbitMQ Management UI: `http://localhost:15674`  
Kullanıcı: `guest` | Şifre: `guest`

---

## 🔍 Elasticsearch — Ürün Arama

ProductApi, ürünleri PostgreSQL'e kaydederken aynı zamanda Elasticsearch'e indeksler. Gelişmiş full-text arama ve filtreleme bu index üzerinden yapılır.

```csharp
// Index oluştur
var settings = new ConnectionSettings(new Uri(elasticUri))
    .DefaultIndex("products");
var client = new ElasticClient(settings);

// Ürün indeksle
await _elasticClient.IndexDocumentAsync(product);

// Ara
var result = await _elasticClient.SearchAsync<Product>(s => s
    .Query(q => q
        .MultiMatch(m => m
            .Fields(f => f.Field(p => p.Name).Field(p => p.Brand))
            .Query(searchTerm)
        )
    )
);
```

Re-index endpoint: `POST /api/product/reindex`

---

## 🗄️ Veritabanı Şeması

### Users
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | UUID | Primary key |
| FullName | text | Ad soyad |
| Email | text | E-posta (unique) |
| PasswordHash | text | BCrypt hash |
| Role | int4 | 0: Customer, 1: Admin |
| PhoneNumber | text | Telefon |
| Gender | text | Cinsiyet |
| City | text | Şehir |
| CreatedAt | timestamptz | Kayıt tarihi |

### Products
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | UUID | Primary key |
| Name | text | Ürün adı |
| Brand | text | Marka |
| Price | numeric | Fiyat |
| Stock | int4 | Stok adedi |
| CategoryId | UUID | Foreign key |
| IsActive | bool | Aktif mi |
| ImageUrl | text | Görsel URL |
| CreatedAt | timestamptz | Oluşturma tarihi |

### Orders
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | UUID | Primary key |
| UserId | UUID | Kullanıcı |
| TotalAmount | numeric | Toplam tutar |
| Address | text | Teslimat adresi |
| Status | int4 | 0: Bekliyor, 1: Tamamlandı, 2: İptal |
| CreatedAt | timestamptz | Sipariş tarihi |

### Cargos
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | UUID | Primary key |
| OrderId | UUID | Sipariş |
| UserId | UUID | Kullanıcı |
| TrackingNumber | text | Takip numarası |
| Status | int4 | 0: Hazırlanıyor, 1: Kargoda, 2: Teslim Edildi |
| CreatedAt | timestamptz | Oluşturma tarihi |

### Coupons
| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | UUID | Primary key |
| Code | text | Kupon kodu |
| DiscountType | int4 | 0: Yüzde, 1: Sabit |
| DiscountValue | numeric | İndirim miktarı |
| MinOrderAmount | numeric | Min sipariş tutarı |
| UsageLimit | int4 | Kullanım limiti |
| UsageCount | int4 | Kullanım sayısı |
| IsActive | bool | Aktif mi |
| ExpiresAt | timestamptz | Son kullanma tarihi |

---

## 🌐 API Dökümantasyonu

Her API, JWT destekli **Swagger UI** ile dökümante edilmiştir:

| Servis | Swagger URL |
|--------|-------------|
| UserApi | `https://localhost:7038/swagger` |
| ProductApi | `https://localhost:7136/swagger` |
| OrderApi | `https://localhost:7124/swagger` |

### Swagger'da JWT ile Test

```
1. POST /api/auth/login  →  token al
2. Sağ üstteki 🔒 Authorize butonuna tıkla
3. "Bearer eyJhbGci..."  yapıştır
4. Artık tüm istekler token ile gönderilir
```

---

## ⚙️ Kurulum

### 1. Ön Koşullar

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Visual Studio 2022 / Rider

### 2. Repoyu Klonla

```bash
git clone https://github.com/BerkayGenceroglu/SmartCommerce.BerkayStore.git
cd SmartCommerce.BerkayStore
```

### 3. Docker Altyapısını Başlat

```bash
docker compose up -d
```

Tüm konteynerler ayağa kalktıktan sonra:

| Servis | URL |
|--------|-----|
| RabbitMQ Management | http://localhost:15674 |
| Kibana | http://localhost:5604 |
| PostgreSQL | localhost:5433 |
| Redis | localhost:6381 |
| Elasticsearch | http://localhost:9204 |

### 4. appsettings.json Yapılandırması

Her proje için `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=SmartCommerce;Username=admin;Password=admin123"
  },
  "Redis": {
    "ConnectionString": "localhost:6381"
  },
  "Jwt": {
    "Key": "smartcommerce-super-secret-key-2024-must-be-long",
    "Issuer": "https://auth.smartcommerce.com",
    "Audience": "https://api.smartcommerce.com",
    "ExpiresDays": 7
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5674,
    "Username": "guest",
    "Password": "guest"
  },
  "Elasticsearch": {
    "Uri": "http://localhost:9204"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  },
  "AllowedHosts": "*"
}
```

### 5. Veritabanı Migration

Package Manager Console'da sırayla her projeyi seçip:

```bash
# UserApi
Add-Migration InitialCreate -Project UserApi
Update-Database -Project UserApi

# ProductApi
Add-Migration InitialCreate -Project ProductApi
Update-Database -Project ProductApi

# OrderApi
Add-Migration InitialCreate -Project OrderApi
Update-Database -Project OrderApi

# InvoiceWorker
Add-Migration InitialCreate -Project InvoiceWorker
Update-Database -Project InvoiceWorker

# CargoWorker
Add-Migration InitialCreate -Project CargoWorker
Update-Database -Project CargoWorker
```

### 6. Seed Verisi

```sql
-- DBeaver veya psql ile çalıştır
\i seed_products.sql
```

### 7. Elasticsearch Re-Index

```
POST https://localhost:7136/api/product/reindex
```

### 8. Projeleri Başlat

Visual Studio → **Multiple Startup Projects** ayarla:

```
✅ UserApi
✅ ProductApi
✅ OrderApi
✅ SmartCommerce.UI
✅ NotificationWorker
✅ InvoiceWorker
✅ CargoWorker
✅ StockWorker
✅ PaymentWorker
```

---

## ✨ Temel Özellikler

### Kullanıcı Tarafı
- 🛍️ Ürün listeleme, kategori ve filtre desteği
- 🔍 Elasticsearch destekli full-text ürün arama
- 🛒 Redis tabanlı sepet yönetimi
- 💸 Kupon kodu ile indirim uygulama
- 📦 Sipariş oluşturma ve geçmiş takibi
- ⭐ Ürün değerlendirme ve yorum sistemi
- ❤️ Favori ürün listesi
- 👤 Profil yönetimi ve şifre değiştirme

### Admin Paneli
- 📊 Dashboard — toplam ürün, sipariş, kullanıcı, satış istatistikleri
- 🏷️ Ürün ve kategori yönetimi (CRUD)
- 📦 Sipariş yönetimi ve durum güncelleme
- 👥 Kullanıcı yönetimi ve rol atama (Admin/Customer)
- 🚚 Kargo takibi ve durum güncelleme
- 💸 Kupon oluşturma ve yönetme
- 🧾 Fatura listeleme ve görüntüleme

### Teknik Özellikler
- 🔑 JWT Bearer tabanlı kimlik doğrulama (tüm servislerde)
- 📨 RabbitMQ + MassTransit ile event-driven asenkron iş akışı
- 📊 Serilog + Elasticsearch + Kibana ile merkezi log yönetimi
- ⚡ Redis ile hızlı ürün cache ve sepet yönetimi
- 🔍 Elasticsearch ile gelişmiş ürün arama ve indeksleme
- 🐳 Docker Compose ile tek komutta altyapı kurulumu
- 📄 Swagger UI ile JWT destekli API dökümantasyonu
- 🏗️ Shared kütüphane ile servisler arası ortak tip paylaşımı

---

## 👤 Geliştirici

**Berkay Gençeroğlu**

- 🐙 GitHub: [@BerkayGenceroglu](https://github.com/BerkayGenceroglu)
- 💼 LinkedIn: [Berkay Gençeroğlu](https://www.linkedin.com/in/berkay-gencero%C4%9Flu-586b52331/)
- 📧 E-posta: berkaygenceroglu6@gmail.com

---

## 💬 Son Söz

Bu proje; mikroservis mimarisi, event-driven tasarım, merkezi loglama ve modern DevOps pratiklerini kapsamlı bir şekilde hayata geçiren **portfolyo odaklı** bir çalışmadır. Her katman bilinçli teknoloji seçimleriyle inşa edilmiştir.

Geri bildirim ve katkılara her zaman açığım. **İyi kodlamalar! 🚀**
