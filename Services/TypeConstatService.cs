﻿using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class TypeConstatService
    {


        private readonly ApplicationDbContext _context;

        public TypeConstatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TypeAudit>> GetAllTypeAudits()
        {
            var typeAudits = await _context.typeAudit.ToListAsync();
            return typeAudits;
        }

        public async Task<TypeAudit> GetTypeAudit(int id)
        {
            var typeAudit = await _context.typeAudit.FindAsync(id);
            return typeAudit;
        }

        public async Task<TypeAudit> AddTypeAudit(TypeAudit typeAudit)
        {
            _context.typeAudit.Add(typeAudit);
            await _context.SaveChangesAsync();
            return typeAudit;
        }

        public async Task<TypeAudit> UpdateTypeAudit(int id, TypeAudit typeAudit)
        {
            var existingTypeAudit = await _context.typeAudit.FindAsync(id);
            if (existingTypeAudit == null)
            {
                return null;
            }

            existingTypeAudit.type = typeAudit.type;
            await _context.SaveChangesAsync();
            return existingTypeAudit;
        }

        public async Task<bool> DeleteTypeAudit(int id)
        {
            var typeAudit = await _context.typeAudit.FindAsync(id);
            if (typeAudit == null)
            {
                return false;
            }

            _context.typeAudit.Remove(typeAudit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
