using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class ComiteeReclamation
    {
        public int ID { get; set; }

        public int ReclamationID { get; set; }
        public Reclamation Reclamation { get; set; }

        public int ConcernedID { get; set; }
        [ForeignKey("ConcernedID")]

        public User ConcernedUser { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
