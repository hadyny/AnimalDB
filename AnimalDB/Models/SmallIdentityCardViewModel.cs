using AnimalDB.Repo.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Models
{
    public class SmallIdentityCardViewModel
    {
        [Display(Name = "Name")]
        public string UniqueAnimalId { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Date of Arrival")]
        public DateTime? ArrivalDate { get; set; }
        public string Strain { get; set; }
    }
}