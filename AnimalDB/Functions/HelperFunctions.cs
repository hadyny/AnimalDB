using AnimalDB.Models;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Implementations;
using System.Linq;
using System.Web;

namespace AnimalDB.Functions
{
    public static class HelperFunctions
    {
        public static UserType? GetUserType(string username)
        {
            if (new AdministratorRepo().GetAdministratorByUsername(username) != null)
            {
                return UserType.Administrator;
            }
            else if (new TechnicianRepo().GetTechnicianByUsername(username) != null)
            {
                return UserType.Technician;
            }
            else if (new VeterinarianRepo().GetVeterinarianByUsername(username) != null)
            {
                return UserType.Veterinarian;
            }
            else if (new InvestigatorRepo().GetInvestigatorByUsername(username) != null)
            {
                return UserType.Investigator;
            }
            else if (new StudentRepo().GetStudentByUsername(username) != null)
            {
                return UserType.Student;
            }
            
            return null;
        }

        internal static MenuViewModel CreateNavStatus(string username)
        {
            var model = new MenuViewModel();
            model.UserRole = GetUserType(username);

            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

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

            model.NotificationAmount = new NotificationRepo().GetPastNotifications().Count();

            return model;
        }
    }
}