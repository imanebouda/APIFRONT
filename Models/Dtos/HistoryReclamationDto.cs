using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class HistoryReclamationDto
    {

        public int ReclamationID { get; set; }
        public string Commentaire { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
