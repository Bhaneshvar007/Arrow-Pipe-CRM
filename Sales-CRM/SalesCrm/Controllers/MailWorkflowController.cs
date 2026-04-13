using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Reflection;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailWorkflowController : ControllerBase
    {
        private readonly IDALMailWorkflow _service;

        public MailWorkflowController(IDALMailWorkflow service)
        {
            _service = service;
        }

        [HttpGet("Get")]
        public IActionResult Get(int PageNumber,int PageSize)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            RequestModel req = new RequestModel
            {
                CompanyID = companyId,
                CompanyUserID = userId,
                PageNumber = PageNumber,
                PageSize = PageSize
            };


            var data = _service.GetWorkflow(req);

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

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

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

        [HttpPost("Add")]
        public IActionResult Add(MailWorkflowModel model)
        {
            model.CompanyId = UserContext.GetCompanyId(User);
            model.CreatedBy = UserContext.GetUserId(User);


            var response = _service.Add(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("Update")]
        public IActionResult Update(MailWorkflowModel model)
        {
            model.CompanyId = UserContext.GetCompanyId(User);
            model.UpdatedBy = UserContext.GetUserId(User);


            var response = _service.Update(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            int userId = UserContext.GetUserId(User);

            var response = _service.Delete(id, userId);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }
    }
}
