using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;
using System.Text.Json;

namespace SalesCrm.Services.DAL
{
    public class DALLeads:IDALLeads
    {
        private string _connectionString;
        private readonly IDALClient _customerService;
        private readonly IDALAttachmentDetails _attachmentService;

        public DALLeads(IDALClient dALClient, IDALAttachmentDetails dALAttachmentDetails)
        {
            _connectionString = CommonHelper.GetConnectionString();
            this._customerService = dALClient;
            this._attachmentService = dALAttachmentDetails;
        }


        //public List<LeadsEnqModel> GetAllLeadEnquiry(RequestModel request)
        //{
        //    List<LeadsEnqModel> data = new List<LeadsEnqModel>();

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            data = con.Query<LeadsEnqModel>(
        //                "sp_get_all_lead_enquiry_list",
        //                new
        //                {
        //                    company_id = request.CompanyID,
        //                    company_user_id = request.CompanyUserID,
        //                    page_number = request.PageNumber,
        //                    page_size = request.PageSize
        //                },
        //                commandType: CommandType.StoredProcedure
        //            ).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.WriteLog(ex.Message);
        //    }

        //    return data;
        //}


        public List<LeadsEnqModel> GetAllLeadEnquiry(int companyId, int userId, int pageNumber, int pageSize, string source = null, string enquiryType = null,
            string assignedTo = null, string status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<LeadsEnqModel> data = new List<LeadsEnqModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadsEnqModel>(
                        "sp_get_all_lead_enquiry_list_by_filter",
                        new
                        {
                            company_id = companyId,
                            company_user_id = userId,
                            page_number = pageNumber,
                            page_size = pageSize,
                            source = source,
                            enquiry_type = enquiryType,
                            assigned_to = assignedTo,
                            status = status,
                            from_date = fromDate,
                            to_date = toDate
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }


        public LeadsEnqModel GetLeadEnquirybyEnqId(string enqId)
        {
            LeadsEnqModel data = new LeadsEnqModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadsEnqModel>(
                        "sp_get_lead_enquiry_byenqId",
                        new { enquiryId = enqId },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    // Attachment JSON → List
                    if (data != null && !string.IsNullOrEmpty(data.Attachments))
                    {
                        data.DocumentList = JsonSerializer.Deserialize<List<EnquiryDocumentsModel>>(
                            data.Attachments,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                    }
                    else
                    {
                        data.DocumentList = new List<EnquiryDocumentsModel>();
                    }

                    // SKU JSON → List 
                    if (!string.IsNullOrEmpty(data.SKUItems))
                    {
                        data.SkuList = JsonSerializer.Deserialize<List<EnquirySkuModel>>(
                            data.SKUItems,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                    }
                    else
                    {
                        data.SkuList = new List<EnquirySkuModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }


        public ResponseModel ConvertEnqToLeadbyEnqIds(ConvertEnquiryRequest request)
        {
            ResponseModel response = new ResponseModel();

            if (request == null || request.EnquiryIds == null || !request.EnquiryIds.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No enquiries selected for conversion."
                };
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@EnquiryIds", string.Join(",", request.EnquiryIds));
                        parameters.Add("@LeadConvertedBy", request.LeadConvertedBy);
                        parameters.Add("@LeadConvertedOn", CommonHelper.GetDate);

                        parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                        con.Execute("sp_convert_enquiry_to_lead", parameters, transaction: tran, commandType: CommandType.StoredProcedure);

                        int affectedRows = parameters.Get<int>("@ReturnVal");

                        if (affectedRows > 0)
                        {
                            tran.Commit();
                            response.Status = true;
                            response.Message = $"{affectedRows} enquiry(s) converted to lead successfully.";
                            response.Type = "success";
                        }
                        else
                        {
                            tran.Rollback();
                            response.Status = false;
                            response.Message = "Error!! Failed to convert enquiries.";
                            response.Type = "warning";
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); 
                        CommonHelper.WriteLog(ex.Message);

                        response.Status = false;
                        response.Message = "An error occurred while converting enquiries. Please try again.";
                        response.Type = "error";
                    }
                }
            }

            return response;
        }


        public ResponseModel DeleteEnqbyEnqIds(List<string> enquiryIds)
        {
            ResponseModel response = new ResponseModel();

            if (enquiryIds == null || !enquiryIds.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No enquiries selected for deletion.",
                    Type = "warning"

                };
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        string enqIds = string.Join(",", enquiryIds);

                        int affectedRows = con.QueryFirstOrDefault<int>(
                            "sp_delete_enquiry_by_ids",
                            new { EnquiryIds = enqIds },
                            transaction: tran, 
                            commandType: CommandType.StoredProcedure
                        );

                        if (affectedRows > 0)
                        {
                            tran.Commit(); 
                            response.Status = true;
                            response.Message = $"{affectedRows} enquiry(s) deleted successfully.";
                            response.Type = "success";

                        }
                        else
                        {
                            tran.Rollback(); 
                            response.Status = false;
                            response.Message = "Failed to delete enquiries.";
                            response.Type = "warning";

                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); 
                        CommonHelper.WriteLog(ex.Message);

                        response.Status = false;
                        response.Message = "Error while deleting enquiries.";
                        response.Type = "error";

                    }
                }
            }

            return response;
        }


        public ResponseModel UpdateEnquiryList(List<UpdateEnquiryItem> request)
        {
            ResponseModel response = new ResponseModel();          

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in request)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_update_enquiry_assign_and_status", con, tran)) // ✅ attach transaction
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@EnquiryId", item.EnquiryId);
                                cmd.Parameters.AddWithValue("@AssignedTo", (object)item.AssignedTo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@EnquiryStatus", (object)item.EnquiryStatus ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedBy", item.UpdatedBy);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit(); 

                        response.Status = true;
                        response.Message = "Enquiries updated successfully.";
                        response.Type = "success";

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); 

                        response.Status = false;
                        response.Message = ex.Message;
                        response.Type = "error";

                    }
                }
            }

            return response;
        }


        public ResponseModel SaveNewEnquiry(LeadsEnqModel model)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (var tran = con.BeginTransaction())
                    {
                        try
                        {

                            // existing logic compare 
                            var existingCust = _customerService.GetClientById(model.CustomerId);

                            if (existingCust == null || existingCust.CustomerID <= 0)
                            {
                                var cust_info_model = DalServiceHelper.CustomerinformationModelBinding(model);
                                response = _customerService.AddClient(cust_info_model);
                                if (response.Type == "error" || response.Type == "warning")
                                {
                                    return response;
                                }

                            }


                            DynamicParameters enqParam = new DynamicParameters();
                            enqParam.Add("@enquiry_source", model.EnquirySource);
                            enqParam.Add("@enquiry_source_details", model.EnquirySourceDetail);
                            enqParam.Add("@received_on", model.ReceivedOn);
                            enqParam.Add("@enquiry_status", model.EnquiryStatus);
                            enqParam.Add("@enquiry_type", model.EnquiryType);
                            enqParam.Add("@delivery_location", model.DeliveryLocation);
                            enqParam.Add("@remark", model.Remark);
                            enqParam.Add("@description", model.EnquiryDescription);

                            enqParam.Add("@from_mail", model.CustomerEmail);
                            enqParam.Add("@name", model.CustomerName);
                            enqParam.Add("@domain", CustomeValidatore.ExtractDomain(model.CustomerEmail));

                            enqParam.Add("@squdetails", model.SKUItems);

                            enqParam.Add("@company_id", model.CompanyID);
                            enqParam.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                            con.Execute("sp_insert_enquiry_details", enqParam, transaction: tran, commandType: CommandType.StoredProcedure);

                            int enqId = enqParam.Get<int>("@id");



                            DynamicParameters assignParam = new DynamicParameters();
                            assignParam.Add("@enqId", enqId);
                            assignParam.Add("@assign_to", model.AssignTo);
                            assignParam.Add("@company_id", model.CompanyID);
                            assignParam.Add("@created_by", model.CompanyUserID);
                            assignParam.Add("@ass_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                            con.Execute("sp_insert_enquiry_assignment", assignParam, transaction: tran, commandType: CommandType.StoredProcedure);


                            tran.Commit();


                            //var doc_list = DalServiceHelper.AttachmentModelBinding(model);

                            //if (doc_list != null && doc_list.Count > 0)
                            //{
                            //    foreach (var item in doc_list)
                            //    {
                            //        item.RelevantID = enqId;
                            //        var res = _attachmentService.AddAttachmentDetails(item);
                            //    }

                            //    response.Status = true;
                            //    response.Message = "Enquiry saved successfully";
                            //    response.Type = "success";

                            //}

                            response.ID = enqId;
                            response.Status = true;
                            response.Message = "Enquiry saved successfully";
                            response.Type = "success";
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            response.Status = false;
                            response.Type = "error";
                            response.Message = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }


        public ResponseModel UpdateEnquiry(LeadsEnqModel model)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (var tran = con.BeginTransaction())
                    {
                        try
                        {
                            //  Customer Update / Insert logic same
                            var cust_info_model = DalServiceHelper.CustomerinformationModelBinding(model);
                            if (model.CustomerId > 0)
                            {
                                response = _customerService.UpdateClient(cust_info_model);
                                if (response.Type == "error"|| response.Type == "warning")
                                {
                                    return response;
                                }
                            }

                            else
                            {
                                response = _customerService.AddClient(cust_info_model);
                                if (response.Type == "error"|| response.Type == "warning")
                                {
                                    return response;
                                }
                            }

                                // Enquiry Update
                            DynamicParameters enqParam = new DynamicParameters();
                            enqParam.Add("@id", model.EnqRefId);  
                            enqParam.Add("@enquiry_source", model.EnquirySource);
                            enqParam.Add("@enquiry_source_details", model.EnquirySourceDetail);
                            enqParam.Add("@received_on", model.ReceivedOn);
                            enqParam.Add("@enquiry_status", model.EnquiryStatus);
                            enqParam.Add("@enquiry_type", model.EnquiryType);
                            enqParam.Add("@delivery_location", model.DeliveryLocation);
                            enqParam.Add("@remark", model.Remark);
                            enqParam.Add("@description", model.EnquiryDescription);

                            enqParam.Add("@from_mail", model.CustomerEmail);
                            enqParam.Add("@name", model.CustomerName);
                            enqParam.Add("@domain", CustomeValidatore.ExtractDomain(model.CustomerEmail));

                            enqParam.Add("@squdetails", model.SKUItems);
                            enqParam.Add("@company_id", model.CompanyID);

                            con.Execute("sp_update_enquiry_details", enqParam, transaction: tran, commandType: CommandType.StoredProcedure);

                            //  Assignment Update
                            DynamicParameters assignParam = new DynamicParameters();
                            assignParam.Add("@enqId", model.EnqRefId);
                            assignParam.Add("@assign_to", model.AssignTo);
                            assignParam.Add("@company_id", model.CompanyID);
                            assignParam.Add("@updated_by", model.CompanyUserID);

                            con.Execute("sp_update_enquiry_assignment", assignParam, transaction: tran, commandType: CommandType.StoredProcedure);

                            tran.Commit();

                            //  Attachments (replace / add logic)
                            /*var doc_list = DalServiceHelper.AttachmentModelBinding(model);

                            if (doc_list != null && doc_list.Count > 0)
                            {
                                foreach (var item in doc_list)
                                {
                                    item.RelevantID = model.EnqRefId;
                                    response = _attachmentService.UpdateAttachmentDetails(item);
                                }
                            }*/
                            response.ID = model.EnqRefId;
                            response.Status = true;
                            response.Message = "Enquiry updated successfully";
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            response.Status = false;
                            response.Type = "error";
                            response.Message = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Type = "error";
                response.Message = ex.Message;
            }

            return response;
        }



        //public List<LeadModel> GetAllLeadList(RequestModel request)
        //{
        //    List<LeadModel> data = new List<LeadModel>();

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            data = con.Query<LeadModel>(
        //                "sp_get_all_lead_list",
        //                new
        //                {
        //                    company_id = request.CompanyID,
        //                    company_user_id = request.CompanyUserID,
        //                    page_number = request.PageNumber,
        //                    page_size = request.PageSize
        //                },
        //                commandType: CommandType.StoredProcedure
        //            ).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.WriteLog(ex.Message);
        //    }

        //    return data;
        //}

        public List<LeadModel> GetAllLeadList(int companyId, int userId, int pageNumber, int pageSize, string source = null, string leadType = null,
        string assignedTo = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<LeadModel> data = new List<LeadModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadModel>(
                        "sp_get_all_lead_list_by_filter",
                        new
                        {
                            company_id = companyId,
                            company_user_id = userId,
                            page_number = pageNumber,
                            page_size = pageSize,
                            source = source,
                            lead_type = leadType,
                            assigned_to = assignedTo,                    
                            from_date = fromDate,
                            to_date = toDate
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }


        public LeadModel GetLeadbyLeadId(string leadId)
        {
            LeadModel data = new LeadModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadModel>(
                        "sp_get_lead_byLeadId",
                        new { leadId = leadId },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    // Attachment JSON → List
                    if (data != null && !string.IsNullOrEmpty(data.Attachments))
                    {
                        data.DocumentList = JsonSerializer.Deserialize<List<LeadDocumentsModel>>(
                            data.Attachments,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                    }
                    else
                    {
                        data.DocumentList = new List<LeadDocumentsModel>();
                    }

                    // SKU JSON → List 
                    if (!string.IsNullOrEmpty(data.SKUItems))
                    {
                        data.SkuList = JsonSerializer.Deserialize<List<LeadSkuModel>>(
                            data.SKUItems,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                    }
                    else
                    {
                        data.SkuList = new List<LeadSkuModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }


        public ResponseModel DeleteLeadbyLeadIds(List<string> LeadIds)
        {
            ResponseModel response = new ResponseModel();

            if (LeadIds == null || !LeadIds.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No lead selected for deletion.",
                    Type = "warning"

                };
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        string ldIds = string.Join(",", LeadIds);

                        int affectedRows = con.QueryFirstOrDefault<int>(
                            "sp_delete_lead_by_ids",
                            new { leadIds = ldIds },
                            transaction: tran,
                            commandType: CommandType.StoredProcedure
                        );

                        if (affectedRows > 0)
                        {
                            tran.Commit();
                            response.Status = true;
                            response.Message = $"{affectedRows} Lead(s) deleted successfully.";
                            response.Type = "success";

                        }
                        else
                        {
                            tran.Rollback();
                            response.Status = false;
                            response.Message = "Failed to delete Lead.";
                            response.Type = "warning";

                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        CommonHelper.WriteLog(ex.Message);

                        response.Status = false;
                        response.Message = "Error while deleting Leads.";
                        response.Type = "error";

                    }
                }
            }

            return response;
        }


        public ResponseModel UpdateLeadList(List<UpdateLeadItem> request)
        {
            ResponseModel response = new ResponseModel();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in request)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_update_lead_assign_and_status", con, tran)) 
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@LeadId", item.LeadId);
                                cmd.Parameters.AddWithValue("@AssignedTo", (object)item.AssignedTo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LeadStatus", (object)item.LeadStatus ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedBy", item.UpdatedBy);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();

                        response.Status = true;
                        response.Message = "Leads updated successfully.";
                        response.Type = "success";

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        response.Status = false;
                        response.Message = ex.Message;
                        response.Type = "error";

                    }
                }
            }

            return response;
        }







    }
}
