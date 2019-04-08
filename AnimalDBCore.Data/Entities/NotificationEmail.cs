using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnimalDBCore.Core.Interfaces;

namespace  AnimalDBCore.Core.Entities
{
    public class NotificationEmail
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual IAnimalUser Investigator { get; set; }
    }
}