namespace AnimalDB.Migrations
{
    using AnimalDB.Repo.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Repo.Contexts.AnimalDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Repo.Contexts.AnimalDBContext context)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            if (!RoleManager.RoleExists("Administrator"))
            {
                RoleManager.Create(new IdentityRole("Administrator"));
            }
            if (!RoleManager.RoleExists("Technician"))
            {
                RoleManager.Create(new IdentityRole("Technician"));
            }
            if (!RoleManager.RoleExists("Veterinarian"))
            {
                RoleManager.Create(new IdentityRole("Veterinarian"));
            }
            if (!RoleManager.RoleExists("Investigator"))
            {
                RoleManager.Create(new IdentityRole("Investigator"));
            }
            if (!RoleManager.RoleExists("Student"))
            {
                RoleManager.Create(new IdentityRole("Student"));
            }

            var administrator = new Administrator()
            {
                UserName = "hadyn",
                FirstName = "Hadyn",
                LastName = "Youens",
                Email = "hadyn@psy.otago.ac.nz"
            };

            var usermanager = new UserManager<Administrator>(new UserStore<Administrator>(context));
            
            if (usermanager.FindByEmail("hadyn@psy.otago.ac.nz") == null)
            {
                usermanager.Create(administrator, "Password not required");
                usermanager.AddToRole(administrator.Id, "Administrator");
            }
        }
    }
}
