namespace ITKANSys_api.Models.Dtos
{
    public class FilterProcedureDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public string libelle { get; set; }
        public string virsion { get; set; }
        public DateTime? dateDebut { get; set; }
        public DateTime? dateFin { get; set; }
        public string field { get; set; }
        public string order { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
    }
}
