namespace AnimalDB.Repo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CulledPups
    {
        public int Id { get; set; }

        [Display(Name = "Mother")]
        public int AnimalId { get; set; }
        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; }
        [Display(Name = "Amount of pups culled")]
        public int AmountCulled { get; set; }
        [Display(Name = "Date pups were culled")]
        public DateTime DateCulled { get; set; }
        [Display(Name = "How many of the pups were female?")]
        public int? NumFemale { get; set; }
        [Display(Name = "How many of the pups were male?")]
        public int? NumMale { get; set; }
    }
}