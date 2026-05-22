using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();

    }
}
