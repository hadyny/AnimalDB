using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Models
{
    public class RelationshipViewModel
    {
        public int? Parent_Id { get; set; }
        [ForeignKey("Parent_Id")]
        public Repo.Entities.Animal Parent { get; set; }

        public int? Child_Id { get; set; }
        [ForeignKey("Child_Id")]
        public Repo.Entities.Animal Child { get; set; }
    }
}