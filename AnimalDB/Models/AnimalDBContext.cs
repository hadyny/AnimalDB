using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AnimalDB.Models
{
    public class AnimalDBContext : DbContext
    {  
        public AnimalDBContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasRequired(m => m.Animal)
                .WithMany(m => m.Notifications)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CageLocation>()
                .HasRequired(m => m.Room)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SurgicalNote>()
                .HasMany(m => m.Surgeries)
                .WithMany()
                .Map(a =>
                    a.MapLeftKey("SurgicalNote_Id")
                    .MapRightKey("SurgeryType_Id")
                    .ToTable("SurgicalNoteSurgeries")
                );

            modelBuilder.Entity<Administrator>();
            modelBuilder.Entity<Technician>();
            modelBuilder.Entity<Veterinarian>();
            modelBuilder.Entity<Investigator>();
            modelBuilder.Entity<Student>();
            
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            
        }

        public System.Data.Entity.DbSet<AnimalDB.Models.Administrator> Admininstrators { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Animal> Animals { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.ArrivalStatus> ArrivalStatus { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Colour> Colours { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Source> Sources { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Strain> Strains { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.SurgeryType> SurgeryTypes { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.EthicsNumber> EthicsNumbers { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.EthicsNumberHistory> EthicsNumberHistories { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.MedicationType> MedicationTypes { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Note> Notes { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Species> Species { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Medication> Medications { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.FeedingGroup> FeedingGroups { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Feed> Feeds { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.SurgicalNote> SurgicalNotes { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.CleaningTask> CleaningTasks { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.CageLocation> CageLocations { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.CageLocationHistory> CageLocationHistories { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Investigator> Investigators { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Technician> Technicians { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Veterinarian> Veterinarians { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.ClinicalIncidentReport> ClinicalIncidentReports { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.AnimalManipulationReport> AnimalManipulationReports { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.NotificationEmail> NotificationEmails { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.ChargeCode> ChargeCode { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.Room> Rooms { get; set; }

        public System.Data.Entity.DbSet<AnimalDB.Models.SurgicalWelfareScore> SurgicalWelfareScores { get; set; }
    }

}
