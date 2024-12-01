﻿using FakeStore.ApiClient.Models;

namespace FakeStore.Business.ProductService;

public interface IProductService
{
	public Task<List<string>> GetCategoriesAsync();

	public Task<(List<Product>, int)> GetProductsByCategoryAsync(string category, int page, int pageSize);
}