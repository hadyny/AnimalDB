using AnimalDB.Models;
using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AnimalDB.Web.Controllers
{
    public class NavController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public NavController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            // Can't have an async partial view, so methods need to run synchronously
            var syncContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);

            var notifications = _unitOfWork.Notifications.GetPast();

            var model = new MenuViewModel
            {
                UserRole = _userManagementService.GetUserType(User.Identity.Name).Result,
                NotificationAmount = notifications.Count()
            };

            SynchronizationContext.SetSynchronizationContext(syncContext);

            var controller = Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var action = Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

            string cl = "active";
            switch (controller)
            {
                case "animals":
                    if (action == "details")
                    {
                        model.a = cl;
                    }
                    else
                    {
                        model.ad = cl;
                    }
                    break;
                case "home":
                case "note":
                case "medication":
                case "surgicalnote":
                case "cagelocationhistory":
                case "ethicsnumberhistory":
                case "relationships":
                case "reports":
                case "fullmedical":
                case "clinicalincidentreports":
                    model.a = cl;
                    break;
                case "documents":
                case "documentcategories":
                    model.d = cl;
                    break;
                case "feedinggroup":
                    model.f = cl;
                    break;
                case "feed":
                    if (action == "details")
                    {
                        model.a = cl;
                    }
                    else
                    {
                        model.w = cl;
                    }
                    break;
                case "admin":
                case "ethicsnumber":
                case "medicationtype":
                case "cagelocation":
                case "arrivalstatus":
                case "approvalnumbers":
                case "colour":
                case "investigator":
                case "source":
                case "species":
                case "strain":
                case "surgeon":
                case "surgerytype":
                case "group":
                case "cleaningtask":
                case "cleaningroomlocation":
                case "cleaninglocation":
                case "animalmanipulationreports":
                case "identitycards":
                case "students":
                case "culled":
                case "notificationemails":
                case "administrators":
                case "racks":
                case "rooms":
                case "technician":
                case "vetinarian":
                case "virustypes":
                    model.ad = cl;
                    break;
                case "notifications":
                    model.n = cl;
                    break;
                case "sops":
                case "sopcategories":
                    model.s = cl;
                    break;
                case "rosters":
                case "rosternotes":
                    model.r = cl;
                    break;
                case "vetschedules":
                    model.v = cl;
                    break;
            }

            return PartialView(model);
        }
    }
}