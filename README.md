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
<img width="1861" height="953" alt="image" src="https://github.com/user-attachments/assets/15726062-0138-4653-aaba-1e741ebcbc23" />


<!-- REGISTER -->
**Kayıt Ol**
<img width="1868" height="955" alt="image" src="https://github.com/user-attachments/assets/b36e8379-7dd5-471a-97be-c38f5cc0f61c" />


</details>

---
 
### 🏠 Kullanıcı Arayüzü

<details>
<summary>Ana Sayfa</summary>

<img width="1868" height="954" alt="image" src="https://github.com/user-attachments/assets/9f0ba8ab-5492-4c0e-a98c-ab493f4d15c9" />
<img width="1859" height="875" alt="image" src="https://github.com/user-attachments/assets/d1718b53-6949-4429-a942-2a71d5e53a2c" />
<img width="1865" height="957" alt="image" src="https://github.com/user-attachments/assets/4f431e71-17b8-406c-9371-2f8b6d25070b" />
<img width="1866" height="958" alt="image" src="https://github.com/user-attachments/assets/a248c8ee-330d-4f62-bff2-c06012ba0483" />
<img width="1866" height="955" alt="image" src="https://github.com/user-attachments/assets/27a72bc8-0bbf-48f3-9670-32afb104cc69" />
<img width="1871" height="955" alt="image" src="https://github.com/user-attachments/assets/ea372f26-15cd-4a4d-8bf6-0035101936a1" />
<img width="1852" height="695" alt="image" src="https://github.com/user-attachments/assets/d8b99be1-dd6e-4b76-ae23-ed92d338ac5b" />
<img width="1869" height="953" alt="image" src="https://github.com/user-attachments/assets/02043083-aa78-480e-bbea-efd0b69f3f78" />

</details>

<details>
<summary>Ürün Listeleme & Arama</summary>

<img width="1854" height="953" alt="image" src="https://github.com/user-attachments/assets/5b562dc5-6355-441b-b1bc-c5905690eeb7" />
<img width="1865" height="961" alt="image" src="https://github.com/user-attachments/assets/f52d7ba3-6232-4f73-a449-e05404a710c5" />
<img width="1865" height="956" alt="image" src="https://github.com/user-attachments/assets/064045ad-3238-43f1-848e-afc2e4c0ae88" />
<img width="1869" height="952" alt="image" src="https://github.com/user-attachments/assets/d5ecb7c3-6a9b-4722-bc2e-cf90ad3edfbb" />
<img width="1864" height="955" alt="image" src="https://github.com/user-attachments/assets/7ae42e1d-57a7-46f7-85fc-9fe250690792" />
<img width="1866" height="959" alt="image" src="https://github.com/user-attachments/assets/90897b0a-1cb1-4df4-82f4-abff0e10bedb" />
<img width="1865" height="950" alt="image" src="https://github.com/user-attachments/assets/9f9194ff-f26e-441c-9ad2-b7966ede77d9" />
<img width="1868" height="950" alt="image" src="https://github.com/user-attachments/assets/8e5dc473-aaae-4b4c-8f89-bbe1e665205e" />


</details>

<details>
<summary>Ürün Detay Sayfası</summary>

<img width="1874" height="962" alt="image" src="https://github.com/user-attachments/assets/699e2480-9689-4313-b561-7b47bb22edc8" />
<img width="1869" height="962" alt="image" src="https://github.com/user-attachments/assets/03f6ed01-67fb-48a5-9493-8cbe64f3f835" />
<img width="1865" height="955" alt="image" src="https://github.com/user-attachments/assets/f19f324b-c329-42eb-aa9e-952e7e5620f8" />


</details>

<details>
<summary>Ürün Detay Yorum ve Değerlendirme</summary>
<img width="1866" height="960" alt="image" src="https://github.com/user-attachments/assets/2e4a2694-be32-4033-a7af-5164f2c0498d" />
<img width="1871" height="956" alt="image" src="https://github.com/user-attachments/assets/c74f24a9-d230-4404-bb05-0eb99fdbb7f9" />



</details>****

<details>
<summary>Favoriler Sayfası</summary>

<img width="1868" height="956" alt="image" src="https://github.com/user-attachments/assets/6b263123-8fd0-4472-b64d-34e6b4e127e6" />
<img width="1874" height="957" alt="image" src="https://github.com/user-attachments/assets/b7c7153f-e2dd-488a-8a27-f41a6e2b0445" />


</details>

<details>
<summary>Sepet Sayfası</summary>
<img width="1865" height="953" alt="image" src="https://github.com/user-attachments/assets/0c95751c-f4d9-4fea-942c-bccdb71e5cad" />
<img width="1864" height="953" alt="image" src="https://github.com/user-attachments/assets/67d70c53-91c1-47ff-ab1b-ecb4b1ef874a" />
<img width="1870" height="951" alt="image" src="https://github.com/user-attachments/assets/fc34838b-8c3a-493f-b718-4fe710e44411" />
<img width="1869" height="954" alt="image" src="https://github.com/user-attachments/assets/373fed01-195e-4a28-9574-c61bf34bef2e" />
<img width="1863" height="955" alt="image" src="https://github.com/user-attachments/assets/bdccb3c6-fb47-42f9-b151-34d33314bda8" />



</details>


<summary>Sipariş Geçmişi & Profil</summary>

<img width="1860" height="957" alt="image" src="https://github.com/user-attachments/assets/cfebb72a-5e71-4e6b-a45e-4c96dcd733ea" />

</details>

<details>

<img width="1866" height="956" alt="image" src="https://github.com/user-attachments/assets/7d7a38f4-31ef-4bde-a9c0-6f59d70e8df3" />
<img width="1864" height="939" alt="image" src="https://github.com/user-attachments/assets/3b443725-0236-483d-a7b5-7b53dceaa044" />

</details>

---

### 🛡️ Admin Paneli

<details>
<summary>Giriş</summary>

<img width="1867" height="952" alt="image" src="https://github.com/user-attachments/assets/ba1671a3-c038-4f54-a1af-c830fefc0442" />

</details>

<details>
<summary>Dashboard</summary>
<img width="1869" height="953" alt="image" src="https://github.com/user-attachments/assets/58a9bca6-d21b-45bb-a0d6-ce803317a69d" />
<img width="1603" height="499" alt="image" src="https://github.com/user-attachments/assets/274b3d45-2287-47c6-9771-42844a097af7" />



</details>

<details>
<summary>Ürün Yönetimi</summary>
<img width="1867" height="953" alt="image" src="https://github.com/user-attachments/assets/555b9362-d2bc-4c63-8971-271bfa4f0096" />
<img width="1871" height="953" alt="image" src="https://github.com/user-attachments/assets/bd798789-9ad7-49f2-9963-7a17a027014d" />
<img width="1870" height="953" alt="image" src="https://github.com/user-attachments/assets/09743b82-ab40-4502-80a8-c3d0974f4bb4" />



</details>

<details>
<summary>Sipariş Yönetimi</summary>
<img width="1869" height="956" alt="image" src="https://github.com/user-attachments/assets/68699751-155e-486b-9d10-74cdcb89d03a" />
<img width="1866" height="955" alt="image" src="https://github.com/user-attachments/assets/234457d5-a9e6-4e80-b72c-17199d158303" />



</details>


<details>
<summary>Kargo & Fatura Yönetimi</summary>

<img width="1869" height="954" alt="image" src="https://github.com/user-attachments/assets/caafc90f-ceb6-4441-9673-9185e4e671dc" />

<img width="1585" height="737" alt="image" src="https://github.com/user-attachments/assets/4586f5dd-d29e-4d65-b392-a52ff7ae2422" />

<img width="1566" height="401" alt="image" src="https://github.com/user-attachments/assets/64756a42-014e-4066-a70e-64a26f8d772c" />

<img width="1543" height="360" alt="image" src="https://github.com/user-attachments/assets/0c9bf35d-4d63-4152-9e95-a28149d40364" />

</details>

<details>
<summary>Kupon Yönetimi</summary>


<img width="1868" height="955" alt="image" src="https://github.com/user-attachments/assets/321e3e22-bc4e-4162-9c01-fd08e14d148e" />

</details>

<details>
<summary>Kullanıcı Yönetimi</summary>

<img width="1868" height="958" alt="image" src="https://github.com/user-attachments/assets/1f300262-dfda-4a94-8d9f-13115b8fce94" />

</details>

<details>
<summary>Fatura Yönetimi</summary>

<img width="1868" height="955" alt="image" src="https://github.com/user-attachments/assets/7d5e2df5-7e60-4891-9146-d16150833b98" />
<img width="1869" height="956" alt="image" src="https://github.com/user-attachments/assets/4dcf82ec-fc5e-4d25-b580-13452a356595" />
<img width="1861" height="960" alt="image" src="https://github.com/user-attachments/assets/f7a2c779-cb44-480f-8cf5-adb21ea4a93f" />

</details>

<details>
<summary>Yorum Yönetimi</summary>

<img width="1869" height="955" alt="image" src="https://github.com/user-attachments/assets/cbbddf9c-c376-4a1c-b36e-813da7c5cbbd" />
<img width="1864" height="956" alt="image" src="https://github.com/user-attachments/assets/ca2c9d5b-785b-4592-ac89-8b7c3eb704e9" />


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
