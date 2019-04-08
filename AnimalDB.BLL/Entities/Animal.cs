namespace AnimalDB.Repo.Entities
{
    using AnimalDB.Repo.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Animal
    {
        public Animal()
        {
            Notes = new HashSet<Note>();
            Parents = new HashSet<Animal>();
            Offspring = new HashSet<Animal>();
            EthicsNumbers = new HashSet<EthicsNumberHistory>();
            CageLocations = new HashSet<CageLocationHistory>();
            Feeds = new HashSet<Feed>();
            Medications = new HashSet<Medication>();
            IncidentReports = new HashSet<ClinicalIncidentReport>();
            SurgicalNotes = new HashSet<SurgicalNote>();
        }
        
        public int Id { get; set; }
        [Required, Display(Name = "Unique Animal ID")]
        public string UniqueAnimalId { get; set; }
        /*
        public int? ChargeCode_Id { get; set; }
        [Display(Name = "Charge Code"), ForeignKey("ChargeCode_Id")]
        public virtual ChargeCode ChargeCode { get; set; }
        */
        [Display(Name = "Arrival Date")]
        public DateTime? ArrivalDate { get; set; }
        [Display(Name = "Born Here?")]
        public bool BornHere { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Death Date")]
        public DateTime? DeathDate { get; set; }

        [Display(Name = "Cause of Death")]
        public CauseOfDeathEnum? CauseOfDeath { get; set; }

        [MaxLength(50)]
        public string Tag { get; set; }

        [Display(Name = "Investigator")]
        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual Investigator Investigator { get; set; }

        [Display(Name = "Researcher")]
        public string Researcher_Id { get; set; }
        [ForeignKey("Researcher_Id")]
        public virtual AnimalUser Researcher { get; set; }


        [Display(Name = "Colour")]
        public int? Colour_Id { get; set; }
        [ForeignKey("Colour_Id")]
        public virtual Colour Colour { get; set; }

        [Display(Name = "Stock Animal")]
        public bool StockAnimal { get; set; }

        [Display(Name = "Base Weight")]
        public int? BaseWeight { get; set; }

        [Display(Name = "Feeding Group")]
        public int? FeedingGroup_Id { get; set; }
        [ForeignKey("FeedingGroup_Id")]
        public virtual FeedingGroup FeedingGroup { get; set; }

        [Display(Name = "Strain")]
        public int? Strain_Id { get; set; }
        [ForeignKey("Strain_Id")]
        public virtual Strain Strain { get; set; }

        [Display(Name = "Transgene ID")]
        public int? Transgene_Id { get; set; }
        [ForeignKey("Transgene_Id")]
        public virtual Transgene Transgene { get; set; }

        [Required,Display(Name = "Source")]
        public int? Source_Id { get; set; }
        [ForeignKey("Source_Id")]
        public virtual Source Source { get; set; }

        [Required,Display(Name = "Arrival Status")]
        public int? ArrivalStatus_Id { get; set; }
        [ForeignKey("ArrivalStatus_Id")]
        public virtual ArrivalStatus ArrivalStatus { get; set; }

        public Sex? Sex { get; set; }

        [Required,Display(Name = "Use For")]
        public Manipulation Manipulation { get; set; }

        [Required]
        public Grading Grading { get; set; }

        public int? ApprovalNumber_Id { get; set; }
        [ForeignKey("ApprovalNumber_Id")]
        public virtual ApprovalNumber ApprovalNumber { get; set; }

        [Required, Display(Name = "Room")]
        public int? Room_Id { get; set; }
        [ForeignKey("Room_Id")]
        public virtual Room Room { get; set; }

        public int? Group_Id { get; set; }
        [ForeignKey("Group_Id")]
        public virtual Group Group { get; set; }

        public bool HasPicture { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Animal> Offspring { get; set; }
        public virtual ICollection<Animal> Parents { get; set; }
        [Display(Name = "Ethics Number")]
        public virtual ICollection<EthicsNumberHistory> EthicsNumbers { get; set; }
        public virtual ICollection<Feed> Feeds { get; set; }
        [Display(Name = "Cage Location")]
        public virtual ICollection<CageLocationHistory> CageLocations { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<SurgicalNote> SurgicalNotes { get; set; }
        public virtual ICollection<ClinicalIncidentReport> IncidentReports { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public DateTime LastChecked { get; set; }

        public virtual ICollection<NotCheckedAnimal> MissedChecks { get; set; }
    }
}
