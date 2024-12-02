using System.Text;
using System.Text.Json;
using FakeStore.ApiClient.Models;
using Microsoft.Extensions.Logging;

namespace FakeStore.ApiCLient.FakeStoreApiClient;

public class FakeStoreApiClient(HttpClient httpClient, ILogger<FakeStoreApiClient> logger) : IFakeStoreApiClient
{
    /// <summary>
    /// Gets all categories from FakeStoreApi
    /// </summary>
    /// <returns>List of all categories</returns>
    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("https://fakestoreapi.com/products/categories").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch categories. HTTP Status Code: {StatusCode}", response.StatusCode);
                return new(); // Return an empty list if request fails
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<string>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching categories.");
            throw;
        }
    }

    /// <summary>
    /// Get all products in given category
    /// </summary>
    /// <param name="category">Name of the category</param>
    /// <returns>List of products in given category</returns>
    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://fakestoreapi.com/products/category/{category}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch products for category '{Category}'. HTTP Status Code: {StatusCode}", category, response.StatusCode);
                return new();
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching products for category '{Category}'.", category);
            throw;
        }
    }

    /// <summary>
    /// Gets users cart by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Users cart</returns>
    public async Task<Cart> GetCartAsyncByUserId(int userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://fakestoreapi.com/carts/user/{userId}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch cart for user '{UserId}'. HTTP Status Code: {StatusCode}", userId, response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<Cart>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }).FirstOrDefault();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching cart for user '{UserId}'.", userId);
            throw;
        }
    }

    /// <summary>
    /// Gets cart by id
    /// </summary>
    /// <param name="cartId">Cart id</param>
    /// <returns>Cart</returns>
    public async Task<Cart> GetCartAsyncById(int cartId)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://fakestoreapi.com/carts/{cartId}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch cart by ID '{CartId}'. HTTP Status Code: {StatusCode}", cartId, response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<Cart>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching cart by ID '{CartId}'.", cartId);
            throw;
        }
    }

    /// <summary>
    /// Adds product with given id to the cart of given id.
    /// </summary>
    /// <param name="cartId">Cart id</param>
    /// <param name="productId">Product id</param>
    public async Task AddToCartAsync(int cartId, int productId)
    {
        try
        {
            var cart = await GetCartAsyncById(cartId).ConfigureAwait(false);
            if (cart == null)
            {
                logger.LogWarning("Cart with ID '{CartId}' not found.", cartId);
                return;
            }

            var existingProduct = cart.Products.FirstOrDefault(product => product.ProductId == productId);
            if (existingProduct == null)
            {
                cart.Products.Add(new() { ProductId = productId, Quantity = 1 });
            }
            else
            {
                existingProduct.Quantity++;
            }

            var cartData = new
            {
                cartId,
                date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                products = cart.Products
            };

            var content = new StringContent(JsonSerializer.Serialize(cartData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://fakestoreapi.com/carts", content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to add product '{ProductId}' to cart '{CartId}'. HTTP Status Code: {StatusCode}", productId, cartId, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding product '{ProductId}' to cart '{CartId}'.", productId, cartId);
            throw;
        }
    }

    /// <summary>
    /// Removes one instance of a product to the cart.
    /// </summary>
    /// <param name="cartId">Cart id</param>
    /// <param name="productId">Product id</param>
    public async Task RemoveFromCartAsync(int cartId, int productId)
    {
        try
        {
            var cart = await GetCartAsyncById(cartId).ConfigureAwait(false);
            if (cart == null)
            {
                logger.LogWarning("Cart with ID '{CartId}' not found.", cartId);
                return;
            }

            var existingProduct = cart.Products.FirstOrDefault(product => product.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity--;
                if (existingProduct.Quantity <= 0)
                {
                    cart.Products.Remove(existingProduct);
                }

                var cartData = new
                {
                    cartId,
                    date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    products = cart.Products
                };

                var content = new StringContent(JsonSerializer.Serialize(cartData), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://fakestoreapi.com/carts", content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Failed to update cart '{CartId}' after removing product '{ProductId}'. HTTP Status Code: {StatusCode}", cartId, productId, response.StatusCode);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while removing product '{ProductId}' from cart '{CartId}'.", productId, cartId);
            throw;
        }
    }

    /// <summary>
    /// Gets product by id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Product</returns>
    public async Task<Product> GetProductById(int id)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://fakestoreapi.com/products/{id}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch product with ID '{ProductId}'. HTTP Status Code: {StatusCode}", id, response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching product with ID '{ProductId}'.", id);
            throw;
        }
    }
}

