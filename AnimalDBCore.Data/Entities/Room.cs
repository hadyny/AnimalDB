using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnimalDBCore.Core.Interfaces;

namespace  AnimalDBCore.Core.Entities
{
    public class Room
    {
        public Room()
        {
            this.Animals = new HashSet<Animal>();
            this.FeedingGroups = new HashSet<FeedingGroup>();
            this.Roster = new HashSet<Roster>();
            this.MissedChecks = new HashSet<NotCheckedRoom>();
        }

        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Email Updates")]
        public bool EmailUpdates { get; set; }

        [Display(Name = "Has animals not in database")]
        public bool NoDBAnimals { get; set; }

        public DateTime? NoDBAnimalsLastCheck { get; set; }

        public virtual ICollection<NotCheckedRoom> MissedChecks { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }

        [Required(ErrorMessage = "The field Technician is required")]
        public string Technician_Id { get; set; }
        [ForeignKey("Technician_Id")]
        public virtual IAnimalUser Technician { get; set; }

        public virtual ICollection<FeedingGroup> FeedingGroups { get; set; }

        public virtual ICollection<Roster> Roster { get; set; }
    }
}