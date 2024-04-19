using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Services
{
  
    public class AuditService : IAuditService
    {

        private static List<Audit> audits = new List<Audit>
    { new Audit
        {
            ID = 1,
            NomAudit = "audit Informatique",
            status = "en cours",
            typeAudit = "informatique"
        },
        new Audit
        {
            ID = 2,
            NomAudit = "audit Informatique",
            status = "en cours",
            typeAudit = "informatique"
        }


    };
        public List<Audit> AddAudit(Audit audit)
        {
            audits.Add(audit);

            return audits;
        }

        public List<Audit>? DeleteAudit(int id)
        {
            var audit = audits.Find(x => x.ID == id);
            if (audit is null)
                return null;


            audits.Remove(audit);
            return audits;
        }

        public List<Audit> GetAllAudit()
        {
            return audits;
        }

        public Audit GetAudit(int id)
        {
            var audit = audits.Find(x => x.ID == id);
            if (audit is null)
                return null;

            return audit;
        }

        public List<Audit>? UpdateAudit(int id, Audit request)
        {
            var audit = audits.Find(x => x.ID == id);
            if (audit is null)
                return null;
            audit.NomAudit = request.NomAudit;
            audit.status = request.status;
            audit.description = request.description;
            audit.typeAudit = request.typeAudit;


            return audits;
            
            
        }
    }
}
