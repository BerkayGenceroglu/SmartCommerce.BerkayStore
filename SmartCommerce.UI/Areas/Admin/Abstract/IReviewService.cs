using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewListsAsync();
    }
}
