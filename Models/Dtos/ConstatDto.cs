using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Models.Dtos
{
    public class ConstatDto
    {
        public int ID { get; set; }
        public string EcartTitle { get; set; }
        public string EcartType { get; set; }
        public Audit Audit { get; set; }
    }
}
