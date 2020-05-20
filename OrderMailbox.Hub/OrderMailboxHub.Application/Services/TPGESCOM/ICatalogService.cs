using OrderMailboxHub.Application.Dtos.Catalog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.TPGESCOM
{
    public interface ICatalogService
    {
        Task<List<Product>> GetProductsByFilter(CatalogFilterDto filter);

        Task<List<PromotionDto>> GetPromotionsByFilter(PromotionFilterDto filter);
    }
}
