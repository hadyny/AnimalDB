using AnimalDBCore.Core.Entities;
using AnimalDBCore.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalDBCore.Infrastructure.Contexts
{
    public class AnimalDBContext : IdentityDbContext<AnimalUser>
    {
        public AnimalDBContext(DbContextOptions<AnimalDBContext> options) 
            :base(options) { }

        public AnimalDBContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasOne(m => m.Animal)
                .WithMany(m => m.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CageLocation>()
                .HasOne(m => m.Room)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasOne(m => m.FeedingGroup)
                .WithMany(m => m.Groups)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rack>()
                .HasMany(m => m.RackEntries)
                .WithOne(m => m.Rack)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Roster>()
                .HasOne(m => m.Room)
                .WithMany(m => m.Roster)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurgicalWelfareScore>()
                .HasOne(m => m.SurgicalNote)
                .WithMany(m => m.WellfareScores)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Animal>()
                .HasMany(m => m.Offspring)
                .WithOne(m => m.Parent);

            modelBuilder.Entity<Animal>()
                .HasMany(m => m.Parents)
                .WithOne(m => m.Child);
        }

        public DbSet<Administrator> Administrators { get; set; }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<ParentChildRelationship> ParentChildRelationships { get; set; }

        public DbSet<ArrivalStatus> ArrivalStatus { get; set; }

        public DbSet<Colour> Colours { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Strain> Strains { get; set; }

        public DbSet<SurgeryType> SurgeryTypes { get; set; }

        public DbSet<EthicsNumber> EthicsNumbers { get; set; }

        public DbSet<EthicsNumberHistory> EthicsNumberHistories { get; set; }

        public DbSet<MedicationType> MedicationTypes { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Species> Species { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<FeedingGroup> FeedingGroups { get; set; }

        public DbSet<Feed> Feeds { get; set; }

        public DbSet<SurgicalNote> SurgicalNotes { get; set; }

        public DbSet<CleaningTask> CleaningTasks { get; set; }

        public DbSet<CageLocation> CageLocations { get; set; }

        public DbSet<CageLocationHistory> CageLocationHistories { get; set; }

        public DbSet<Investigator> Investigators { get; set; }

        public DbSet<Technician> Technicians { get; set; }

        public DbSet<Veterinarian> Veterinarians { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<ClinicalIncidentReport> ClinicalIncidentReports { get; set; }

        public DbSet<AnimalManipulationReport> AnimalManipulationReports { get; set; }

        public DbSet<NotificationEmail> NotificationEmails { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ChargeCode> ChargeCode { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<SurgicalWelfareScore> SurgicalWelfareScores { get; set; }

        public DbSet<ApprovalNumber> ApprovalNumbers { get; set; }

        public DbSet<CulledPups> CulledPups { get; set; }

        public DbSet<VirusType> VirusTypes { get; set; }

        public DbSet<MedicationFollowUp> MedicationFollowUps { get; set; }

        public DbSet<Transgene> Transgenes { get; set; }

        public DbSet<Rack> Racks { get; set; }

        public DbSet<RackEntry> RackEntries { get; set; }

        public DbSet<Sop> Sops { get; set; }

        public DbSet<SopCategory> SopCategories { get; set; }

        public DbSet<Roster> Rosters { get; set; }

        public DbSet<RosterNote> RosterNotes { get; set; }

        public DbSet<EthicsDocument> EthicsDocuments { get; set; }

        public DbSet<VetSchedule> VetSchedules { get; set; }

        public DbSet<GDTimeline> GDTimelines { get; set; }

        public DbSet<AnimalRoomCount> AnimalRoomCounts { get; set; }

        public DbSet<NotCheckedAnimal> NotCheckedAnimals { get; set; }

        public DbSet<NotCheckedRoom> NotCheckedRooms { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentCategory> DocumentCategories { get; set; }
    }
}