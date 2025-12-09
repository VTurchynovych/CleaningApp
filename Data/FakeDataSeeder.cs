using CleaningApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleaningApp.Data
{
    public static class FakeDataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Sprawdź, czy mamy już dużo danych. Jeśli tak, nie rób nic (żeby nie dublować).
            if (await context.Orders.CountAsync() > 10) return;

            // Upewnij się, że są jakiekolwiek usługi (jeśli nie, dodaj podstawowe)
            var services = await context.Services.ToListAsync();
            if (!services.Any())
            {
                services = new List<Service>
                {
                    new Service { Name = "Sprzątanie Standard", DefaultPrice = 150, ImageUrl = "/img/services/default.jpg" },
                    new Service { Name = "Mycie Okien", DefaultPrice = 200, ImageUrl = "/img/services/default.jpg" },
                    new Service { Name = "Pranie Dywanów", DefaultPrice = 300, ImageUrl = "/img/services/default.jpg" }
                };
                await context.Services.AddRangeAsync(services);
                await context.SaveChangesAsync();
            }

            // 2. Generowanie 30 UŻYTKOWNIKÓW (Klientów)
            var createdUsers = new List<ApplicationUser>();

            for (int i = 1; i <= 30; i++)
            {
                var email = $"klient{i}@test.pl";

                // Sprawdź czy już istnieje
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = $"Klient Testowy {i}",
                        PhoneNumber = $"500-000-0{i:00}", // Np. 500-000-001
                        EmailConfirmed = true
                    };

                    // Hasło dla wszystkich: User123!
                    var result = await userManager.CreateAsync(user, "User123!");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        createdUsers.Add(user);
                    }
                }
            }

            // Jeśli nie udało się stworzyć userów (np. błąd bazy), przerwij
            if (!createdUsers.Any()) return;

            // 3. Generowanie 25 ZLECEŃ
            var random = new Random();
            var orders = new List<Order>();

            for (int i = 1; i <= 25; i++)
            {
                // Losujemy klienta i usługę
                var randomClient = createdUsers[random.Next(createdUsers.Count)];
                var randomService = services[random.Next(services.Count)];

                // Losujemy datę (od -10 dni do +10 dni od dziś)
                var randomDate = DateTime.Now.AddDays(random.Next(-10, 10));

                // Losujemy status
                // 0=Oczekujące, 1=WTrakcie, 2=Zakończone, 3=Anulowane
                var randomStatus = (OrderStatus)random.Next(0, 4);

                // Jeśli Zakończone, dodajemy czasem ocenę
                int? rating = null;
                string comment = null;

                if (randomStatus == OrderStatus.Zakończone)
                {
                    rating = random.Next(3, 6); // Ocena 3, 4 lub 5
                    comment = rating == 5 ? "Super robota, polecam!" : "W porządku, ale spóźnienie.";
                }

                orders.Add(new Order
                {
                    ClientId = randomClient.Id,
                    ServiceId = randomService.Id,
                    Address = $"ul. Losowa {random.Next(1, 100)}/{random.Next(1, 50)}, Warszawa",
                    OrderDate = randomDate,
                    Status = randomStatus,
                    ClientNote = random.Next(0, 2) == 1 ? "Proszę uważać na psa." : null, // Co drugie zlecenie ma notatkę
                    InternalNotes = null,
                    Rating = rating,
                    ClientComment = comment
                });
            }

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
    }
}