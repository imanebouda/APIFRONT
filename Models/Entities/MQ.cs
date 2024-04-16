using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ITKANSys_api.Models.Entities.Param;

namespace ITKANSys_api.Models.Entities
{
    public class MQ
    {
        public int ID { get; set; }
        public int SMQID { get; set; }
        public string Libelle { get; set; }
        public string Version { get; set; }
        public DateTime DateApplication { get; set; }
        public int Perime { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("SMQID")]
        public SMQ SMQ { get; set; }
    }
}
