<div align="center">

<img width="1870" height="913" alt="banner" src="https://github.com/user-attachments/assets/30312b1a-9623-456e-a48e-5bbe3409a123" />

# SmartCommerce · Berkay Store

**Mikroservis mimarisi üzerine inşa edilmiş, production-ready e-ticaret platformu**

<p>
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/PostgreSQL-316192?style=flat-square&logo=postgresql&logoColor=white"/>
  <img src="https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white"/>
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=flat-square&logo=rabbitmq&logoColor=white"/>
  <img src="https://img.shields.io/badge/Elasticsearch-005571?style=flat-square&logo=elasticsearch&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white"/>
  <img src="https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white"/>
  <img src="https://img.shields.io/badge/Serilog-333333?style=flat-square"/>
  <img src="https://img.shields.io/badge/MassTransit-8.x-blue?style=flat-square"/>
</p>

[Özellikler](#-özellikler) · [Mimari](#-sistem-mimarisi) · [Kurulum](#-kurulum) · [Ekran Görüntüleri](#-ekran-görüntüleri) · [Geliştirici](#-geliştirici)

</div>

---

## Proje Hakkında

SmartCommerce, gerçek dünya üretim senaryolarını yansıtmak amacıyla tasarlanmış tam kapsamlı bir e-ticaret altyapısıdır. **10 bağımsız .NET 8 projesi** tek bir solution altında; bağımsız API servisleri, event-driven worker'lar, merkezi loglama, önbellekleme ve mesaj kuyruğu gibi modern yazılım mühendisliği pratiklerini bir arada uygular.

> Bu proje portfolyo amaçlı geliştirilmiştir. Gerçek bir ticari işletme değildir.

---

## Teknoloji Yığını

### Backend & Framework

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **.NET / ASP.NET Core** | 8.0 | Tüm API ve Worker servisleri |
| **Entity Framework Core** | 8.0 | ORM, code-first migration |
| **MassTransit** | 8.x | RabbitMQ mesaj kuyruğu yönetimi |
| **JWT Bearer** | — | Stateless kimlik doğrulama |
| **BCrypt.Net** | — | Güvenli şifre hashleme |
| **Serilog** | — | Yapılandırılmış merkezi loglama |
| **NEST / Elasticsearch.Net** | 8.x | Elasticsearch istemcisi |
| **StackExchange.Redis** | — | Redis önbellek istemcisi |
| **Npgsql EF Core** | — | PostgreSQL sağlayıcısı |
| **Newtonsoft.Json** | — | JSON serializasyon (UI katmanı) |

### Altyapı & Mesajlaşma

| Servis | Dış Port | İç Port | Kullanım Amacı |
|--------|----------|---------|----------------|
| **PostgreSQL 16** | 5433 | 5432 | Ana ilişkisel veritabanı |
| **Redis (alpine)** | 6381 | 6379 | Ürün cache + Sepet yönetimi |
| **RabbitMQ** | 5674 (AMQP) | 5672 | Servisler arası mesajlaşma |
| **RabbitMQ Management** | 15674 | 15672 | Kuyruk yönetim arayüzü |
| **Elasticsearch 8.11** | 9204 | 9200 | Full-text arama + loglama |
| **Kibana 8.11** | 5604 | 5601 | Elasticsearch görsel yönetim |

### Frontend

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| **ASP.NET Core MVC + Razor** | Server-side rendering |
| **Plus Jakarta Sans** | Ana font (display + body) |
| **Font Awesome 6.5** | İkon seti |
| **Chart.js 4.4** | Admin dashboard grafikleri |
| **Vanilla JavaScript + Fetch API** | Client-side etkileşimler |

---

## Sistem Mimarisi

```
┌─────────────────────────────────────────────────────────┐
│                  SmartCommerce.UI (7050)                 │
│         ASP.NET Core MVC · Razor · Admin Panel           │
└──────────┬──────────────────┬──────────────────┬─────────┘
           │ HTTP             │ HTTP             │ HTTP
    ┌──────▼──────┐   ┌───────▼──────┐   ┌──────▼───────┐
    │  UserApi    │   │  ProductApi  │   │   OrderApi   │
    │  :7038      │   │  :7136       │   │   :7124      │
    │─────────────│   │──────────────│   │──────────────│
    │ JWT · BCrypt│   │ Redis · NEST │   │ MassTransit  │
    │ Profil · Rol│   │ Kategori     │   │ Sepet · Kupon│
    └──────┬──────┘   └───────┬──────┘   └──────┬───────┘
           │                  │                  │
           └──────────────────┴─── PostgreSQL ───┘
                                        │
                              ┌─────────▼─────────┐
                              │     RabbitMQ       │
                              │  OrderCreated Event│
                              └──┬──┬──┬──┬────────┘
              ┌──────────────────┘  │  │  └──────────────┐
              ▼                     ▼  ▼                  ▼
    ┌──────────────┐  ┌──────────────┐  ┌──────┐  ┌──────────┐
    │Notification  │  │   Invoice    │  │Cargo │  │  Stock   │
    │   Worker     │  │   Worker     │  │Worker│  │  Worker  │
    │ Gmail SMTP   │  │ Fatura → DB  │  │Takip │  │ Stok ↓   │
    └──────────────┘  └──────────────┘  └──────┘  └──────────┘
```

### Solution Yapısı (10 Proje)

```
SmartCommerce/
├── UserApi/              → Port 7038  · Kimlik doğrulama, profil yönetimi
├── ProductApi/           → Port 7136  · Ürünler, kategoriler, arama
├── OrderApi/             → Port 7124  · Sepet, sipariş, kupon
├── SmartCommerce.UI/     → Port 7050  · MVC frontend + Admin paneli
├── NotificationWorker/   →            · E-posta bildirimleri
├── InvoiceWorker/        →            · Fatura oluşturma
├── CargoWorker/          →            · Kargo takip numarası
├── StockWorker/          →            · Stok güncelleme
├── PaymentWorker/        →            · Ödeme kaydı
└── Shared/               →            · Ortak entity, enum, event modelleri
```

---

## Akış Süreçleri

### Sipariş Akışı (Event-Driven)

```
Kullanıcı "Sipariş Ver" butonuna basar
              │
              ▼
         OrderApi
    ┌─────────────────────────────────────┐
    │ ① Sepet Redis'ten okunur            │
    │ ② Kupon varsa indirim uygulanır     │
    │ ③ PostgreSQL'e sipariş kaydedilir   │
    │ ④ RabbitMQ'ya OrderCreated yayınlanır│
    │ ⑤ Redis sepeti temizlenir           │
    └─────────────────────────────────────┘
              │
              ▼  (Fanout — 5 kuyruğa düşer)
    ┌─────────────────────────────────────┐
    │ NotificationWorker → E-posta gönder │
    │ InvoiceWorker      → Fatura kaydet  │
    │ CargoWorker        → Kargo oluştur  │
    │ StockWorker        → Stoğu düşür    │
    │ PaymentWorker      → Ödeme işle     │
    └─────────────────────────────────────┘
```

### Redis Cache Stratejisi

```
GET /api/product isteği gelir
        │
        ▼
Redis'te "products" key var mı?
        │
   Var  └──► Cache'den döner  (~1ms)
        │
   Yok  └──► PostgreSQL'den çek
              │
              ▼
         Redis'e yaz (TTL: 5dk)
              │
              ▼
         Response döner
```

### JWT Kimlik Doğrulama Akışı

```
POST /api/auth/login  →  Email + şifre
         │
         ▼
    BCrypt.Verify()
         │
         ▼
    JWT Token üret (HS256, 7 gün)
         │
         ▼
    Session'a kaydet (UI)
         │
         ▼
    Her API isteğinde → Authorization: Bearer {token}
```

---

## Özellikler

### Kullanıcı Tarafı

| Sayfa | Açıklama |
|-------|----------|
| **Ana Sayfa** | Hero banner, Elasticsearch destekli canlı arama, kategori grid |
| **Ürün Listesi** | Kategori / marka / fiyat / stok filtresi, sıralama |
| **Ürün Detay** | Varyant seçici (renk/beden/numara), fiyat modifier, stok durumu |
| **Sepet** | Redis tabanlı, 30 dk TTL, kupon uygulama, sipariş özeti |
| **Siparişlerim** | Sipariş takip çubuğu, ürün görseli, adres bilgisi |
| **Favorilerim** | Grid layout, favori ekle/çıkar |
| **Profil** | Tab yapısı: bilgi güncelleme + şifre değiştirme |
| **Kayıt** | Ad, email, şifre + opsiyonel telefon, cinsiyet, şehir |
| **Yorum Sistemi** | 1–5 yıldız puanlama, yorum metni, kullanıcı adı |

### Admin Paneli

| Sayfa | Açıklama |
|-------|----------|
| **Dashboard** | Sipariş/gelir istatistikleri, Chart.js doughnut, kritik stok listesi |
| **Ürünler** | Listeleme, kategori/marka/stok filtresi, sayfalama, silme |
| **Siparişler** | Listeleme, durum filtresi, detay modal |
| **Kargo** | Durum filtresi (Preparing/Shipped/Delivered), takip arama |
| **Kuponlar** | Oluşturma (tip, değer, limit, son tarih), silme |
| **Kullanıcılar** | Listeleme, rol filtresi, renkli avatar |
| **Faturalar** | Detay modal, tarayıcı yazdırma desteği |
| **Yorumlar** | Tüm yorumlar, en çok yorumlanan ürünler, ortalama puan |

---

## Ekran Görüntüleri

### Kimlik Doğrulama

#### Giriş Yap
> JWT token tabanlı, rol kontrolü ile admin/kullanıcı ayrımı yapılır.

<img width="1861" height="953" alt="Giriş Sayfası" src="https://github.com/user-attachments/assets/15726062-0138-4653-aaba-1e741ebcbc23" />

---

#### Kayıt Ol
> Ad soyad, e-posta ve şifre zorunludur. Telefon, cinsiyet ve şehir opsiyonel olarak alınır.

<img width="1868" height="955" alt="Kayıt Sayfası" src="https://github.com/user-attachments/assets/b36e8379-7dd5-471a-97be-c38f5cc0f61c" />

---

### Kullanıcı Arayüzü

#### Ana Sayfa
> Canlı kayan bilgi bandı, hero alanı, kategori kartları ve öne çıkan ürün bölümlerinden oluşur. Navbar'daki arama kutusu Elasticsearch'e bağlıdır.

<img width="1868" height="954" alt="Ana Sayfa 1" src="https://github.com/user-attachments/assets/9f0ba8ab-5492-4c0e-a98c-ab493f4d15c9" />
<img width="1859" height="875" alt="Ana Sayfa 2" src="https://github.com/user-attachments/assets/d1718b53-6949-4429-a942-2a71d5e53a2c" />
<img width="1865" height="957" alt="Ana Sayfa 3" src="https://github.com/user-attachments/assets/4f431e71-17b8-406c-9371-2f8b6d25070b" />
<img width="1866" height="958" alt="Ana Sayfa 4" src="https://github.com/user-attachments/assets/a248c8ee-330d-4f62-bff2-c06012ba0483" />
<img width="1866" height="955" alt="Ana Sayfa 5" src="https://github.com/user-attachments/assets/27a72bc8-0bbf-48f3-9670-32afb104cc69" />
<img width="1871" height="955" alt="Ana Sayfa 6" src="https://github.com/user-attachments/assets/ea372f26-15cd-4a4d-8bf6-0035101936a1" />
<img width="1852" height="695" alt="Ana Sayfa Footer" src="https://github.com/user-attachments/assets/d8b99be1-dd6e-4b76-ae23-ed92d338ac5b" />
<img width="1869" height="953" alt="Ana Sayfa 8" src="https://github.com/user-attachments/assets/02043083-aa78-480e-bbea-efd0b69f3f78" />

---

#### Ürün Listesi & Arama
> Sol sidebar'dan kategori, marka, fiyat ve stok filtresi uygulanır. Uygulanan filtreler etiket olarak gösterilir ve tek tıkla kaldırılabilir.

<img width="1854" height="953" alt="Ürün Listesi 1" src="https://github.com/user-attachments/assets/5b562dc5-6355-441b-b1bc-c5905690eeb7" />
<img width="1865" height="961" alt="Ürün Listesi 2" src="https://github.com/user-attachments/assets/f52d7ba3-6232-4f73-a449-e05404a710c5" />
<img width="1865" height="956" alt="Ürün Listesi 3" src="https://github.com/user-attachments/assets/064045ad-3238-43f1-848e-afc2e4c0ae88" />
<img width="1869" height="952" alt="Ürün Listesi 4" src="https://github.com/user-attachments/assets/d5ecb7c3-6a9b-4722-bc2e-cf90ad3edfbb" />
<img width="1864" height="955" alt="Ürün Listesi 5" src="https://github.com/user-attachments/assets/7ae42e1d-57a7-46f7-85fc-9fe250690792" />
<img width="1866" height="959" alt="Ürün Listesi 6" src="https://github.com/user-attachments/assets/90897b0a-1cb1-4df4-82f4-abff0e10bedb" />
<img width="1865" height="950" alt="Ürün Listesi 7" src="https://github.com/user-attachments/assets/9f9194ff-f26e-441c-9ad2-b7966ede77d9" />
<img width="1868" height="950" alt="Ürün Listesi 8" src="https://github.com/user-attachments/assets/8e5dc473-aaae-4b4c-8f89-bbe1e665205e" />

---

#### Ürün Detay
> Varyantlar (renk, beden, numara, kapasite vb.) dinamik butonlarla seçilir. Seçime göre fiyat farkı (PriceModifier) anlık güncellenir.

<img width="1874" height="962" alt="Ürün Detay 1" src="https://github.com/user-attachments/assets/699e2480-9689-4313-b561-7b47bb22edc8" />
<img width="1869" height="962" alt="Ürün Detay 2" src="https://github.com/user-attachments/assets/03f6ed01-67fb-48a5-9493-8cbe64f3f835" />
<img width="1865" height="955" alt="Ürün Detay 3" src="https://github.com/user-attachments/assets/f19f324b-c329-42eb-aa9e-952e7e5620f8" />

---

#### Yorum & Değerlendirme
> Giriş yapmış kullanıcılar 1–5 yıldız puan ve yorum bırakabilir. Her kullanıcı bir ürüne yalnızca bir yorum yapabilir. Genel puan ortalaması ve yıldız dağılım grafiği gösterilir.

<img width="1866" height="960" alt="Yorum 1" src="https://github.com/user-attachments/assets/2e4a2694-be32-4033-a7af-5164f2c0498d" />
<img width="1871" height="956" alt="Yorum 2" src="https://github.com/user-attachments/assets/c74f24a9-d230-4404-bb05-0eb99fdbb7f9" />

---

#### Favorilerim
> Ürün kartlarındaki kalp ikonuyla favoriye ekleme/çıkarma yapılır. Her üründen doğrudan sepete ekleme yapılabilir.

<img width="1868" height="956" alt="Favoriler 1" src="https://github.com/user-attachments/assets/6b263123-8fd0-4472-b64d-34e6b4e127e6" />
<img width="1874" height="957" alt="Favoriler 2" src="https://github.com/user-attachments/assets/b7c7153f-e2dd-488a-8a27-f41a6e2b0445" />

---

#### Sepet
> Sepet verileri Redis'te `cart:{userId}` anahtarıyla tutulur, 30 dakika TTL uygulanır. Kupon kodu ile yüzde veya sabit tutar indirimi uygulanabilir.

<img width="1865" height="953" alt="Sepet 1" src="https://github.com/user-attachments/assets/0c95751c-f4d9-4fea-942c-bccdb71e5cad" />
<img width="1864" height="953" alt="Sepet 2" src="https://github.com/user-attachments/assets/67d70c53-91c1-47ff-ab1b-ecb4b1ef874a" />
<img width="1870" height="951" alt="Sepet 3" src="https://github.com/user-attachments/assets/fc34838b-8c3a-493f-b718-4fe710e44411" />
<img width="1869" height="954" alt="Sepet 4" src="https://github.com/user-attachments/assets/373fed01-195e-4a28-9574-c61bf34bef2e" />
<img width="1863" height="955" alt="Sepet 5" src="https://github.com/user-attachments/assets/bdccb3c6-fb47-42f9-b151-34d33314bda8" />

---

#### Siparişlerim
> Her sipariş; 4 adımlı takip çubuğu (Sipariş Alındı → Onaylandı → Kargoya Verildi → Teslim Edildi), ürün görselleri ve teslimat adresiyle görüntülenir.

<img width="1860" height="957" alt="Siparişler" src="https://github.com/user-attachments/assets/cfebb72a-5e71-4e6b-a45e-4c96dcd733ea" />

---

#### Profil
> İki sekmeli yapı: **Hesap Bilgileri** (ad soyad, telefon, cinsiyet, şehir) ve **Şifre** (mevcut şifre doğrulamalı değiştirme).

<img width="1866" height="956" alt="Profil 1" src="https://github.com/user-attachments/assets/7d7a38f4-31ef-4bde-a9c0-6f59d70e8df3" />
<img width="1864" height="939" alt="Profil 2" src="https://github.com/user-attachments/assets/3b443725-0236-483d-a7b5-7b53dceaa044" />

---

### Admin Paneli

#### Admin Girişi
> Yalnızca `Role = Admin` kullanıcılar erişebilir. JWT decode edilir, rol kontrolü yapılır. `OnActionExecuting` her istek için doğrulama yapar.

<img width="1867" height="952" alt="Admin Giriş" src="https://github.com/user-attachments/assets/ba1671a3-c038-4f54-a1af-c830fefc0442" />

---

#### Dashboard
> Toplam sipariş, gelir, ürün ve kullanıcı istatistikleri. Chart.js doughnut kategori dağılımı. En çok sipariş edilen ürünler ve kritik stok uyarıları.

<img width="1869" height="953" alt="Dashboard 1" src="https://github.com/user-attachments/assets/58a9bca6-d21b-45bb-a0d6-ce803317a69d" />
<img width="1603" height="499" alt="Dashboard 2" src="https://github.com/user-attachments/assets/274b3d45-2287-47c6-9771-42844a097af7" />

---

#### Ürün Yönetimi
> Kategori, marka ve stok durumuna göre filtreleme. Sayfalama (10 ürün/sayfa). Ürün silme işlemi ProductApi'ye DELETE isteği gönderir.

<img width="1867" height="953" alt="Ürün Yönetimi 1" src="https://github.com/user-attachments/assets/555b9362-d2bc-4c63-8971-271bfa4f0096" />
<img width="1871" height="953" alt="Ürün Yönetimi 2" src="https://github.com/user-attachments/assets/bd798789-9ad7-49f2-9963-7a17a027014d" />
<img width="1870" height="953" alt="Ürün Yönetimi 3" src="https://github.com/user-attachments/assets/09743b82-ab40-4502-80a8-c3d0974f4bb4" />

---

#### Sipariş Yönetimi
> Tüm siparişler tarih, tutar ve durum ile listelenir. Detay modalında sipariş kalemleri, ürün adları ve teslimat adresi görüntülenir.

<img width="1869" height="956" alt="Sipariş Yönetimi 1" src="https://github.com/user-attachments/assets/68699751-155e-486b-9d10-74cdcb89d03a" />
<img width="1866" height="955" alt="Sipariş Yönetimi 2" src="https://github.com/user-attachments/assets/234457d5-a9e6-4e80-b72c-17199d158303" />

---

#### Kargo & Fatura Yönetimi
> Kargo takip numaraları CargoWorker tarafından otomatik oluşturulur. Fatura detay modalında `window.print()` ile tarayıcıdan yazdırma desteklenir.

<img width="1869" height="954" alt="Kargo" src="https://github.com/user-attachments/assets/caafc90f-ceb6-4441-9673-9185e4e671dc" />
<img width="1585" height="737" alt="Fatura 1" src="https://github.com/user-attachments/assets/4586f5dd-d29e-4d65-b392-a52ff7ae2422" />
<img width="1566" height="401" alt="Fatura 2" src="https://github.com/user-attachments/assets/64756a42-014e-4066-a70e-64a26f8d772c" />
<img width="1543" height="360" alt="Fatura 3" src="https://github.com/user-attachments/assets/0c9bf35d-4d63-4152-9e95-a28149d40364" />

---

#### Kupon Yönetimi
> Kupon oluştururken kod, indirim tipi (yüzde/sabit), değer, minimum sipariş tutarı, kullanım limiti ve son tarih belirlenir.

<img width="1868" height="955" alt="Kuponlar" src="https://github.com/user-attachments/assets/321e3e22-bc4e-4162-9c01-fd08e14d148e" />

---

#### Kullanıcı Yönetimi
> Tüm kullanıcılar ad, e-posta, rol ve kayıt tarihi ile listelenir. Admin/Customer rol filtresi uygulanabilir.

<img width="1868" height="958" alt="Kullanıcılar" src="https://github.com/user-attachments/assets/1f300262-dfda-4a94-8d9f-13115b8fce94" />

---

#### Yorum Yönetimi
> Tüm yorumlar kullanıcı adı, ürün referansı, puan ve yorum metniyle listelenir. En çok yorum alan ve en yüksek puanlı ürünler analiz kartlarında gösterilir.

<img width="1869" height="955" alt="Yorumlar 1" src="https://github.com/user-attachments/assets/cbbddf9c-c376-4a1c-b36e-813da7c5cbbd" />
<img width="1864" height="956" alt="Yorumlar 2" src="https://github.com/user-attachments/assets/ca2c9d5b-785b-4592-ac89-8b7c3eb704e9" />

---

### Altyapı & DevOps

#### PostgreSQL
> Ana ilişkisel veritabanı. `Host: localhost · Port: 5433 · DB: SmartCommerce · User: admin`. Users, Products, Orders, Cargos, Invoices tabloları burada tutulur.

> 📷 _PostgreSQL ekran görüntüsünü buraya ekleyin_

---

#### Redis
> Ürün listesi cache (TTL: 5 dk) ve sepet yönetimi (`cart:{userId}`) için kullanılır. Port: `6381`.

> 📷 _Redis ekran görüntüsünü buraya ekleyin_

---

#### Elasticsearch & Kibana
> Ürünler PostgreSQL'e kaydedilirken aynı zamanda Elasticsearch'e indekslenir. Serilog ile tüm servis logları Elasticsearch'e yazılır, Kibana'dan izlenir. Port: `9204` (ES) · `5604` (Kibana).

> 📷 _Elasticsearch / Kibana ekran görüntüsünü buraya ekleyin_

---

#### Serilog — Merkezi Loglama
> Her servis ve worker loglarını hem konsola hem Elasticsearch'e yazar. Index formatı: `{servisadi}-logs-{yil.ay}`.

> 📷 _Serilog / Kibana log akışı ekran görüntüsünü buraya ekleyin_

---

#### RabbitMQ Management
> `OrderCreated` event'i Fanout exchange ile 5 kuyruğa düşer. Her worker kendi kuyruğunu bağımsız tüketir. Management UI: `http://localhost:15674`.

> 📷 _RabbitMQ Management UI ekran görüntüsünü buraya ekleyin_

---

#### Docker & Swagger
> Tüm altyapı `docker compose up -d` ile ayağa kalkar. Her API JWT destekli Swagger UI ile dökümante edilmiştir.

> 📷 _Docker Desktop ve Swagger UI ekran görüntüsünü buraya ekleyin_

---

## Kurulum

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Visual Studio 2022 veya JetBrains Rider

### 1. Repoyu Klonla

```bash
git clone https://github.com/BerkayGenceroglu/SmartCommerce.BerkayStore.git
cd SmartCommerce.BerkayStore
```

### 2. Docker Altyapısını Başlat

```bash
docker compose up -d
```

### 3. Veritabanı Migration

```bash
cd UserApi       && dotnet ef database update
cd ../ProductApi && dotnet ef database update
cd ../OrderApi   && dotnet ef database update
```

### 4. Seed Verisi

```sql
\i seed_products.sql
```

### 5. Elasticsearch Re-Index

```
POST https://localhost:7136/api/product/reindex
```

### 6. Admin Kullanıcısı

```sql
UPDATE "Users" SET "Role" = 1 WHERE "Email" = 'admin@berkaystore.com';
```

### 7. Projeleri Başlat

Visual Studio → **Multiple Startup Projects**:

```
✅ UserApi          ✅ ProductApi        ✅ OrderApi
✅ SmartCommerce.UI ✅ NotificationWorker ✅ InvoiceWorker
✅ CargoWorker      ✅ StockWorker        ✅ PaymentWorker
```

### Erişim Adresleri

| Servis | URL |
|--------|-----|
| Uygulama | https://localhost:7050 |
| Admin Paneli | https://localhost:7050/Admin/Login |
| UserApi Swagger | https://localhost:7038/swagger |
| ProductApi Swagger | https://localhost:7136/swagger |
| OrderApi Swagger | https://localhost:7124/swagger |
| RabbitMQ Management | http://localhost:15674 |
| Kibana | http://localhost:5604 |

---

## Geliştirici

<div align="center">

**Berkay Gençeroğlu**

[![GitHub](https://img.shields.io/badge/GitHub-BerkayGenceroglu-181717?style=flat-square&logo=github)](https://github.com/BerkayGenceroglu)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Berkay%20Gençeroğlu-0A66C2?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/berkay-gencero%C4%9Flu-586b52331/)
[![Email](https://img.shields.io/badge/Email-berkaygenceroglu6@gmail.com-EA4335?style=flat-square&logo=gmail)](mailto:berkaygenceroglu6@gmail.com)

*Bu proje portfolyo amaçlı geliştirilmiştir. Gerçek bir ticari işletme değildir.*

</div>
