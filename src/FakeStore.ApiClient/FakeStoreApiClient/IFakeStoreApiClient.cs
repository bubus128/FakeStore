using FakeStore.Data.Models;

namespace FakeStore.ApiCLient.FakeStoreApiClient
{
    public interface IFakeStoreApiClient
    {
        Task<List<Product>> GetProductsAsync();
    }
}
