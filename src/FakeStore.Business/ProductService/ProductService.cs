using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;

namespace FakeStore.Business.ProductService;

public class ProductService(IFakeStoreApiClient apiClient, ILogger<ProductService> logger) : IProductService
{
    /// <summary>
    /// Gets all categories
    /// </summary>
    /// <returns>List of all categories</returns>
    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            return await apiClient.GetCategoriesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving the categories");
            throw;
        }
    }

    /// <summary>
    /// Gets product by id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Product</returns>
    public async Task<Product> GetProductAsync(int id)
    {
        try
        {
            return await apiClient.GetProductById(id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while downloading product ID {id}");
            throw;
        }
    }

    /// <summary>
    /// Get all products in given category
    /// </summary>
    /// <param name="category">Name of the category</param>
    /// <returns>List of products in given category</returns>
    public async Task<(List<Product>, int)> GetProductsByCategoryAsync(string category, int page, int pageSize)
    {
        try
        {
            var products = await apiClient.GetProductsByCategoryAsync(category).ConfigureAwait(false);
            int totalProducts = products.Count;
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (paginatedProducts, totalProducts);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while retrieving products for the category {category}");
            throw;
        }
    }

}
