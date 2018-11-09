namespace AnimalDB.Models
{
    using System.ComponentModel.DataAnnotations;


    public class AddToEthicsViewModel
    {
        public int EthicsId { get; set; }
        [Required, Display(Name = "Unique Animal ID")]
        public string AnimalId { get; set; }
    }

    public class EthicsNumberViewModel
    {
        [Required]
        public int? Id { get; set; }
    }
}
