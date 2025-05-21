using Restaurant.Models;
using Restaurant.Services.Interfaces;
using Restaurant.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class AllergenService : IAllergenService
    {
        private readonly RestaurantDbContext _context;

        public AllergenService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Allergen>> GetAllAllergensAsync()
        {
            return await _context.Allergens.ToListAsync();
        }
    }
} 