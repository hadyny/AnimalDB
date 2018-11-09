using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;

namespace AnimalDB.Repo.Contexts
{
    public interface IAnimalDBContext
    {
        DbSet<Administrator> Administrators { get; set; }
        DbSet<AnimalManipulationReport> AnimalManipulationReports { get; set; }
        DbSet<AnimalRoomCount> AnimalRoomCounts { get; set; }
        DbSet<Animal> Animals { get; set; }
        DbSet<ApprovalNumber> ApprovalNumbers { get; set; }
        DbSet<ArrivalStatus> ArrivalStatus { get; set; }
        DbSet<CageLocationHistory> CageLocationHistories { get; set; }
        DbSet<CageLocation> CageLocations { get; set; }
        DbSet<ChargeCode> ChargeCode { get; set; }
        DbSet<CleaningTask> CleaningTasks { get; set; }
        DbSet<ClinicalIncidentReport> ClinicalIncidentReports { get; set; }
        DbSet<Colour> Colours { get; set; }
        DbSet<CulledPups> CulledPups { get; set; }
        DbSet<EthicsDocument> EthicsDocuments { get; set; }
        DbSet<EthicsNumberHistory> EthicsNumberHistories { get; set; }
        DbSet<EthicsNumber> EthicsNumbers { get; set; }
        DbSet<FeedingGroup> FeedingGroups { get; set; }
        DbSet<Feed> Feeds { get; set; }
        DbSet<GDTimeline> GDTimelines { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<Investigator> Investigators { get; set; }
        DbSet<MedicationFollowUp> MedicationFollowUps { get; set; }
        DbSet<Medication> Medications { get; set; }
        DbSet<MedicationType> MedicationTypes { get; set; }
        DbSet<NotCheckedAnimal> NotCheckedAnimals { get; set; }
        DbSet<NotCheckedRoom> NotCheckedRooms { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<NotificationEmail> NotificationEmails { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<RackEntry> RackEntries { get; set; }
        DbSet<Rack> Racks { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<RosterNote> RosterNotes { get; set; }
        DbSet<Roster> Rosters { get; set; }
        DbSet<SopCategory> SopCategories { get; set; }
        DbSet<Sop> Sops { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<Species> Species { get; set; }
        DbSet<Strain> Strains { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<SurgeryType> SurgeryTypes { get; set; }
        DbSet<SurgicalNote> SurgicalNotes { get; set; }
        DbSet<SurgicalWelfareScore> SurgicalWelfareScores { get; set; }
        DbSet<Technician> Technicians { get; set; }
        DbSet<Transgene> Transgenes { get; set; }
        DbSet<Veterinarian> Veterinarians { get; set; }
        DbSet<VetSchedule> VetSchedules { get; set; }
        DbSet<VirusType> VirusTypes { get; set; }


        DbEntityEntry Entry(object entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}