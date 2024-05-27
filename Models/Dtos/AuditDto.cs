
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Models.Dtos
{
    public class AuditDto
    {
        public string nomAudit { get; set; }
        public DateTime dateAudit { get; set; }
        public string status { get; set; }
        
        public string typeAudit { get; set; }
        public ICollection<Constat> Constats { get; set; }
        public ICollection<Processus> Processus { get; set; }

    }
}
