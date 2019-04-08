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
    public class ChargeCodeRepo : IChargeCode, IDisposable
    {
        private AnimalDBContext db;

        public ChargeCodeRepo()
        {
            this.db = new AnimalDBContext();
        }
        public ChargeCodeRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateChargeCode(ChargeCode chargeCode)
        {
            db.ChargeCode.Add(chargeCode);
            await db.SaveChangesAsync();
        }

        public async Task DeleteChargeCode(ChargeCode chargeCode)
        {
            if (db.Entry(chargeCode).State == EntityState.Detached)
            {
                db.ChargeCode.Attach(chargeCode);
            }
            db.ChargeCode.Remove(chargeCode);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<ChargeCode> GetChargeCodeById(int id)
        {
            return await db.ChargeCode.FindAsync(id);
        }

        public IEnumerable<ChargeCode> GetChargeCodes()
        {
            return db.ChargeCode.ToList();
        }

        public async Task UpdateChargeCode(ChargeCode chargeCode)
        {
            db.Entry(chargeCode).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}