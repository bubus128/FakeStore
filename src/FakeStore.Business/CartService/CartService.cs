using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;

namespace FakeStore.Business.CartService;

public class CartService(IFakeStoreApiClient apiClient, ILogger<CartService> logger) : ICartService
{
	/// <summary>
	/// Gets users cart by user id
	/// </summary>
	/// <param name="userId">User id</param>
	/// <returns>Users cart</returns>
	public async Task<Cart> GetCartAsync(int userId)
	{
		try
		{
			return await apiClient.GetCartAsyncByUserId(userId);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while retrieving the cart");
			throw;
		}
	}

	/// <summary>
	/// Adds product with given id to the cart of given id.
	/// </summary>
	/// <param name="cartId">Cart id</param>
	/// <param name="productId">Product id</param>
	public async Task AddToCartAsync(int userId, int productId)
	{
		try
		{
			var cart = await apiClient.GetCartAsyncByUserId(userId);
			await apiClient.AddToCartAsync(cart.Id, productId);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, $"An error occurred while adding product ID {productId} to users {userId} cart");
			throw;
		}
	}

	/// <summary>
	/// Removes one instance of a product to the cart.
	/// </summary>
	/// <param name="cartId">Cart id</param>
	/// <param name="productId">Product id</param>
	public async Task RemoveFromCartAsync(int userId, int productId)
	{
		try
		{
			var cart = await apiClient.GetCartAsyncByUserId(userId);
			await apiClient.RemoveFromCartAsync(cart.Id, productId);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, $"An error occurred while removing product ID {productId} from users {userId} cart");
			throw;
		}
	}
}