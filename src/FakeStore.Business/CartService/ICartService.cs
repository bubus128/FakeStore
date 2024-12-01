using FakeStore.ApiClient.Models;

namespace FakeStore.Business.CartService;

public interface ICartService
{
    public Task AddToCartAsync(int userId, int productId);

    public Task<Cart> GetCartAsync(int userId);

    public Task RemoveFromCartAsync(int userId, int productId);
}