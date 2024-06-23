using ITKANSys_api.Models.Entities;
using ITKANSys_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class Check_listController : ControllerBase
    {
        private readonly ICheck_listService _checkListService;

        public Check_listController(ICheck_listService checkListService)
        {
            _checkListService = checkListService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Check_list>>> GetAllCheckListAudit()
        {
            var checkLists = await _checkListService.GetAllCheckListAudit();
            return Ok(checkLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Check_list>> GetCheckListAudit(int id)
        {
            var checkList = await _checkListService.GetCheckListAudit(id);

            if (checkList == null)
            {
                return NotFound();
            }

            return Ok(checkList);
        }

        [HttpPost, Produces("application/json")]
        public async Task<ActionResult<Check_list>> AddCheckListAudit(Check_list checkListAudit)
        {
            var createdCheckList = await _checkListService.AddCheckListAudit(checkListAudit);
            return CreatedAtAction(nameof(GetCheckListAudit), new { id = createdCheckList.Id }, createdCheckList);

           
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Check_list>>> UpdateCheckListAudit(int id, Check_list request)
        {
            var updatedCheckLists = await _checkListService.UpdateCheckListAudit(id, request);

            if (updatedCheckLists == null)
            {
                return NotFound();
            }

            return Ok(updatedCheckLists);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Check_list>>> DeleteCheckListAudit(int id)
        {
            var deletedCheckLists = await _checkListService.DeleteCheckListAudit(id);

            if (deletedCheckLists == null)
            {
                return NotFound();
            }

            return Ok(deletedCheckLists);
        }
    }
}
