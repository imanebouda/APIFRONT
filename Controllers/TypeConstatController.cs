using ITKANSys_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeConstatController : Controller
    {
        private readonly ITypeConstatServicecs _typeConstatService;

        public TypeConstatController(ITypeConstatServicecs typeConstatService)
        {
            _typeConstatService = typeConstatService;

        }
       
        [HttpGet]
        public async Task<ActionResult<List<TypeContat>>> GetAllTypeConstats()
        {
            var typeConstats = await _typeConstatService.GetAllTypeConstat();
            return Ok(typeConstats);
        }
    }
}
