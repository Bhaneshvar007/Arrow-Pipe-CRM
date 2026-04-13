using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALDropDownCategory
    {
        List<DropDownCategoryModel> GetCategories(RequestModel request);

        DropDownCategoryModel GetCategoryById(int id);

        ResponseModel AddCategory(DropDownCategoryModel objModel);

        ResponseModel UpdateCategory(DropDownCategoryModel objModel);

        ResponseModel DeleteCategory(int id, int userId);
    }
}
