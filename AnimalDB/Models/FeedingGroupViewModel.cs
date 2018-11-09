namespace AnimalDB.Models
{
    using AnimalDB.Repo.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AddToFeedingGroupViewModel
    {
        public AddToFeedingGroupViewModel()
        {
            this.ExistingAnimals = new HashSet<Repo.Entities.Animal>();
        }

        public int Id { get; set; }

        [Display(Name = "Unique Animal Id")]
        public int? Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        [NotMapped]
        public virtual ICollection<Repo.Entities.Animal> ExistingAnimals { get; set; }
    }

    public class CreateGroupViewModel
    {
        public CreateGroupViewModel()
        {
            this.AnimalGroups = new HashSet<AnimalGroup>();
            this.Groups = new HashSet<Group>();
        }

        public int? Id { get; set; }

        public string Description { get; set; }

        public ICollection<AnimalGroup> AnimalGroups { get; set; }

        public ICollection<Group> Groups { get; set; }
    }

    public class AnimalGroup
    {
        public int? Id { get; set; }

        public string UniqueAnimalId { get; set; }

        public int? Group_Id { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> GroupList { get; set; }
    }
}
