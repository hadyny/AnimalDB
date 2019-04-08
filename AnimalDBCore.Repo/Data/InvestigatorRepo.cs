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
    public class InvestigatorRepo : IInvestigator<Investigator>, IDisposable
    {
        private readonly AnimalDBContext _db;
        private readonly SignInManager<Investigator> _signInManager;
        private readonly UserManager<Investigator> _userManager;

        public InvestigatorRepo(AnimalDBContext db, SignInManager<Investigator> signinManager, UserManager<Investigator> userManager)
        {
            _db = db;
            _signInManager = signinManager;
            _userManager = userManager;
        }

        public async Task CreateInvestigator(Investigator investigator)
        {
            var result = await _userManager.CreateAsync(investigator, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            await _userManager.AddToRoleAsync(investigator, "Investigator");
        }

        public Investigator GetInvestigatorByUsername(string username)
        {
            return _db.Investigators.SingleOrDefault(m => m.UserName == username);
        }

        public async Task DeleteInvestigator(Investigator investigator)
        {
            await _userManager.RemoveFromRoleAsync(investigator, "Investigator");
            await _userManager.DeleteAsync(investigator);
            await _db.SaveChangesAsync();
        }

        public async Task<Investigator> GetInvestigatorById(string id)
        {
            return await _db.Investigators.FindAsync(id);
        }

        public IEnumerable<Investigator> GetInvestigators()
        {
            return _db.Investigators.ToList();
        }

        public async Task UpdateInvestigator(Investigator investigator)
        {
            _db.Entry(investigator).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Investigator user = GetInvestigatorByUsername(userName);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}
