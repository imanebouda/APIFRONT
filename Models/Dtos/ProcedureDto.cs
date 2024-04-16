using static ITKANSys_api.Data.Services.ProcessusService;
using ITKANSys_api.Models.Dtos;
namespace ITKANSys_api.Models.Dtos
{
    public class ProcedureDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime CreationDate { get; set; }
        public Processus Processus { get; set; }
        public List<ProcDocumentsDto> ProcDocuments { get; set; }
        public List<ProcObjectifsDto> ProcObjectifs { get; set; }
    }
}
