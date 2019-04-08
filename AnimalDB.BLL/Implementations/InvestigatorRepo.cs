using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Implementations
{
    public class InvestigatorRepo : IInvestigator
    {
        private readonly AnimalDBContext db;

        public InvestigatorRepo()
        {
            this.db = new AnimalDBContext();
        }

        public InvestigatorRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateInvestigator(Investigator investigator)
        {
            var usermanager = new UserManager<Investigator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Investigator>(db));
            var result = await usermanager.CreateAsync(investigator, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            usermanager.AddToRole(investigator.Id, "Investigator");
        }

        public Investigator GetInvestigatorByUsername(string username)
        {
            return db.Investigators.SingleOrDefault(m => m.UserName == username);
        }

        public async Task DeleteInvestigator(Investigator investigator)
        {
            var usermanager = new UserManager<Investigator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Investigator>(db));
            usermanager.RemoveFromRole(investigator.Id, "Investigator");
            await usermanager.DeleteAsync(investigator);
        }

        public async Task<Investigator> GetInvestigatorById(string id)
        {
            return await db.Investigators.FindAsync(id);
        }

        public IEnumerable<Investigator> GetInvestigators()
        {
            return db.Investigators.ToList();
        }

        public async Task UpdateInvestigator(Investigator investigator)
        {
            db.Entry(investigator).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Investigator user = GetInvestigatorByUsername(userName);
            var AdminManager = new UserManager<Investigator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Investigator>(db));
            var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
        }
    }
}
