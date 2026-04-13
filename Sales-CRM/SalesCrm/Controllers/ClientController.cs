using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IDALClient _customerService;

        public ClientController(IDALClient customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(int PageNumber, int PageSize)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            RequestModel reqmodel = new RequestModel
            {
                CompanyID = companyId,
                CompanyUserID = userId,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var data = _customerService.GetClient(reqmodel);
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

        [Authorize]
        [HttpGet("GetCustomerById")]
        public IActionResult GetCustomerById(int customer_id)
        {
            var data = _customerService.GetClientById(customer_id);

            return Ok(data);
        }

        [Authorize]
        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(CustomerDataModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _customerService.AddClient(model);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("UpdateCustomer")]
        public IActionResult UpdateCustomer(CustomerDataModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);
            model.LastUpdatedBy = UserContext.GetUserId(User);

            var response = _customerService.UpdateClient(model);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomer(int customer_id)
        {
            int userId = UserContext.GetUserId(User);

            var response = _customerService.DeleteClient(customer_id, userId);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }


        [Authorize]
        [HttpGet("GetCustomerBySuggestion")]
        public IActionResult GetCustomerBySuggestion(string customername)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);



            var data = _customerService.GetClientSuggestion(companyId, customername);
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

    }
}