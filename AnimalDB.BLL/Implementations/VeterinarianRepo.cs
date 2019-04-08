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
    public class VeterinarianRepo : IVeterinarian
    {
        private readonly AnimalDBContext db;

        public VeterinarianRepo()
        {
            this.db = new AnimalDBContext();
        }

        public VeterinarianRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateVeterinarian(Veterinarian veterinarian)
        {
            var usermanager = new UserManager<Veterinarian>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Veterinarian>(db));
            var result = await usermanager.CreateAsync(veterinarian, "Password not required");
            if (result.Succeeded)
            {
                usermanager.AddToRole(veterinarian.Id, "Veterinarian");
            }
        }

        public Veterinarian GetVeterinarianByUsername(string username)
        {
            return db.Veterinarians.SingleOrDefault(m => m.UserName == username);
        }

        public async Task DeleteVeterinarian(Veterinarian veterinarian)
        {
            var usermanager = new UserManager<Veterinarian>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Veterinarian>(db));
            usermanager.RemoveFromRole(veterinarian.Id, "Veterinarian");
            await usermanager.DeleteAsync(veterinarian);
        }

        public async Task<Veterinarian> GetVeterinarianById(string id)
        {
            return await db.Veterinarians.FindAsync(id);
        }

        public IEnumerable<Veterinarian> GetVeterinarians()
        {
            return db.Veterinarians.ToList();
        }

        public async Task UpdateVeterinarian(Veterinarian veterinarian)
        {
            db.Entry(veterinarian).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Veterinarian user = GetVeterinarianByUsername(userName);
            var AdminManager = new UserManager<Veterinarian>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Veterinarian>(db));
            var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
        }
    }
}
