namespace ITKANSys_api.Models.Dtos
{
    public class ReclamationDto

    {
        public int Id { get; set; }
        public string Objet { get; set; }
        public string Détail { get; set; }
        public string Analyse { get; set; }
        public string Status { get; set; }
        public int ReclamantID { get; set; }
        public int ResponsableID { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
