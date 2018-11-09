namespace AnimalDB.Models
{
    using System.ComponentModel.DataAnnotations;


    public class AddToGroupViewModel
    {
        public int GroupId { get; set; }
        [Required, Display(Name="Unique Animal ID")]
        public string AnimalId { get; set; }
    }
}
