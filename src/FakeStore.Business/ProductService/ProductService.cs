using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;

namespace FakeStore.Business.ProductService;

public class ProductService(IFakeStoreApiClient productApiClient) : IProductService
{
	public async Task<List<string>> GetCategoriesAsync()
	{
		return await productApiClient.GetCategoriesAsync();
	}

	public async Task<(List<Product>, int)> GetProductsByCategoryAsync(string category, int page, int pageSize)
	{
		var products = await productApiClient.GetProductsByCategoryAsync(category);

		int totalProducts = products.Count;
		var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

		return (paginatedProducts, totalProducts);
	}

}
