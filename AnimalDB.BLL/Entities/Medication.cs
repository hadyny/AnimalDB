namespace AnimalDB.Repo.Entities
{
    using AnimalDB.Repo.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Medication
    {
        public Medication() {
            this.Reminders = new HashSet<Notification>();
        }

        public int Id { get; set; }
        [Display(Name = "Start Date")]
        public DateTime Timestamp { get; set; }
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public string Dosage { get; set; }

        public string Rate { get; set; }

        public int? Frequency { get; set; }

        public RecurringType? FrequencyValue { get; set; }

        public int? Duration { get; set; }

        public RecurringType? DurationValue { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Medication Type")]
        public int MedicationType_Id { get; set; }
        [ForeignKey("MedicationType_Id")]
        public virtual MedicationType MedicationType { get; set; }

        [Display(Name = "Clinical Incident Report")]
        public int? IncidentReport_Id { get; set; }
        [ForeignKey("IncidentReport_Id")]
        public virtual ClinicalIncidentReport Report { get; set; }

        public virtual ICollection<MedicationFollowUp> FollowUps { get; set; }

        public virtual ICollection<Notification> Reminders { get; set; }

        [Display(Name = "Who will get reminders for the medication?")]
        public string WhoToNotify_Id { get; set; }
        [ForeignKey("WhoToNotify_Id")]
        public virtual AnimalUser WhoToNotify { get; set; }
    }

    public class MedicationFollowUp
    {
        public int Id { get; set; }

        [UIHint("LongDateTime"), Display(Name = "Time of Administration")]
        public DateTime Timestamp { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Medication")]
        public int Medication_Id { get; set; }
        [ForeignKey("Medication_Id")]
        public virtual Medication Medication { get; set; }
    }
}