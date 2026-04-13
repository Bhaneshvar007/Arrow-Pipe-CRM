using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownCategoryController : ControllerBase
    {
        private readonly IDALDropDownCategory _service;

        public DropDownCategoryController(IDALDropDownCategory service)
        {
            _service = service;
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories(int PageNumber, int PageSize)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            RequestModel request = new RequestModel
            {
                CompanyID = companyId,
                CompanyUserID = userId,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var data = _service.GetCategories(request);

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

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById(int id)
        {
            var data = _service.GetCategoryById(id);
            if (data == null )
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Data not found",
                });

            }
            return Ok(data);
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(DropDownCategoryModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _service.AddCategory(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory(DropDownCategoryModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _service.UpdateCategory(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int CategoryID)
        {
            int userId = UserContext.GetUserId(User);

            var response = _service.DeleteCategory(CategoryID, userId);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")

                return BadRequest(response);
            return Ok(response);
        }
    }
}