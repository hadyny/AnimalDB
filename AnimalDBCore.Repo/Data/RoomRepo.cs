﻿using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class RoomRepo : IRoom, IDisposable
    {
        private AnimalDBContext db;

        public RoomRepo()
        {
            this.db = new AnimalDBContext();
        }

        public RoomRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateRoom(Room room)
        {
            db.Rooms.Add(room);
            await db.SaveChangesAsync();
        }

        public int GetCountOfLivingAnimalsByRoomId(int roomId)
        {
            return db.Rooms
                    .Find(roomId)
                    .Animals
                    .Count(m => m.DeathDate == null);
        }

        public int GetCountOfGMOAnimalsByRoomId(int roomId)
        {
            return db.Rooms.Find(roomId)
                    .Animals
                    .Where(m => m.ApprovalNumber_Id != null && m.DeathDate == null)
                    .Count();
        }

        public IEnumerable<Animal> GetLivingAnimalsByRoomId(int roomId)
        {
            return db.Rooms.Find(roomId).Animals.Where(m => m.DeathDate == null);
        }

        public async Task DeleteRoom(Room room)
        {
            if (db.Entry(room).State == EntityState.Detached)
            {
                db.Rooms.Attach(room);
            }
            db.Rooms.Remove(room);
            await db.SaveChangesAsync();
        }

        public async Task<Room> GetRoomById(int id)
        {
            return await db.Rooms.FindAsync(id);
        }

        public IEnumerable<Room> GetRooms()
        {
            return db.Rooms.ToList();
        }

        public async Task UpdateRoom(Room room)
        {
            db.Entry(room).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<Room> RoomsThatHaventBeenCheckedToday()
        {

            var r =  db.Rooms
                    .Where(m => 
                            m.NoDBAnimals &&
                            (!m.NoDBAnimalsLastCheck.HasValue || 
                                (m.NoDBAnimalsLastCheck.HasValue && 
                                EF.Functions.DateDiffDay(m.NoDBAnimalsLastCheck.Value, DateTime.Now) != 0)))
                    .ToList();
            return r;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task MarkAllAnimalsAsCheckedByRoomId(int roomId)
        {
            var room = await db.Rooms.FindAsync(roomId);

            foreach (var animal in room.Animals.Where(m => m.DeathDate == null))
            {
                animal.LastChecked = DateTime.Now.Date;
            }

            await db.SaveChangesAsync();
        }

        public async Task MarkRoomAsChecked(int roomId)
        {
            var room = await db.Rooms.FindAsync(roomId);

            room.NoDBAnimalsLastCheck = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}