using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class NotificationEmail
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual Investigator Investigator { get; set; }
    }
}