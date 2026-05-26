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
╔══════════════════════════════════════════════════════════════╗
║                    SmartCommerce.UI                          ║
║              ASP.NET Core MVC  |  Razor Views                ║
║         👤 Kullanıcı Arayüzü   |   🛡️ Admin Paneli           ║
╚══════════════╤═══════════════════════════╤═══════════════════╝
               │      HTTP / HttpClient    │
     ┌─────────┼───────────────────────────┼─────────┐
     ▼         ▼                           ▼         ▼
┌─────────┐ ┌──────────────────┐ ┌──────────────────────────┐
│ UserApi │ │   ProductApi     │ │        OrderApi           │
│─────────│ │──────────────────│ │──────────────────────────│
│ • Kayıt │ │ • Ürün CRUD      │ │ • Sepet Yönetimi (Redis)  │
│ • Giriş │ │ • Elasticsearch  │ │ • Sipariş Oluşturma       │
│ • JWT   │ │   (Arama/Index)  │ │ • Kupon Doğrulama         │
│ • Profil│ │ • Redis Cache    │ │ • RabbitMQ Event Yayını   │
│ • BCrypt│ │ • Kategori CRUD  │ │ • Dashboard (Admin)       │
└────┬────┘ └────────┬─────────┘ └────────────┬─────────────┘
     │               │                        │
     └───────────────┴──────── PostgreSQL ─────┘
                                              │
                                             ▼
                              ╔══════════════════════════╗
                              ║         RabbitMQ          ║
                              ║   📨 order.created event  ║
                              ║   (Fanout / Pub-Sub)      ║
                              ╚══╤═══╤════╤═════╤════════╝
              ┌──────────────────┘   │    │     └─────────────────┐
              ▼                      ▼    ▼                        ▼
   ┌─────────────────┐  ┌──────────────────┐  ┌──────────────────────────┐
   │NotificationWorker│  │  InvoiceWorker   │  │      CargoWorker          │
   │─────────────────│  │──────────────────│  │──────────────────────────│
   │ 📧 E-posta Gönder│  │ 🧾 Fatura Oluştur│  │ 🚚 Kargo Kaydı Oluştur   │
   │ 🔔 Bildirim Yaz  │  │ 📄 DB'ye Kaydet  │  │ 📦 Takip No Ata           │
   └─────────────────┘  └──────────────────┘  └──────────────────────────┘
              ▼                                          ▼
   ┌─────────────────┐                    ┌──────────────────────────┐
   │   StockWorker   │                    │      PaymentWorker        │
   │─────────────────│                    │──────────────────────────│
   │ 📉 Stok Düş     │                    │ 💳 Ödeme İşlemi Gerçekleş│
   │ 🔄 Stok Güncelle│                    │ 📝 Ödeme Kaydı Oluştur   │
   └─────────────────┘                    └──────────────────────────┘

╔══════════════════════════════════════════════════════════════════════╗
║                    🐳 Docker Altyapısı (smartcommerce-net)           ║
║                                                                      ║
║   🐘 PostgreSQL    🔴 Redis    🐇 RabbitMQ    🔍 Elasticsearch       ║
║   (port: 5433)   (port: 6381) (port: 5674)   (port: 9204)           ║
║                                                  📊 Kibana           ║
║                                                  (port: 5604)        ║
╚══════════════════════════════════════════════════════════════════════╝
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
│   ├── NotificationWorker/            # RabbitMQ'dan order.created dinler → e-posta / bildirim
│   ├── InvoiceWorker/                 # order.created → fatura oluşturur, PostgreSQL'e kaydeder
│   ├── CargoWorker/                   # order.created → kargo kaydı ve takip numarası oluşturur
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
 ┌──────────────────────────────────────────────────────────────────┐
 │   👤 Kullanıcı "Sipariş Ver" butonuna basar                       │
 └──────────────────────────────┬───────────────────────────────────┘
                                ▼
 ┌──────────────────────────────────────────────────────────────────┐
 │                         OrderApi                                  │
 │                                                                   │
 │   ① Validasyon     →  Ürün var mı? Stok yeterli mi?              │
 │   ② Kayıt          →  PostgreSQL'e sipariş yazılır               │
 │   ③ Cache          →  Redis güncellenir                           │
 │   ④ Event Yayını   →  RabbitMQ'ya "order.created" gönderilir     │
 │   ⑤ Yanıt          →  201 Created döner, kullanıcı yönlendirilir │
 └──────────────────────────────┬───────────────────────────────────┘
                                ▼
 ┌──────────────────────────────────────────────────────────────────┐
 │                   🐇 RabbitMQ                                     │
 │          "order.created" event — Fanout / Pub-Sub                 │
 │   Aynı event 5 farklı kuyruğa düşer, her worker kendi kuyruğunu  │
 │   bağımsız olarak tüketir. Hiçbiri diğerini beklemez.            │
 └───┬──────────┬──────────┬──────────┬──────────────┬─────────────┘
     │          │          │          │              │
     ▼          ▼          ▼          ▼              ▼
┌─────────┐ ┌────────┐ ┌────────┐ ┌────────┐  ┌─────────┐
│Notifi-  │ │Invoice │ │ Cargo  │ │ Stock  │  │Payment  │
│cation   │ │Worker  │ │Worker  │ │Worker  │  │Worker   │
│Worker   │ │        │ │        │ │        │  │         │
│─────────│ │────────│ │────────│ │────────│  │─────────│
│📧 E-posta│ │🧾Fatura│ │🚚Kargo │ │📉Stok  │  │💳Ödeme  │
│🔔Bildirim│ │  Oluşt │ │  Kayıt │ │  Düşür │  │  İşle   │
│  Oluştur│ │  DB'ye │ │  Takip │ │  DB    │  │  DB'ye  │
│  DB'ye  │ │  Kaydet│ │  No Ata│ │  Güncl │  │  Kaydet │
└─────────┘ └────────┘ └────────┘ └────────┘  └─────────┘
```

> 💡 **MassTransit** kuyrukları otomatik oluşturur. Her worker yalnızca kendi `IConsumer<OrderCreatedEvent>` implementasyonunu yazar, routing tamamen soyutlanmıştır.

---

## 🐳 Docker — Altyapı Servisleri

Tüm bağımlılıklar Docker Compose ile tek komutta ayağa kalkar:

```bash
docker compose up -d
```

```yaml
services:
  rabbitmq:
    image: rabbitmq:3-management
    container_name: smartcommerce-rabbitmq
    ports:
      - "5674:5672"      # AMQP
      - "15674:15672"    # Management UI
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
      - "5604:5601"
    depends_on:
      - elasticsearch

networks:
  smartcommerce-net:
    driver: bridge
```

### Konteyner Port Özeti

| Servis | Container Adı | Dış Port | Açıklama |
|--------|---------------|----------|----------|
| RabbitMQ (AMQP) | smartcommerce-rabbitmq | **5674** | Mesaj kuyruğu bağlantısı |
| RabbitMQ (UI) | smartcommerce-rabbitmq | **15674** | `http://localhost:15674` |
| Redis | smartcommerce-redis | **6381** | Cache & Sepet |
| PostgreSQL | smartcommerce-postgres | **5433** | Ana veritabanı |
| Elasticsearch | smartcommerce-elasticsearch | **9204** | Arama & Loglama |
| Kibana | smartcommerce-kibana | **5604** | `http://localhost:5604` |

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
① POST /api/auth/login      →  Email + şifre gönder
② UserApi                   →  BCrypt ile şifre doğrula
③ Token Üretimi             →  HS256, 7 günlük JWT oluştur
④ Token Döner               →  Access Token client'a iletilir
⑤ Korumalı İstek            →  Authorization: Bearer eyJhbGci...
⑥ Swagger'da Test           →  🔒 Authorize → "Bearer ..." yapıştır
```

---

## 📊 Serilog + Elasticsearch + Kibana — Merkezi Loglama

Her servis ve worker, loglarını hem konsola hem de **Elasticsearch'e** yazar. Kibana üzerinden tüm sistem logları tek panelde izlenebilir.

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

Kibana'ya erişim: `http://localhost:5604`

---

## ⚡ Redis — Önbellekleme & Sepet Yönetimi

Redis iki amaçla kullanılır:

**1. Ürün Cache (ProductApi)**
```csharp
// Ürünü cache'e al
await _redis.SetStringAsync($"product:{id}", JsonSerializer.Serialize(product));

// Cache'den oku, yoksa DB'den çek
var cached = await _redis.GetStringAsync($"product:{id}");
```

**2. Sepet Yönetimi (OrderApi)**
Kullanıcının sepeti Redis'te `cart:{userId}` anahtarıyla tutulur. Sipariş onaylandığında temizlenir.

**Bağlantı:** `localhost:6381`

---

## 🔍 Elasticsearch — Ürün Arama & İndeksleme

ProductApi, ürünleri PostgreSQL'e kaydederken aynı zamanda Elasticsearch'e indeksler.

```csharp
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

### 4. Seed Verisi

```sql
-- DBeaver veya psql ile çalıştır
\i seed_products.sql
```

### 5. Elasticsearch Re-Index

```
POST https://localhost:7136/api/product/reindex
```

### 6. Projeleri Başlat

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

## 📸 Ekran Görüntüleri

### 🔐 Kimlik Doğrulama

<details>
<summary>Giriş & Kayıt Sayfaları</summary>

<!-- LOGIN -->
**Giriş Yap**
> 📷 _Buraya login sayfası ekran görüntüsü ekle_

<!-- REGISTER -->
**Kayıt Ol**
> 📷 _Buraya register sayfası ekran görüntüsü ekle_

</details>

---

### 🏠 Kullanıcı Arayüzü

<details>
<summary>Ana Sayfa</summary>

> 📷 _Buraya ana sayfa (hero banner, öne çıkan ürünler, kategoriler) ekran görüntüsü ekle_

</details>

<details>
<summary>Ürün Listeleme & Arama</summary>

> 📷 _Buraya ürün listesi ve kategori filtresi ekran görüntüsü ekle_

</details>

<details>
<summary>Ürün Detay Sayfası</summary>

> 📷 _Buraya ürün detay (görsel, açıklama, fiyat, sepete ekle) ekran görüntüsü ekle_

</details>

<details>
<summary>Sepet Sayfası</summary>

> 📷 _Buraya sepet (ürün listesi, kupon kodu, toplam) ekran görüntüsü ekle_

</details>

<details>
<summary>Sipariş & Ödeme</summary>

> 📷 _Buraya checkout formu ve ödeme sayfası ekran görüntüsü ekle_

</details>

<details>
<summary>Sipariş Geçmişi & Profil</summary>

> 📷 _Buraya siparişlerim ve profil sayfası ekran görüntüsü ekle_

</details>

---

### 🛡️ Admin Paneli

<details>
<summary>Dashboard</summary>

> 📷 _Buraya admin dashboard (istatistikler, grafikler) ekran görüntüsü ekle_

</details>

<details>
<summary>Ürün Yönetimi</summary>

> 📷 _Buraya ürün listesi, ekleme ve düzenleme ekran görüntüsü ekle_

</details>

<details>
<summary>Sipariş Yönetimi</summary>

> 📷 _Buraya sipariş listesi ve detay ekran görüntüsü ekle_

</details>

<details>
<summary>Kullanıcı & Rol Yönetimi</summary>

> 📷 _Buraya kullanıcı listesi ve rol atama ekran görüntüsü ekle_

</details>

<details>
<summary>Kargo & Fatura Yönetimi</summary>

> 📷 _Buraya kargo listesi ve fatura ekran görüntüsü ekle_

</details>

<details>
<summary>Kupon Yönetimi</summary>

> 📷 _Buraya kupon oluşturma ve listeleme ekran görüntüsü ekle_

</details>

---

### 🐳 Docker & Altyapı

<details>
<summary>Docker Containers</summary>

> 📷 _Buraya Docker Desktop veya `docker ps` çıktısı ekran görüntüsü ekle_

</details>

<details>
<summary>RabbitMQ Management UI</summary>

> 📷 _Buraya RabbitMQ kuyruk ekranı (http://localhost:15674) ekran görüntüsü ekle_

</details>

<details>
<summary>Kibana Dashboard</summary>

> 📷 _Buraya Kibana log akışı ekran görüntüsü ekle_

</details>

<details>
<summary>Swagger UI</summary>

> 📷 _Buraya Swagger API dökümantasyonu ekran görüntüsü ekle_

</details>

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
