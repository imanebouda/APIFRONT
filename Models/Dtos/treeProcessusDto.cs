using ITKANSys_api.Models.Entities.Param;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class treeProcessusDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string NamePilote { get; set; }
        public string NameCoPilote { get; set; }
        public List<ProcedureDto> Procedures { get; set; }
        public List<ProcesDocumentsDto> ProcesDocuments { get; set; }
        public List<ProcesObjectifsDto> ProcesObjectifs { get; set; }
    }
}
