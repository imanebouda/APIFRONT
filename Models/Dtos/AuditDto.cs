
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Models.Dtos
{
    public class AuditDto
    {
        public int ID { get; set; }
        public string NomAudit { get; set; }
        public DateTime DateAudit { get; set; }
        public string status { get; set; }
        
        public string typeAudit { get; set; }
        public ICollection<Constat> Constats { get; set; }
        public ICollection<Processus> Processus { get; set; }

    }
}
