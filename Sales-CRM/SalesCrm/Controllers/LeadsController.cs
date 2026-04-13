using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.DAL;
using SalesCrm.Services.IDAL;
using System.ComponentModel.Design;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly IDALLeads _leads;

        public LeadsController(IDALLeads leads)
        {
            this._leads = leads;
        }

        #region Enquiry Module

        //[HttpPost("GetAllLeadEnquiry")]
        //public IActionResult GetLeadEnquiry(int PageNumber, int PageSize)
        //{
        //    int companyId = UserContext.GetCompanyId(User);
        //    int userId = UserContext.GetUserId(User);

        //    EnqListModel request = new EnqListModel
        //    {
        //        CompanyID = companyId,
        //        CompanyUserID = userId,
        //        PageNumber = PageNumber,
        //        PageSize = PageSize
        //    };
        //    var data = _leads.GetAllLeadEnquiry(request);
        //    return Ok(data);
        //}

        [HttpPost("GetAllLeadEnquiry")]
        public IActionResult GetLeadEnquiry(int PageNumber, int PageSize, string source = null, string enquiryType = null,
            string assignedTo = null,string status = null, DateTime? fromDate = null,DateTime? toDate = null)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            var data = _leads.GetAllLeadEnquiry(companyId, userId, PageNumber, PageSize, source,
                enquiryType, assignedTo,status, fromDate,toDate);


            // var data = _leads.GetAllLeadEnquiry(request);
            return Ok(data);
        }



        [HttpGet("GetLeadEnquirybyEnqId")]
        public IActionResult GetLeadEnquirybyEnqId(string enqId)
        {
            var data = _leads.GetLeadEnquirybyEnqId(enqId);
            return Ok(data);
        }


        [HttpPost("ConvertEnqToLeadbyEnqIds")]
        public IActionResult ConvertEnqToLeadbyEnqIds([FromBody] ConvertEnquiryRequest request)
        {
            var data = _leads.ConvertEnqToLeadbyEnqIds(request);

            if (data.Type == "error")
                return StatusCode(500, data);

            if (data.Type == "warning")
                return BadRequest(data);
            return Ok(data);
        }


        [HttpPost("DeleteEnqbyEnqIds")]
        public IActionResult DeleteEnqbyEnqIds(List<string> EnquiryIds)
        {
            var response = _leads.DeleteEnqbyEnqIds(EnquiryIds);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("UpdateEnquiryList")]
        public IActionResult UpdateEnquiryList([FromBody] List<UpdateEnquiryItem> request)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);
            
            foreach (var item in request)
            {
                item.UpdatedBy = userId;
                item.CompanyId = companyId;
            }

            var response = _leads.UpdateEnquiryList(request);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("AddNewEnquiry")]
        public IActionResult AddNewEnquiry([FromBody] LeadsEnqModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var res = _leads.SaveNewEnquiry(model);
            if (res.Type == "error")
                return StatusCode(500, res);

            if (res.Type == "warning")
                return BadRequest(res);

            return Ok(res);
        }


        [HttpPost("UpdateEnquiry")]
        public IActionResult UpdateEnquiry([FromBody] LeadsEnqModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var res = _leads.UpdateEnquiry(model);

            if (res.Type == "error")
                return StatusCode(500, res);

            if (res.Type == "warning")
                return BadRequest(res);

            return Ok(res);
        }

        #endregion



        #region Lead Module

        //public IActionResult GetAllLeadList(int PageNumber, int PageSize)
        //{
        //    int companyId = UserContext.GetCompanyId(User);
        //    int userId = UserContext.GetUserId(User);

        //    RequestModel request = new RequestModel
        //    {
        //        CompanyID = companyId,
        //        CompanyUserID = userId,
        //        PageNumber = PageNumber,
        //        PageSize = PageSize
        //    };
        //    var data = _leads.GetAllLeadList(request);
        //    return Ok(data);
        //}


        [HttpPost("GetAllLeadList")]
        public IActionResult GetAllLeadList(int PageNumber, int PageSize, string source = null, string leadType = null,
           string assignedTo = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            var data = _leads.GetAllLeadList(companyId, userId, PageNumber, PageSize, source,
                leadType, assignedTo, fromDate, toDate);


            return Ok(data);
        }


        [HttpGet("GetLeadbyLeadId")]
        public IActionResult GetLeadbyLeadId(string leadId)
        {
            var data = _leads.GetLeadbyLeadId(leadId);
            return Ok(data);
        }


        [HttpPost("DeleteLeadbyLeadIds")]
        public IActionResult DeleteLeadbyLeadIds(List<string> LeadIds)
        {
            var response = _leads.DeleteLeadbyLeadIds(LeadIds);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("UpdateLeadList")]
        public IActionResult UpdateLeadList([FromBody] List<UpdateLeadItem> request)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            foreach (var item in request)
            {
                item.UpdatedBy = userId;
                item.CompanyId = companyId;
            }

            var response = _leads.UpdateLeadList(request);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }



        #endregion

    }
}
