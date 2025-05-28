using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services.Interfaces
{
    public interface IAllergenService
    {
        Task<List<Allergen>> GetAllAllergensAsync();
    }
} 