using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class ProgrammeAuditService :IProgrammeAudit
    {
        private readonly ApplicationDbContext _context;


        public ProgrammeAuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProgrammeAudit>> GetProgrammeAuditsForAudit(int auditId)
        {
            return await _context.ProgrammeAudit 
                .Where(pa => pa.AuditID == auditId)
                .ToListAsync();
        }



        public async Task<List<ProgrammeAudit>> GetProgrammeAudits()
        {
            return await _context.ProgrammeAudit.ToListAsync();
        }
    }
}
