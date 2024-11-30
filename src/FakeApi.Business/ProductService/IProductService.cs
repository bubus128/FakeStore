using FakeStore.Data.Models;

namespace FakeApi.Business.ProductService
{
    internal interface IProductService
    {
        Task<(List<Product>, int)> GetProductsAsync(int page, int pageSize);
    }
}
