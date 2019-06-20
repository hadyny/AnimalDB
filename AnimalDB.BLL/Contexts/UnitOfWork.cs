using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repo.Repositories.Concrete;
using System;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnimalDBContext context = new AnimalDBContext();
        public AnimalDBContext Context
        {
            get {
                    return context;
                }
        }

        private IAdministratorRepository administrators;
        public IAdministratorRepository Administrators
        {
            get
            {
                if (this.administrators == null)
                {
                    this.administrators = new AdministratorRepository(context);
                }
                return administrators;
            }
        }

        private IAnimalManipulationReportRepository animalManipulationReports;
        public IAnimalManipulationReportRepository AnimalManipulationReports
        {
            get
            {
                if (this.animalManipulationReports == null)
                {
                    this.animalManipulationReports = new AnimalManipulationReportRepository(context);
                }
                return animalManipulationReports;
            }
        }

        private IAnimalRepository animals;
        public IAnimalRepository Animals
        {
            get
            {
                if (this.animals == null)
                {
                    this.animals = new AnimalRepository(context);
                }
                return animals;
            }
        }



        private IAnimalRoomCountRepository animalRoomCounts;
        public IAnimalRoomCountRepository AnimalRoomCounts
        {
            get
            {
                if (this.animalRoomCounts == null)
                {
                    this.animalRoomCounts = new AnimalRoomCountRepository(context);
                }
                return animalRoomCounts;
            }
        }

        private IApprovalNumberRepository approvalNumbers;
        public IApprovalNumberRepository ApprovalNumbers
        {
            get
            {
                if (this.approvalNumbers == null)
                {
                    this.approvalNumbers = new ApprovalNumberRepository(context);
                }
                return approvalNumbers;
            }
        }

        private IArrivalStatusRepository arrivalStatus;
        public IArrivalStatusRepository ArrivalStatus
        {
            get
            {
                if (this.arrivalStatus == null)
                {
                    this.arrivalStatus = new ArrivalStatusRepository(context);
                }
                return arrivalStatus;
            }
        }

        private ICageLocationHistoryRepository cageLocationHistories;
        public ICageLocationHistoryRepository CageLocationHistories
        {
            get
            {
                if (this.cageLocationHistories == null)
                {
                    this.cageLocationHistories = new CageLocationHistoryRepository(context);
                }
                return cageLocationHistories;
            }
        }

        private ICageLocationRepository cageLocations;
        public ICageLocationRepository CageLocations
        {
            get
            {
                if (this.cageLocations == null)
                {
                    this.cageLocations = new CageLocationRepository(context);
                }
                return cageLocations;
            }
        }

        private IChargeCodeRepository chargeCodes;
        public IChargeCodeRepository ChargeCodes
        {
            get
            {
                if (this.chargeCodes == null)
                {
                    this.chargeCodes = new ChargeCodeRepository(context);
                }
                return chargeCodes;
            }
        }

        private IClinicalIncidentReportRepository clinicalIncidentReports;
        public IClinicalIncidentReportRepository ClinicalIncidentReports
        {
            get
            {
                if (this.clinicalIncidentReports == null)
                {
                    this.clinicalIncidentReports = new ClinicalIncidentReportRepository(context);
                }
                return clinicalIncidentReports;
            }
        }

        private IColourRepository colour;
        public IColourRepository Colours
        {
            get
            {
                if (this.colour == null)
                {
                    this.colour = new ColourRepository(context);
                }
                return colour;
            }
        }

        private ICulledPupsRepository culledPups;
        public ICulledPupsRepository CulledPups
        {
            get
            {
                if (this.culledPups == null)
                {
                    this.culledPups = new CulledPupsRepository(context);
                }
                return culledPups;
            }
        }

        private IDocumentCategoryRepository documentCategories;
        public IDocumentCategoryRepository DocumentCategories
        {
            get
            {
                if (this.documentCategories == null)
                {
                    this.documentCategories = new DocumentCategoryRepository(context);
                }
                return documentCategories;
            }
        }

        private IDocumentRepository documents;
        public IDocumentRepository Documents
        {
            get
            {
                if (this.documents == null)
                {
                    this.documents = new DocumentRepository(context);
                }
                return documents;
            }
        }

        private IEthicsDocumentRepository ethicsDocuments;
        public IEthicsDocumentRepository EthicsDocuments
        {
            get
            {
                if (this.ethicsDocuments == null)
                {
                    this.ethicsDocuments = new EthicsDocumentRepository(context);
                }
                return ethicsDocuments;
            }
        }

        private IEthicsNumberHistoryRepository ethicsNumberHistories;
        public IEthicsNumberHistoryRepository EthicsNumberHistories
        {
            get
            {
                if (this.ethicsNumberHistories == null)
                {
                    this.ethicsNumberHistories = new EthicsNumberHistoryRepository(context);
                }
                return ethicsNumberHistories;
            }
        }

        private IEthicsNumberRepository ethicsNumbers;
        public IEthicsNumberRepository EthicsNumbers
        {
            get
            {
                if (this.ethicsNumbers == null)
                {
                    this.ethicsNumbers = new EthicsNumberRepository(context);
                }
                return ethicsNumbers;
            }
        }

        private IFeedingGroupRepository feedingGroups;
        public IFeedingGroupRepository FeedingGroups
        {
            get
            {
                if (this.feedingGroups == null)
                {
                    this.feedingGroups = new FeedingGroupRepository(context);
                }
                return feedingGroups;
            }
        }

        private IFeedRepository feeds;
        public IFeedRepository Feeds
        {
            get
            {
                if (this.feeds == null)
                {
                    this.feeds = new FeedRepository(context);
                }
                return feeds;
            }
        }

        private IGDTimelineRepository gDTimelines;
        public IGDTimelineRepository GDTimelines
        {
            get
            {
                if (this.gDTimelines == null)
                {
                    this.gDTimelines = new GDTimelineRepository(context);
                }
                return gDTimelines;
            }
        }

        private IGroupRepository groups;
        public IGroupRepository Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new GroupRepository(context);
                }
                return groups;
            }
        }

        private IInvestigatorRepository investigators;
        public IInvestigatorRepository Investigators
        {
            get
            {
                if (this.investigators == null)
                {
                    this.investigators = new InvestigatorRepository(context);
                }
                return investigators;
            }
        }

        private IMedicationFollowUpRepository medicationFollowUps;
        public IMedicationFollowUpRepository MedicationFollowUps
        {
            get
            {
                if (this.medicationFollowUps == null)
                {
                    this.medicationFollowUps = new MedicationFollowUpRepository(context);
                }
                return medicationFollowUps;
            }
        }

        private IMedicationRepository medication;
        public IMedicationRepository Medication
        {
            get
            {
                if (this.medication == null)
                {
                    this.medication = new MedicationRepository(context);
                }
                return medication;
            }
        }

        private IMedicationTypeRepository medicationTypes;
        public IMedicationTypeRepository MedicationTypes
        {
            get
            {
                if (this.medicationTypes == null)
                {
                    this.medicationTypes = new MedicationTypeRepository(context);
                }
                return medicationTypes;
            }
        }

        private INotCheckedAnimalRepository notCheckedAnimals;
        public INotCheckedAnimalRepository NotCheckedAnimals
        {
            get
            {
                if (this.notCheckedAnimals == null)
                {
                    this.notCheckedAnimals = new NotCheckedAnimalRepository(context);
                }
                return notCheckedAnimals;
            }
        }

        private INotCheckedRoomRepository notCheckedRooms;
        public INotCheckedRoomRepository NotCheckedRooms
        {
            get
            {
                if (this.notCheckedRooms == null)
                {
                    this.notCheckedRooms = new NotCheckedRoomRepository(context);
                }
                return notCheckedRooms;
            }
        }

        private INoteRepository notes;
        public INoteRepository Notes
        {
            get
            {
                if (this.notes == null)
                {
                    this.notes = new NoteRepository(context);
                }
                return notes;
            }
        }

        private INotificationEmailRepository notificationEmails;
        public INotificationEmailRepository NotificationEmails
        {
            get
            {
                if (this.notificationEmails == null)
                {
                    this.notificationEmails = new NotificationEmailRepository(context);
                }
                return notificationEmails;
            }
        }

        private INotificationRepository notifications;
        public INotificationRepository Notifications
        {
            get
            {
                if (this.notifications == null)
                {
                    this.notifications = new NotificationRepository(context);
                }
                return notifications;
            }
        }

        private IRackEntryRepository rackEntries;
        public IRackEntryRepository RackEntries
        {
            get
            {
                if (this.rackEntries == null)
                {
                    this.rackEntries = new RackEntryRepository(context);
                }
                return rackEntries;
            }
        }

        private IRackRepository racks;
        public IRackRepository Racks
        {
            get
            {
                if (this.racks == null)
                {
                    this.racks = new RackRepository(context);
                }
                return racks;
            }
        }

        private IRoomRepository rooms;
        public IRoomRepository Rooms
        {
            get
            {
                if (this.rooms == null)
                {
                    this.rooms = new RoomRepository(context);
                }
                return rooms;
            }
        }

        private IRosterNoteRepository rosterNotes;
        public IRosterNoteRepository RosterNotes
        {
            get
            {
                if (this.rosterNotes == null)
                {
                    this.rosterNotes = new RosterNoteRepository(context);
                }
                return rosterNotes;
            }
        }

        private IRosterRepository rosters;
        public IRosterRepository Rosters
        {
            get
            {
                if (this.rosters == null)
                {
                    this.rosters = new RosterRepository(context);
                }
                return rosters;
            }
        }

        private ISopCategoryRepository sopCategories;
        public ISopCategoryRepository SopCategories
        {
            get
            {
                if (this.sopCategories == null)
                {
                    this.sopCategories = new SopCategoryRepository(context);
                }
                return sopCategories;
            }
        }

        private ISopRepository sops;
        public ISopRepository Sops
        {
            get
            {
                if (this.sops == null)
                {
                    this.sops = new SopRepository(context);
                }
                return sops;
            }
        }

        private ISourceRepository sources;
        public ISourceRepository Sources
        {
            get
            {
                if (this.sources == null)
                {
                    this.sources = new SourceRepository(context);
                }
                return sources;
            }
        }

        private ISpeciesRepository species;
        public ISpeciesRepository Species
        {
            get
            {
                if (this.species == null)
                {
                    this.species = new SpeciesRepository(context);
                }
                return species;
            }
        }

        private IStrainRepository strains;
        public IStrainRepository Strains
        {
            get
            {
                if (this.strains == null)
                {
                    this.strains = new StrainRepository(context);
                }
                return strains;
            }
        }

        private IStudentRepository students;
        public IStudentRepository Students
        {
            get
            {
                if (this.students == null)
                {
                    this.students = new StudentRepository(context);
                }
                return students;
            }
        }

        private ISurgeryTypeRepository surgeryTypes;
        public ISurgeryTypeRepository SurgeryTypes
        {
            get
            {
                if (this.surgeryTypes == null)
                {
                    this.surgeryTypes = new SurgeryTypeRepository(context);
                }
                return surgeryTypes;
            }
        }

        private ISurgicalNoteRepository surgicalNotes;
        public ISurgicalNoteRepository SurgicalNotes
        {
            get
            {
                if (this.surgicalNotes == null)
                {
                    this.surgicalNotes = new SurgicalNoteRepository(context);
                }
                return surgicalNotes;
            }
        }

        private ISurgicalWelfareScoreRepository surgicalWelfareScores;
        public ISurgicalWelfareScoreRepository SurgicalWelfareScores
        {
            get
            {
                if (this.surgicalWelfareScores == null)
                {
                    this.surgicalWelfareScores = new SurgicalWelfareScoreRepository(context);
                }
                return surgicalWelfareScores;
            }
        }

        private ITechnicianRepository technicians;
        public ITechnicianRepository Technicians
        {
            get
            {
                if (this.technicians == null)
                {
                    this.technicians = new TechnicianRepository(context);
                }
                return technicians;
            }
        }

        private ITransgeneRepository transgenes;
        public ITransgeneRepository Transgenes
        {
            get
            {
                if (this.transgenes == null)
                {
                    this.transgenes = new TransgeneRepository(context);
                }
                return transgenes;
            }
        }

        private IVeterinarianRepository veterinarians;
        public IVeterinarianRepository Veterinarians
        {
            get
            {
                if (this.veterinarians == null)
                {
                    this.veterinarians = new VeterinarianRepository(context);
                }
                return veterinarians;
            }
        }

        private IVetScheduleRepository vetSchedules;
        public IVetScheduleRepository VetSchedules
        {
            get
            {
                if (this.vetSchedules == null)
                {
                    this.vetSchedules = new VetScheduleRepository(context);
                }
                return vetSchedules;
            }
        }

        private IVirusTypeRepository virusTypes;
        public IVirusTypeRepository VirusTypes
        {
            get
            {
                if (this.virusTypes == null)
                {
                    this.virusTypes = new VirusTypeRepository(context);
                }
                return virusTypes;
            }
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}