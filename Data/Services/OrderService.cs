using CleaningApp.Data;
using CleaningApp.Models;
using Microsoft.EntityFrameworkCore;

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

        // --- Metody pomocnicze ---

        // (W przyszłości dodamy tu metody do pobierania 
        // zleceń tylko dla Pracownika lub tylko dla Klienta)
    }
}