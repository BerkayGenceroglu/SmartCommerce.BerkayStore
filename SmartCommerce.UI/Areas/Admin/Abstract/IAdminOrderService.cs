using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface IAdminOrderService
    {
        Task<List<OrderDto>> GetAllAsync();

    }
}
