using System.Collections.Generic;
using AnimalDB.Models;
using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Models
{
    public class AddInvestigatorVM
    {
        public AddInvestigatorVM()
        {
            this.Investigators = new HashSet<Repo.Entities.Investigator>();
        }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public string Selected_Investigator_Id { get; set; }

        public ICollection<Repo.Entities.Investigator> Investigators { get; set; }
    }
}