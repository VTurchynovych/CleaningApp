using CleaningApp.Data;
using CleaningApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningApp.Data.Services
{
    // Serwis do zarządzania Usługami 
    public class ServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Pobierz wszystkie usługi
        public async Task<List<Service>> GetServicesAsync()
        {
            return await _context.Services.AsNoTracking().ToListAsync();
        }

        // Dodaj nową usługę
        public async Task AddServiceAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
        }

        // Aktualizuj usługę
        public async Task UpdateServiceAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        // Usuń usługę
        public async Task DeleteServiceAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}