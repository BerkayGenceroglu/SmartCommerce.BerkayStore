using SmartCommerce.UI.Areas.Admin.Dtos;

namespace SmartCommerce.UI.Areas.Admin.Abstract
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetAllAsync();

    }
}
