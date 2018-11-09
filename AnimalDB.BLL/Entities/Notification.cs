using AnimalDB.Repo.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Display(Name = "Notification Date & Time")]
        [UIHint("LongDateTime")]
        public DateTime NotificationDate { get; set; }

        public NotificationType Type { get; set; }

        public int? Medication_Id { get; set; }
        [ForeignKey("Medication_Id")]
        public virtual Medication Medication { get; set; }

        public int? Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }
    }
}