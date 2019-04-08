using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalDBCore.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnimalDBCore.Core.Interfaces;

namespace AnimalDBCore.Infrastructure.Data
{
    public class AdministratorRepo : IAdministrator<Administrator>, IDisposable
    {
        private readonly AnimalDBContext _db;
        private readonly SignInManager<Administrator> _signInManager;
        private readonly UserManager<Administrator> _userManager;

        public AdministratorRepo(AnimalDBContext db, SignInManager<Administrator> signinManager, UserManager<Administrator> userManager)
        {
            _db = db;
            _signInManager = signinManager;
            _userManager = userManager;
        }

        public async Task CreateAdministrator(Administrator administrator)
        {
            var result = await _userManager.CreateAsync(administrator, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            await _userManager.AddToRoleAsync(administrator, "Administrator");
        }

        public Administrator GetAdministratorByUsername(string userName)
        {
            return _db.Administrators.SingleOrDefault(m => m.UserName == userName);
        }

        public async Task DeleteAdministrator(Administrator administrator)
        {
            if (await _userManager.IsInRoleAsync(administrator, "Investigator"))
            {
                await _userManager.RemoveFromRoleAsync(administrator, "Investigator");
            }
            await _userManager.RemoveFromRoleAsync(administrator, "Administrator");
            await _userManager.DeleteAsync(administrator);
            await _db.SaveChangesAsync();
        }

        public async Task<Administrator> GetAdministratorById(string id)
        {
            return await _db.Administrators.FindAsync(id);
        }

        public IEnumerable<Administrator> GetAdministrators()
        {
            return _db.Administrators;
        }

        public async Task UpdateAdministrator(Administrator administrator)
        {
            _db.Entry(administrator).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Administrator user = GetAdministratorByUsername(userName);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}
