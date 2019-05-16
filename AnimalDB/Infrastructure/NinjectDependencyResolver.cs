using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repo.Services;
using AnimalDB.Repositories.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AnimalDB.Web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // Bind Services
            kernel.Bind<IAdministratorService>().To<AdministratorService>();
            kernel.Bind<IAnimalManipulationReportService>().To<AnimalManipulationReportService>();
            kernel.Bind<IAnimalRoomCountService>().To<AnimalRoomCountService>();
            kernel.Bind<IAnimalService>().To<AnimalService>();
            kernel.Bind<IApprovalNumberService>().To<ApprovalNumberService>();
            kernel.Bind<IArrivalStatusService>().To<ArrivalStatusService>();
            kernel.Bind<ICageLocationHistoryService>().To<CageLocationHistoryService>();
            kernel.Bind<ICageLocationService>().To<CageLocationService>();
            kernel.Bind<IChargeCodeService>().To<ChargeCodeService>();
            kernel.Bind<IClinicalIncidentReportService>().To<ClinicalIncidentReportService>();
            kernel.Bind<IColourService>().To<ColourService>();
            kernel.Bind<ICulledPupsService>().To<CulledPupsService>();
            kernel.Bind<IDocumentCategoryService>().To<DocumentCategoryService>();
            kernel.Bind<IDocumentService>().To<DocumentService>();
            kernel.Bind<IEthicsDocumentService>().To<EthicsDocumentService>();
            kernel.Bind<IEthicsNumberHistoryService>().To<EthicsNumberHistoryService>();
            kernel.Bind<IEthicsNumberService>().To<EthicsNumberService>();
            kernel.Bind<IFeedingGroupService>().To<FeedingGroupService>();
            kernel.Bind<IFeedService>().To<FeedService>();
            kernel.Bind<IGDTimelineService>().To<GDTimelineService>();
            kernel.Bind<IGroupService>().To<GroupService>();
            kernel.Bind<IInvestigatorService>().To<InvestigatorService>();
            kernel.Bind<IMedicationFollowUpService>().To<MedicationFollowUpService>();
            kernel.Bind<IMedicationService>().To<MedicationService>();
            kernel.Bind<IMedicationTypeService>().To<MedicationTypeService>();
            kernel.Bind<INotCheckedAnimalService>().To<NotCheckedAnimalService>();
            kernel.Bind<INotCheckedRoomService>().To<NotCheckedRoomService>();
            kernel.Bind<INoteService>().To<NoteService>();
            kernel.Bind<INotificationEmailService>().To<NotificationEmailService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
            kernel.Bind<IRackEntryService>().To<RackEntryService>();
            kernel.Bind<IRackService>().To<RackService>();
            kernel.Bind<IRoomService>().To<RoomService>();
            kernel.Bind<IRosterNoteService>().To<RosterNoteService>();
            kernel.Bind<IRosterService>().To<RosterService>();
            kernel.Bind<ISopCategoryService>().To<SopCategoryService>();
            kernel.Bind<ISopService>().To<SopService>();
            kernel.Bind<ISourceService>().To<SourceService>();
            kernel.Bind<ISpeciesService>().To<SpeciesService>();
            kernel.Bind<IStrainService>().To<StrainService>();
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<ISurgeryTypeService>().To<SurgeryTypeService>();
            kernel.Bind<ISurgicalNoteService>().To<SurgicalNoteService>();
            kernel.Bind<ISurgicalWelfareScoreService>().To<SurgicalWelfareScoreService>();
            kernel.Bind<ITechnicianService>().To<TechnicianService>();
            kernel.Bind<ITransgeneService>().To<TransgeneService>();
            kernel.Bind<IUserManagementService>().To<UserManagementService>();
            kernel.Bind<IVeterinarianService>().To<VeterinarianService>();
            kernel.Bind<IVetScheduleService>().To<VetScheduleService>();
            kernel.Bind<IVirusTypeService>().To<VirusTypeService>();            
            
            // Bind generic repository
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind(typeof(IUserRepository<>)).To(typeof(UserRepository<>));
        }
    }
}
