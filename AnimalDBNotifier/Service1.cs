using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;

namespace AnimalDB.Notifier
{
    public partial class AnimalDBNotifier : ServiceBase
    {
        private EventLog eventLog1;
        private System.Timers.Timer timer1;

        public AnimalDBNotifier()
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("Animal Database Notifier"))
            {
               EventLog.CreateEventSource(
                    "Animal Database Notifier", "Animal Database Notifier Log");
            }
            eventLog1.Source = "Animal Database Notifier";
            eventLog1.Log = "Animal Database Notifier Log";

            timer1 = new System.Timers.Timer(300000);
            timer1.Elapsed += timer1_Tick;
            timer1.Enabled = true;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Animal Database Notification Service started", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Animal Database Notification Service stopped", EventLogEntryType.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var notifications = from n in db.Notifications
                                        where n.NotificationDate >= DateTime.Now && n.NotificationDate < DateTime.Now.AddMinutes(5)
                                        select n;
                    SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SystemUsername"], ConfigurationManager.AppSettings["SystemPassword"]);

                    smtp.Port = 587;

                    foreach (var notification in notifications)
                    {
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress(ConfigurationManager.AppSettings["SystemEmail"]);
                        msg.Subject = "Notification from the Animal Database";
                        msg.IsBodyHtml = true;
                        switch (notification.Type)
                        {
                            case NotificationType.Medication:
                                var reipient = db.AspNetUsers.Single(m => m.Id == notification.Medication.WhoToNotify_Id);
                                if (reipient != null) {
                                    msg.To.Add(reipient.Email);
                                    msg.Body = "There is new medication due for " + notification.Animal.UniqueAnimalId + "<br /><br />";
                                    msg.Body += "Animal: " + notification.Animal.UniqueAnimalId + "<br />";
                                    msg.Body += "Room: " + notification.Animal.Room.Description + "<br />";
                                    msg.Body += "Medication: " + notification.Medication.MedicationType.Description + "<br />";
                                    msg.Body += "Dosage: " + notification.Medication.Dosage + "<br />";
                                    msg.Body += "Due: " + notification.NotificationDate.ToString();
                                }
                                break;
                            /*
                        case NotificationType.Injection:
                            msg.To.Add(notification.Animal.AspNetUser.Email);
                            msg.Body = "There is an injection due today for " + notification.Animal.UniqueAnimalId + "<br /><br />";
                            break;
                             */
                        }
                        if (msg.To.Count() != 0)
                        {
                            smtp.Send(msg);
                            eventLog1.WriteEntry("Animal Database Notification Sent to " + msg.To.ToString(), EventLogEntryType.Information);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Animal Database Notifier ran into an error: " + ex.Message, EventLogEntryType.Error);
            }
        }
    }

    public enum NotificationType
    {
        Medication = 0, 
        Injection = 1
    }
}
