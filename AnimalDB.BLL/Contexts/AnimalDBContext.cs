using AnimalDB.Repo.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnimalDB.Repo.Contexts
{
    public class AnimalDBContext : IdentityDbContext<AnimalUser>, IAnimalDBContext
    {
        public AnimalDBContext() : base("DefaultConnection", throwIfV1Schema: false) { }

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

            modelBuilder.Entity<Group>()
                .HasRequired(m => m.FeedingGroup)
                .WithMany(m => m.Groups)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Rack>()
                .HasMany(m => m.RackEntries)
                .WithRequired(m => m.Rack)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roster>()
                .HasRequired(m => m.Room)
                .WithMany(m => m.Roster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SurgicalWelfareScore>()
                .HasRequired(m => m.SurgicalNote)
                .WithMany(m => m.WellfareScores)
                .WillCascadeOnDelete(true);
        }

        public System.Data.Entity.DbSet<Administrator> Administrators { get; set; }

        public System.Data.Entity.DbSet<Animal> Animals { get; set; }

        public System.Data.Entity.DbSet<ArrivalStatus> ArrivalStatus { get; set; }

        public System.Data.Entity.DbSet<Colour> Colours { get; set; }

        public System.Data.Entity.DbSet<Group> Groups { get; set; }

        public System.Data.Entity.DbSet<Source> Sources { get; set; }

        public System.Data.Entity.DbSet<Strain> Strains { get; set; }

        public System.Data.Entity.DbSet<SurgeryType> SurgeryTypes { get; set; }

        public System.Data.Entity.DbSet<EthicsNumber> EthicsNumbers { get; set; }

        public System.Data.Entity.DbSet<EthicsNumberHistory> EthicsNumberHistories { get; set; }

        public System.Data.Entity.DbSet<MedicationType> MedicationTypes { get; set; }

        public System.Data.Entity.DbSet<Note> Notes { get; set; }

        public System.Data.Entity.DbSet<Species> Species { get; set; }

        public System.Data.Entity.DbSet<Medication> Medications { get; set; }

        public System.Data.Entity.DbSet<FeedingGroup> FeedingGroups { get; set; }

        public System.Data.Entity.DbSet<Feed> Feeds { get; set; }

        public System.Data.Entity.DbSet<SurgicalNote> SurgicalNotes { get; set; }

        public System.Data.Entity.DbSet<CleaningTask> CleaningTasks { get; set; }

        public System.Data.Entity.DbSet<CageLocation> CageLocations { get; set; }

        public System.Data.Entity.DbSet<CageLocationHistory> CageLocationHistories { get; set; }

        public System.Data.Entity.DbSet<Investigator> Investigators { get; set; }

        public System.Data.Entity.DbSet<Technician> Technicians { get; set; }

        public System.Data.Entity.DbSet<Veterinarian> Veterinarians { get; set; }

        public System.Data.Entity.DbSet<Student> Students { get; set; }

        public System.Data.Entity.DbSet<ClinicalIncidentReport> ClinicalIncidentReports { get; set; }

        public System.Data.Entity.DbSet<AnimalManipulationReport> AnimalManipulationReports { get; set; }

        public System.Data.Entity.DbSet<NotificationEmail> NotificationEmails { get; set; }

        public System.Data.Entity.DbSet<Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<ChargeCode> ChargeCode { get; set; }

        public System.Data.Entity.DbSet<Room> Rooms { get; set; }

        public System.Data.Entity.DbSet<SurgicalWelfareScore> SurgicalWelfareScores { get; set; }

        public System.Data.Entity.DbSet<ApprovalNumber> ApprovalNumbers { get; set; }

        public System.Data.Entity.DbSet<CulledPups> CulledPups { get; set; }

        public System.Data.Entity.DbSet<VirusType> VirusTypes { get; set; }

        public System.Data.Entity.DbSet<MedicationFollowUp> MedicationFollowUps { get; set; }

        public System.Data.Entity.DbSet<Transgene> Transgenes { get; set; }

        public System.Data.Entity.DbSet<Rack> Racks { get; set; }

        public System.Data.Entity.DbSet<RackEntry> RackEntries { get; set; }

        public System.Data.Entity.DbSet<Sop> Sops { get; set; }

        public System.Data.Entity.DbSet<SopCategory> SopCategories { get; set; }

        public System.Data.Entity.DbSet<Roster> Rosters { get; set; }

        public System.Data.Entity.DbSet<RosterNote> RosterNotes { get; set; }

        public System.Data.Entity.DbSet<EthicsDocument> EthicsDocuments { get; set; }

        public System.Data.Entity.DbSet<VetSchedule> VetSchedules { get; set; }

        public System.Data.Entity.DbSet<GDTimeline> GDTimelines { get; set; }

        public System.Data.Entity.DbSet<AnimalRoomCount> AnimalRoomCounts { get; set; }

        public System.Data.Entity.DbSet<NotCheckedAnimal> NotCheckedAnimals { get; set; }

        public System.Data.Entity.DbSet<NotCheckedRoom> NotCheckedRooms { get; set; }

        public static AnimalDBContext Create()
        {
            return new AnimalDBContext();
        }
    }
}