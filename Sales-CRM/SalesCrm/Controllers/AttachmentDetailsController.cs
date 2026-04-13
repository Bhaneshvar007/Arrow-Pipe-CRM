using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentDetailsController : ControllerBase
    {
        private readonly IDALAttachmentDetails _attachmentService;
        private readonly IConfiguration _config;

        public AttachmentDetailsController(IDALAttachmentDetails attachmentService, IConfiguration config)
        {
            _attachmentService = attachmentService;
            _config = config;
        }

        [HttpGet("GetAttachments")]
        public IActionResult GetAttachments(int PageID, int RelevantID, string PageRef)
        {
            int companyId = UserContext.GetCompanyId(User);

            var data = _attachmentService.GetAttachmentDetails(PageID, RelevantID, PageRef, companyId);
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

        [HttpGet("GetAttachmentList")]
        public IActionResult GetAttachmentList(int PageNumber, int PageSize)
        {
            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            RequestModel requestModel = new RequestModel
            {
                CompanyID = companyId,
                CompanyUserID = userId,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var data = _attachmentService.GetAttachmentList(requestModel);

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

        [HttpGet("GetAttachmentById")]
        public IActionResult GetAttachmentById(int id)
        {
            var data = _attachmentService.GetAttachmentDetailsById(id);

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

        [HttpPost("AddAttachment")]
        public IActionResult AddAttachment(AttachmentDetailsModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _attachmentService.AddAttachmentDetails(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("UpdateAttachment")]
        public IActionResult UpdateAttachment(AttachmentDetailsModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _attachmentService.UpdateAttachmentDetails(model);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteAttachment(int id)
        {
            int userId = UserContext.GetUserId(User);

            var response = _attachmentService.DeleteAttachmentDetails(id);

            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);

            return Ok(response);
        }



        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(
     List<IFormFile> files,
     [FromForm] int pageId,
     [FromForm] string pageRef,
     [FromForm] int relevantId)
        {
            ResponseModel finalResponse = new ResponseModel();
            /*
            var delRes = _attachmentService.DeleteAttachments(
                            pageId,
                            pageRef,
                            relevantId,
                            UserContext.GetCompanyId(User)
                    );

            if (!delRes.Status)
            {
                return BadRequest(delRes);  
            }
            */
            try
            {
                if (files == null || files.Count == 0)
                {
                    finalResponse.Status = false;
                    finalResponse.Message = "No files uploaded";
                    finalResponse.Type = "warning";
                    return BadRequest(finalResponse);
                }

                var basePath = _config["FileUploads:FilePath"];
                var safePageRef = pageRef.Replace(" ", "_");
                var uploadPath = Path.Combine(basePath, safePageRef);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var responses = new List<object>();

                foreach (var file in files)
                {
                    //var fileName = Path.GetFileName(file.FileName);
                    var fileName = Path.GetFileName(file.FileName).Replace(" ", "_");
                    var fullPath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var dbPath = "/FileUploads/" + safePageRef + "/" + fileName;

                    var model = new AttachmentDetailsModel
                    {
                        AttachmentName = fileName,
                        AttachmentType = Path.GetExtension(file.FileName),
                        AttachmentPath = dbPath,
                        PageID = pageId,
                        PageRef = pageRef,
                        RelevantID = relevantId,
                        CompanyID = UserContext.GetCompanyId(User),
                        CompanyUserID = UserContext.GetUserId(User),
                        CreatedDate = DateTime.Now
                    };

                    var response = _attachmentService.AddAttachmentDetails(model);

                    if (response.Type == "error")
                        return StatusCode(500, response);

                    if (response.Type == "warning")
                        return BadRequest(response);

                    responses.Add(new
                    {
                        AttachmentName = fileName,
                        AttachmentPath = dbPath,
                        Id = response.ID
                    });
                }

                finalResponse.Status = true;
                finalResponse.Message = "Files uploaded successfully";
                finalResponse.Data = responses;

                return Ok(finalResponse);
            }
            catch (Exception ex)
            {
                finalResponse.Status = false;
                finalResponse.Message = ex.Message;
                finalResponse.Type = "error";

                return StatusCode(500, finalResponse);
            }
        }
    }
}