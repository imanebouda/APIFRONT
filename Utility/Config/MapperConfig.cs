using AutoMapper;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;

namespace ITKANSys_api.Utility.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Audit, AuditDto>().ReverseMap();
        }
    }
}
