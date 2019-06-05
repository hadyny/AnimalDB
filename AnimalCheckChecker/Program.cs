using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repo.Services;
using AnimalDB.Repositories.Concrete;
using Ninject;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AnimalCheckChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IAnimalService>().To<AnimalService>();
            kernel.Bind<IRoomService>().To<RoomService>();
            kernel.Bind<INotCheckedRoomService>().To<NotCheckedRoomService>();
            kernel.Bind<INotCheckedAnimalService>().To<NotCheckedAnimalService>();
            kernel.Bind<IEthicsNumberHistoryService>().To<EthicsNumberHistoryService>();
            kernel.Bind<IEthicsNumberService>().To<EthicsNumberService>();
            kernel.Bind<ICageLocationHistoryService>().To<CageLocationHistoryService>();
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind(typeof(IUserRepository<>)).To(typeof(UserRepository<>));

            IRoomService roomRepo = new RoomService(kernel.Get<IRepository<Room>>());
            INotCheckedRoomService ncrRepo = new NotCheckedRoomService(kernel.Get<IRepository<NotCheckedRoom>>());
            IAnimalService animalRepo = new AnimalService(kernel.Get<IRepository<Animal>>(), 
                                                          kernel.Get<IRepository<EthicsNumberHistory>>(), 
                                                          kernel.Get<IRepository<EthicsNumber>>(), 
                                                          kernel.Get<IRepository<CageLocationHistory>>(), 
                                                          kernel.Get<IUserRepository<Student>>());
            INotCheckedAnimalService ncaRepo = new NotCheckedAnimalService(kernel.Get<IRepository<NotCheckedAnimal>>());

            /*var notCheckedRooms = db.Rooms.Where(m => m.NoDBAnimals && 
                (!m.NoDBAnimalsLastCheck.HasValue || (m.NoDBAnimalsLastCheck.HasValue && DbFunctions.TruncateTime(m.NoDBAnimalsLastCheck.Value) != DbFunctions.TruncateTime(DateTime.Now)))).ToList();
                */
            foreach (var room in roomRepo.RoomsThatHaventBeenCheckedToday().Result)
            {
                ncrRepo.CreateNotCheckedRoom(new NotCheckedRoom()
                {
                    Room_Id = room.Id,
                    Timestamp = DateTime.Now
                });
            }

            /*
            // Search for all living animals, where their last check date != today
            var notCheckedAnimals = db.Animals.Where(m => m.DeathDate == null && DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now)).ToList();
            */

            // add these animals to NotCheckedAnimal table

            foreach (var animal in animalRepo.GetLivingAnimalsNotCheckedToday().Result)
            {
                ncaRepo.CreateNotCheckedAnimal(new NotCheckedAnimal()
                {
                    Animal_Id = animal.Id,
                    Timestamp = DateTime.Now
                });
            }

            //// Search for all living animals that haven't had a feed yet
            //var notFedAnimals = db.Animals.Where(m => m.DeathDate == null &&
            //                                          (m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault() == null ||
            //                                          //(m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault().FeedAmount == -1 &&    /// Don't include free feeding
            //                                          DbFunctions.TruncateTime(m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault().Date) != DbFunctions.TruncateTime(DateTime.Now))/*)*/).ToList();


            //notCheckedAnimals = notCheckedAnimals.Where(m => m.Room.EmailUpdates).ToList();
            //notFedAnimals = notFedAnimals.Where(m => m.Room.EmailUpdates).ToList();

            if (SendEmail)
            {
                string email = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
        <title></title>
        <style></style>
    </head>
    <body>
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" height=""100%"" width=""100%"" id=""bodyTable"">
            <tr>
                <td align=""center"" valign=""top"">
                    <table border=""0"" cellpadding=""20"" cellspacing=""0"" width=""600"" id=""emailContainer"">
                        <tr>
                            <td align=""center"" valign=""top"">
                                <div style=""font-family:sans-serif;line-height:150%;margin:5%;color:#222222;font-size:14px;text-align:left;"">
                                <h2>Animal database statistics for " + DateTime.Now.ToLongDateString() + @"</h2>";

                if (animalRepo.GetLivingAnimalsNotCheckedTodayForEmailing().Result.Count() != 0)
                {
                    email += "<h3>The following animals were not checked on " + DateTime.Now.ToShortDateString() + ":</h3>";
                    string room = "";
                    int daysSinceChecked = 0;
                    foreach (var animal in animalRepo.GetLivingAnimalsNotCheckedTodayForEmailing().Result.OrderBy(m => m.Room_Id))
                    {
                        if (room != animal.Room.Description)
                        {
                            email += "<h3>" + animal.Room.Description + "</h3>";
                            room = animal.Room.Description;
                        }

                        daysSinceChecked = (DateTime.Now.Date - animal.LastChecked.Date).Days;
                        if (daysSinceChecked > 40000)
                        {
                            email += "<p>" + animal.UniqueAnimalId + ", days since check: Never checked</p>";
                        }
                        else
                        {
                            email += "<p>" + animal.UniqueAnimalId + ", days since check: " + daysSinceChecked + "</p>";
                        }
                    }
                }
                else
                {
                    email += "<h3>All animals were checked on " + DateTime.Now.ToShortDateString() + "</h3>";
                }

                email += "<hr />";

                if (animalRepo.GetLivingAnimalsNotFedTodayForEmailing().Result.Count() != 0)
                {
                    email += "<h3>The following animals were not fed on " + DateTime.Now.ToShortDateString() + ":</h3>";
                    string room = "";
                    string daysSinceFed = "";
                    foreach (var animal in animalRepo.GetLivingAnimalsNotFedTodayForEmailing().Result.OrderBy(m => m.Room_Id))
                    {
                        if (room != animal.Room.Description)
                        {
                            email += "<h3>" + animal.Room.Description + "</h3>";
                            room = animal.Room.Description;
                        }

                        daysSinceFed = animal.Feeds.OrderByDescending(m => m.Date).FirstOrDefault() == null ? "Never fed" : (DateTime.Now.Date - animal.Feeds.OrderByDescending(m => m.Date).First().Date).Days.ToString();

                        daysSinceFed += animal.Feeds.OrderByDescending(m => m.Date).FirstOrDefault() != null && animal.Feeds.OrderByDescending(m => m.Date).FirstOrDefault().FeedAmount == -1 ? " (free feeding)" : "";

                        email += "<p>" + animal.UniqueAnimalId + ", days since fed: " + daysSinceFed + "</p>";
                    }
                }
                else
                {
                    email += "<h3>All animals were fed on " + DateTime.Now.ToShortDateString() + "</h3>";
                }

                email += @"
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
</html>
";

                // send an email to notify which animals weren't checked and fed
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"])
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SystemUsername"], ConfigurationManager.AppSettings["SystemPassword"]),
                    Port = 587
                };
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SystemEmail"], "Psychology Department Web Server");
                MailAddress to = new MailAddress(EmailAddressToSend);
                MailMessage msg = new MailMessage(from, to)
                {
                    Body = email,
                    BodyEncoding = Encoding.UTF8,
                    Subject = "Animal Database: " + animalRepo.GetLivingAnimalsNotCheckedTodayForEmailing().Result.Count() + " animals weren't checked today, " + animalRepo.GetLivingAnimalsNotFedTodayForEmailing().Result.Count() + " animals weren't fed today",
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                client.Send(msg);
            }
        }
        public static string EmailAddressToSend = ConfigurationManager.AppSettings["DeveloperEmail"];
        public static bool SendEmail = false;
    }
}
