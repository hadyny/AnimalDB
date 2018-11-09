namespace AnimalDB.Migrations
{
    using AnimalDB.Repo.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AnimalDB.Repo.Contexts.AnimalDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AnimalDB.Repo.Contexts.AnimalDBContext context)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityResult roleresult;
            if (!RoleManager.RoleExists("Administrator"))
            {
                roleresult = RoleManager.Create(new IdentityRole("Administrator"));
            }
            if (!RoleManager.RoleExists("Technician"))
            {
                roleresult = RoleManager.Create(new IdentityRole("Technician"));
            }
            if (!RoleManager.RoleExists("Veterinarian"))
            {
                roleresult = RoleManager.Create(new IdentityRole("Veterinarian"));
            }
            if (!RoleManager.RoleExists("Investigator"))
            {
                roleresult = RoleManager.Create(new IdentityRole("Investigator"));
            }
            if (!RoleManager.RoleExists("Student"))
            {
                roleresult = RoleManager.Create(new IdentityRole("Student"));
            }

            var administrator = new Administrator()
            {
                UserName = "hadyn",
                FirstName = "Hadyn",
                LastName = "Youens",
                Email = "hadyn@psy.otago.ac.nz"
            };

            var usermanager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(context));
            
            if (usermanager.FindByEmail("hadyn@psy.otago.ac.nz") == null)
            {
                var result = usermanager.Create(administrator, "Password not required");
                usermanager.AddToRole(administrator.Id, "Administrator");
            }
        }
    }
}
