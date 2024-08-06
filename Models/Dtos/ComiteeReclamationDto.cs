namespace ITKANSys_api.Models.Dtos
{
    public class ComiteeReclamationDto
    {

        public int ReclamationID { get; set; }
        public int ConcernedID { get; set; }
        public DateTime CreationDate { get; set; }
        public List<int> UserIds { get; set; }
    }
}
