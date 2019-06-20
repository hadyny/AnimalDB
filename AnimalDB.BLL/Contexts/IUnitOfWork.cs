using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Contexts
{
    public interface IUnitOfWork : IDisposable
    {
        AnimalDBContext Context { get; }

        IAdministratorRepository Administrators { get; }

        IAnimalManipulationReportRepository AnimalManipulationReports { get; }

        IAnimalRepository Animals { get; }

        IAnimalRoomCountRepository AnimalRoomCounts { get; }

        IApprovalNumberRepository ApprovalNumbers { get; }

        IArrivalStatusRepository ArrivalStatus { get; }

        ICageLocationHistoryRepository CageLocationHistories { get; }

        ICageLocationRepository CageLocations { get; }

        IChargeCodeRepository ChargeCodes { get; }

        IClinicalIncidentReportRepository ClinicalIncidentReports { get; }

        IColourRepository Colours { get; }

        ICulledPupsRepository CulledPups { get; }

        IDocumentCategoryRepository DocumentCategories { get; }

        IDocumentRepository Documents { get; }

        IEthicsDocumentRepository EthicsDocuments { get; }

        IEthicsNumberHistoryRepository EthicsNumberHistories { get; }

        IEthicsNumberRepository EthicsNumbers { get; }

        IFeedingGroupRepository FeedingGroups { get; }

        IFeedRepository Feeds { get; }

        IGDTimelineRepository GDTimelines { get; }

        IGroupRepository Groups { get; }

        IInvestigatorRepository Investigators { get; }

        IMedicationFollowUpRepository MedicationFollowUps { get; }

        IMedicationRepository Medication { get; }

        IMedicationTypeRepository MedicationTypes { get; }

        INotCheckedAnimalRepository NotCheckedAnimals { get; }

        INotCheckedRoomRepository NotCheckedRooms { get; }

        INoteRepository Notes { get; }

        INotificationEmailRepository NotificationEmails { get; }

        INotificationRepository Notifications { get; }

        IRackEntryRepository RackEntries { get; }

        IRackRepository Racks { get; }

        IRoomRepository Rooms { get; }

        IRosterNoteRepository RosterNotes { get; }

        IRosterRepository Rosters { get; }

        ISopCategoryRepository SopCategories { get; }

        ISopRepository Sops { get; }

        ISourceRepository Sources { get; }

        ISpeciesRepository Species { get; }

        IStrainRepository Strains { get; }

        IStudentRepository Students { get; }

        ISurgeryTypeRepository SurgeryTypes { get; }

        ISurgicalNoteRepository SurgicalNotes { get; }

        ISurgicalWelfareScoreRepository SurgicalWelfareScores { get; }

        ITechnicianRepository Technicians { get; }

        ITransgeneRepository Transgenes { get; }

        IVeterinarianRepository Veterinarians { get; }

        IVetScheduleRepository VetSchedules { get; }

        IVirusTypeRepository VirusTypes { get; }

        Task<int> Complete();
    }
}