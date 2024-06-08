using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Constat
    {
        public int ID { get; set; }
        public string constat { get; set; }
       

        public int typeConstatId { get; set; }

        // Propriété de navigation
        [ForeignKey("typeConstatId")]
        public TypeContat typeAudit { get; set; }
    }
}
