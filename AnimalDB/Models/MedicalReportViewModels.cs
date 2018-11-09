using System;
using System.Collections.Generic;

namespace AnimalDB.Models
{
    public class MedicalReportViewModel
    {
        public MedicalReportViewModel()
        {
            this.MedicalItems = new HashSet<MedicalReportItem>();
        }

        public Repo.Entities.Animal Animal { get; set; }

        public IEnumerable<MedicalReportItem> MedicalItems { get; set; }
    }

    public class MedicalReportItem
    {
        public string Type { get; set; }

        public DateTime Timestamp { get; set; }

        public string Details { get; set; }

        public string Description { get; set; }

        public string Css { get; set; }
    }
}