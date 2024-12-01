using FakeStore.ApiClient.Models;

namespace FakeStore.ApiCLient.FakeStoreApiClient;

public interface IFakeStoreApiClient
{
    public Task<List<string>> GetCategoriesAsync();
    public Task<List<Product>> GetProductsByCategoryAsync(string category);
    public Task<Cart> GetCartAsyncByUserId(int userId);
    public Task<Cart> GetCartAsyncById(int cartId);
    public Task AddToCartAsync(int userId, int productId);
    public Task<Product> GetProductById(int id);
    public Task RemoveFromCartAsync(int cartId, int productId);
}