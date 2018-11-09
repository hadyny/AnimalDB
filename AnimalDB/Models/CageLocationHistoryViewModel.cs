namespace AnimalDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CageLocationHistoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Date Moved to Location")]
        public DateTime Timestamp { get; set; }
        [Display(Name = "Cage Location")]
        public int? CageLocation_Id { get; set; }
        public int? RackEntry_X { get; set; }
        public string RackEntry_Y { get; set; }
        public int? SelectedRack { get; set; }
        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }

        public IEnumerable<Repo.Entities.Rack> Racks { get; set; }

    }
}
