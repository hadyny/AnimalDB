using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using Microsoft.Owin.Security;

namespace AnimalDB.Repo.Implementations
{
    public class TechnicianRepo : ITechnician, IDisposable
    {
        private AnimalDBContext db;

        public TechnicianRepo()
        {
            this.db = new AnimalDBContext();
        }

        public Technician GetTechnicianByUsername(string username)
        {
            return db.Technicians.SingleOrDefault(m => m.UserName == username);
        }

        public TechnicianRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateTechnician(Technician technician)
        {
            var usermanager = new UserManager<Technician>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Technician>(db));
            var result = await usermanager.CreateAsync(technician, "Password not required");
            usermanager.AddToRole(technician.Id, "Technician");
        }

        public async Task DeleteTechnician(Technician technician)
        {
            var usermanager = new UserManager<Technician>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Technician>(db));
            usermanager.RemoveFromRole(technician.Id, "Technician");
            await usermanager.DeleteAsync(technician);
        }

        public async Task<Technician> GetTechnicianById(string id)
        {
            return await db.Technicians.FindAsync(id);
        }

        public IEnumerable<Technician> GetTechnicians()
        {
            return db.Technicians.ToList();
        }

        public async Task UpdateTechnician(Technician technician)
        {
            db.Entry(technician).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Technician user = GetTechnicianByUsername(userName);
            var AdminManager = new UserManager<Technician>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Technician>(db));
            var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
