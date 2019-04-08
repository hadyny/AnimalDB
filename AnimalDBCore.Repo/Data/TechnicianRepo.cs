using AnimalDBCore.Infrastructure.Contexts;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AnimalDBCore.Core.Interfaces;

namespace AnimalDBCore.Infrastructure.Data
{
    public class TechnicianRepo : ITechnician<Technician>, IDisposable
    {
        private readonly AnimalDBContext _db;
        private readonly SignInManager<Technician> _signInManager;
        private readonly UserManager<Technician> _userManager;

        public TechnicianRepo(AnimalDBContext db, SignInManager<Technician> signinManager, UserManager<Technician> userManager)
        {
            _db = db;
            _signInManager = signinManager;
            _userManager = userManager;
        }

        public Technician GetTechnicianByUsername(string username)
        {
            return _db.Technicians.SingleOrDefault(m => m.UserName == username);
        }

        public async Task CreateTechnician(Technician technician)
        {
            var result = await _userManager.CreateAsync(technician, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            await _userManager.AddToRoleAsync(technician, "Technician");
        }

        public async Task DeleteTechnician(Technician technician)
        {
            await _userManager.RemoveFromRoleAsync(technician, "Technician");
            await _userManager.DeleteAsync(technician);
            await _db.SaveChangesAsync();
        }

        public async Task<Technician> GetTechnicianById(string id)
        {
            return await _db.Technicians.FindAsync(id);
        }

        public IEnumerable<Technician> GetTechnicians()
        {
            return _db.Technicians.ToList();
        }

        public async Task UpdateTechnician(Technician technician)
        {
            _db.Entry(technician).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Technician user = GetTechnicianByUsername(userName);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}
