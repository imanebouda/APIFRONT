using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;




namespace ITKANSys_api.Services
{
    public class SiteAuditService : ISiteAuditService
    {
        private readonly ApplicationDbContext _context;

        public SiteAuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SiteAudit>> getAllSiteAudits()
        {
            var siteAudits = await _context.SiteAudits.ToListAsync();
            return siteAudits;
        }

        public async Task<List<SiteAudit>?> addSiteAudit(SiteAudit nSiteAudit)
        {


            _context.SiteAudits.Add(nSiteAudit);
            await _context.SaveChangesAsync();

            return await getAllSiteAudits(); // Retourner la liste mise à jour des SiteAudits
        }

        public async Task<List<SiteAudit>?> updateSiteAudit(int id, SiteAudit request)
        {

            var siteAudit = await _context.SiteAudits.FindAsync(id);
            if (siteAudit == null)
                return null;

            siteAudit.name = request.name;
            siteAudit.address = request.address;
            siteAudit.city = request.city;

            await _context.SaveChangesAsync();

            return await getAllSiteAudits(); // Retourner la liste mise à jour des SiteAudits
        }

        public async Task<List<SiteAudit>?> deleteSiteAudit(int id)
        {
            var siteAudit = await _context.SiteAudits.FindAsync(id);
            if (siteAudit == null)
                return null;

            _context.SiteAudits.Remove(siteAudit);
            await _context.SaveChangesAsync();

            return await getAllSiteAudits(); // Retourner la liste mise à jour des SiteAudits
        }

        public async Task<SiteAudit?> getSiteAudit(int id)
        {
            var siteAudit = await _context.SiteAudits.FindAsync(id);
            return siteAudit;
        }


    }
}
