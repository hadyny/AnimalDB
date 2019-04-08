using AnimalDBCore.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AnimalDBCore.Core.Interfaces;

namespace AnimalDBCore.Infrastructure.Data
{
    public class VeterinarianRepo : IVeterinarian<Veterinarian>, IDisposable
    {
        private readonly AnimalDBContext _db;
        private readonly SignInManager<Veterinarian> _signInManager;
        private readonly UserManager<Veterinarian> _userManager;

        public VeterinarianRepo(AnimalDBContext db, SignInManager<Veterinarian> signinManager, UserManager<Veterinarian> userManager)
        {
            _db = db;
            _signInManager = signinManager;
            _userManager = userManager;
        }

        public async Task CreateVeterinarian(Veterinarian veterinarian)
        {
            var result = await _userManager.CreateAsync(veterinarian, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            await _userManager.AddToRoleAsync(veterinarian, "Veterinarian");
        }

        public Veterinarian GetVeterinarianByUsername(string username)
        {
            return _db.Veterinarians.SingleOrDefault(m => m.UserName == username);
        }

        public async Task DeleteVeterinarian(Veterinarian veterinarian)
        {
            await _userManager.RemoveFromRoleAsync(veterinarian, "Veterinarian");
            await _userManager.DeleteAsync(veterinarian);
            await _db.SaveChangesAsync();
        }

        public async Task<Veterinarian> GetVeterinarianById(string id)
        {
            return await _db.Veterinarians.FindAsync(id);
        }

        public IEnumerable<Veterinarian> GetVeterinarians()
        {
            return _db.Veterinarians.ToList();
        }

        public async Task UpdateVeterinarian(Veterinarian veterinarian)
        {
            _db.Entry(veterinarian).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Veterinarian user = GetVeterinarianByUsername(userName);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}
