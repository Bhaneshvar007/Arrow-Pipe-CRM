using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALDropDownMaster
    {
        List<DropDownMasterModel> GetDropDownValues(DropDownFilterModel dropDownFilterModel);

        List<DropDownMasterModel> GetDropDownValuesByCetagory(int categoryId, int companyId);
        DropDownMasterModel GetDropDownValueById(int id);

        ResponseModel AddDropDownValue(DropDownMasterModel objModel);

        ResponseModel UpdateDropDownValue(DropDownMasterModel objModel);

        ResponseModel DeleteDropDownValue(int id, int userId);
    }
}
