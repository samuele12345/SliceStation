# 📋 TODO - Applicazione Pizzeria

## ✅ Completato
- [x] Creati modelli: ApplicationUser, Order, Pizza
- [x] Configurato DbContext (TestJQueryContext)
- [x] Convertito progetto da Razor Pages a MVC
- [x] Creata struttura base MVC (Controllers, Views, ViewModels)

---

## ⚠️ DA FARE - IMPORTANTE!

### 1. **Aggiornare DbContext** (`Data/TestJQueryContext.cs`)
Aggiungere i DbSet mancanti:
```csharp
public DbSet<Order> Orders { get; set; }
public DbSet<Pizza> Pizzas { get; set; }
```

### 2. **Configurare Identity e Database** (`Program.cs`)
- Aggiungere servizi Identity
- Configurare connessione database
- Aggiungere middleware Authentication

### 3. **Creare ViewModels** (cartella `ViewModels/`)
- [ ] `RegisterViewModel.cs` - per registrazione utenti
- [ ] `LoginViewModel.cs` - per login
- [ ] `CreateOrderViewModel.cs` - per creare ordini
- [ ] `OrderDetailsViewModel.cs` - per visualizzare dettagli ordine

### 4. **Creare Controllers** (cartella `Controllers/`)
- [ ] `AccountController.cs` - gestione autenticazione (Register, Login, Logout)
- [ ] `OrdersController.cs` - gestione ordini (Index, Create, Details, Delete)
- [ ] `PizzasController.cs` - catalogo pizze (Index, eventualmente CRUD per admin)

### 5. **Creare Views corrispondenti**
- [ ] `Views/Account/` - Register.cshtml, Login.cshtml
- [ ] `Views/Orders/` - Index.cshtml, Create.cshtml, Details.cshtml
- [ ] `Views/Pizzas/` - Index.cshtml (catalogo)

### 6. **Database Migration**
Eseguire in Package Manager Console:
```powershell
Add-Migration InitialCreate
Update-Database
```

---

## 📝 Note Architettura

**Relazioni:**
- `ApplicationUser` (1) → (N) `Order`
- `Order` (1) → (N) `Pizza`

**Workflow utente tipico:**
1. Login/Register
2. Visualizza catalogo pizze
3. Crea ordine selezionando pizze
4. Visualizza i propri ordini
5. Vede dettaglio singolo ordine

---

## 🔗 Riferimenti utili
- [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [MVC in ASP.NET Core](https://learn.microsoft.com/aspnet/core/mvc/overview)
