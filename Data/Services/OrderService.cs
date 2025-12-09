using CleaningApp.Data;
using CleaningApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CleaningApp.Data.Services
{
    public class OrderService
    {
        //(DbContext)
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Metody READ

        // Metoda do pobrania wszystkich zleceń

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)     // Załaduj dane Klienta
                .Include(o => o.Service)    // Załaduj dane Usługi
                .Include(o => o.Worker)     // Załaduj dane Pracownika 
                .AsNoTracking() // "tylko do odczytu"
                .ToListAsync();
        }

        // Metoda do pobrania jednego zlecenia 
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // --- Metody CREATE, UPDATE, DELETE

        // Metoda do dodawania nowego zlecenia
        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        // Metoda do aktualizacji istniejącego zlecenia
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // Metoda do usuwania zlecenia
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        // Dedykowana metoda do szybkiej zmiany statusu
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        // --- Metody pomocnicze --


        // Metoda do oceniania zlecenia przez klienta
        public async Task RateOrderAsync(int orderId, int rating, string comment)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Rating = rating;
                order.ClientComment = comment;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        // Metoda do pobierania zleceń konkretnego użytkownika 
        public async Task<List<Order>> GetUserOrdersAsync(string userName)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Worker)
                .Where(o => o.Client != null && o.Client.UserName == userName)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();
        }
        // Szuka po ID, a nie po nazwie
        public async Task<List<Order>> GetClientOrdersAsync(string clientId)
        {
            return await _context.Orders
                .Include(o => o.Service)
                .Include(o => o.Worker)
                .Include(o => o.Client)
                .Where(o => o.ClientId == clientId) 
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Metoda do pobierania najlepszych opinii na stronę główną
        public async Task<List<Order>> GetTopReviewsAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Where(o => o.Rating == 5 && !string.IsNullOrEmpty(o.ClientComment))
                .OrderByDescending(o => o.OrderDate)
                .Take(3)
                .AsNoTracking()
                .ToListAsync();
        }

    public async Task<List<Order>> GetWorkerOrdersAsync(string workerId)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Where(o => o.WorkerId == workerId)
                .OrderBy(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<OrderStats> GetStatisticsAsync()
        {
            var stats = new OrderStats();

            // 1. Liczba oczekujących 
            stats.PendingOrders = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Oczekujące);

            // 2. Liczba zakończonych
            stats.CompletedOrders = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Zakończone);

            // 3. Przychód 
            stats.TotalRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Zakończone)
                .SumAsync(o => o.Service.DefaultPrice ?? 0);

            // 4. Liczba unikalnych klientów
            stats.TotalClients = await _context.Orders
                .Select(o => o.ClientId)
                .Distinct()
                .CountAsync();

            return stats;
        }
        //EKSPORT CSV
        public async Task<byte[]> ExportOrdersToCsvAsync()
        {
            var orders = await GetOrdersAsync();
            var builder = new StringBuilder();
            builder.AppendLine("Id;Data;Klient;Email Klienta;Adres;Usluga;Pracownik;Status;Ocena");

            foreach (var order in orders)
            {
                string clientName = order.Client?.FullName ?? "Brak danych";
                string clientEmail = order.Client?.Email ?? "-";
                string serviceName = order.Service?.Name ?? "Brak usługi";
                string workerName = order.Worker?.FullName ?? "Nieprzypisany";
                string date = order.OrderDate.ToString("yyyy-MM-dd");
                string rating = order.Rating.HasValue ? order.Rating.ToString() : "";

                builder.AppendLine($"{order.Id};{date};{clientName};{clientEmail};{order.Address};{serviceName};{workerName};{order.Status};{rating}");
            }
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
        // Metoda wyszukiwania dla Admina
        public async Task<List<Order>> SearchOrdersAsync(string searchText, OrderStatus? statusFilter)
        {
            var query = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Worker)
                .AsQueryable();

            // 1. Filtrowanie po tekście 
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(o =>
                    o.Id.ToString().Contains(searchText) ||
                    o.Address.ToLower().Contains(searchText) ||
                    (o.Client != null && o.Client.FullName.ToLower().Contains(searchText)) ||
                    (o.Client != null && o.Client.Email.ToLower().Contains(searchText))
                );
            }

            // 2. Filtrowanie po statusie 
            if (statusFilter.HasValue)
            {
                query = query.Where(o => o.Status == statusFilter.Value);
            }

            //najnowsze na górze
            return await query.OrderByDescending(o => o.OrderDate).AsNoTracking().ToListAsync();
        }
    }
    public class OrderStats
    {
        public int PendingOrders { get; set; }   
        public int CompletedOrders { get; set; } 
        public decimal TotalRevenue { get; set; }
        public int TotalClients { get; set; }    
    }



}