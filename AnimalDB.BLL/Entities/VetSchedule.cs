using System;

namespace AnimalDB.Repo.Entities
{
    public class VetSchedule
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
