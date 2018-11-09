namespace AnimalDB.Models
{
    using AnimalDB.Repo.Entities;
    using System.ComponentModel.DataAnnotations;

    public class BatchAnimalViewModel
    {
        [Display(Name = "Amount in batch")]
        public int Amount { get; set; }

        [UIHint("AnimalTemplate")]
        public Animal Animal { get; set; }
    }
}
