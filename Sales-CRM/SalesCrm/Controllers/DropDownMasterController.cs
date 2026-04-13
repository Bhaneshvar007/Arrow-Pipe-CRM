using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownMasterController : ControllerBase
    {
        private readonly IDALDropDownMaster _dropDownService;

        public DropDownMasterController(IDALDropDownMaster dropDownService)
        {
            _dropDownService = dropDownService;
        }

        [HttpPost("GetValues")]
        public IActionResult GetValues(DropDownFilterModel dropDownFilterModel)
        {
            dropDownFilterModel.CompanyID = UserContext.GetCompanyId(User);
            dropDownFilterModel.CompanyUserID = UserContext.GetUserId(User);


            var data = _dropDownService.GetDropDownValues(dropDownFilterModel);

            if (data == null || !data.Any())
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Data not found",
                });
            }

            return Ok(data);
        }

        [HttpGet("GetValuesByCetegory")]
        public IActionResult GetValuesByCetegory(int categoryId)
        {
            int companyId = UserContext.GetCompanyId(User);

            var data = _dropDownService.GetDropDownValuesByCetagory(categoryId, companyId);
            if (data == null || !data.Any())
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Data not found",
                });
            }

            return Ok(data);
        }

        [HttpGet("GetValueById")]
        public IActionResult GetValueById(int id)
        {
            var data = _dropDownService.GetDropDownValueById(id);

            if (data == null)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Data not found",
                });
            }

            return Ok(data);
        }

        [HttpPost("AddValue")]
        public IActionResult AddValue(DropDownMasterModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.AddedBy = UserContext.GetUserId(User);

            var response = _dropDownService.AddDropDownValue(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("UpdateValue")]
        public IActionResult UpdateValue(DropDownMasterModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.UpdatedBy = UserContext.GetUserId(User);

            var response = _dropDownService.UpdateDropDownValue(model);
            if (response.Type == "error")
                return StatusCode(500, response);  

            if (response.Type == "warning")
                return BadRequest(response);


            return Ok(response);
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteValue(int DropDown_id)
        {
            int userId = UserContext.GetUserId(User);

            var response = _dropDownService.DeleteDropDownValue(DropDown_id, userId);


            if (response.Type == "error")
                return StatusCode(500, response); 

            if (response.Type == "warning")
                return BadRequest(response);


            return Ok(response);
        }
    }
}