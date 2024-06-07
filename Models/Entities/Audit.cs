using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Audit
    {
        public int ID { get; set; }
        public string NomAudit { get; set; }
 
        public DateTime DateAudit { get; set; }
        public string status { get; set; }
        public string  description { get; set; }
        public string typeAudit { get; set; }
       

        // Association One-to-Many avec la classe Constat
        [InverseProperty("Audit")]
        public ICollection<Constat> Constats { get; set; }

        // Association One-to-Many avec la classe Processus
        [InverseProperty("Audit")]
        public ICollection<Processus> Processus { get; set; }



    }
}
