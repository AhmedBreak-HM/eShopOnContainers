﻿using Matgr.UI.Models;
using Matgr.UI.Models.Dtos;

namespace Matgr.UI.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpClientFactory httpClient) : base(httpClient)
        {
        }

        public async Task<T> CreateProduct<T>(ProductDto productDto, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = SD.ProductsAPIUrl + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProduct<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductsAPIUrl + $"/api/products/{id}",
                AccessToken = token
            });
        }

        public async Task<T> GetAllProducts<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductsAPIUrl + $"/api/products",
                AccessToken = token
            });
        }

        public async Task<T> GetProduct<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductsAPIUrl + $"/api/products/{id}",
                AccessToken = token
            });
        }

        public async Task<T> UpdateProduct<T>(ProductDto productDto, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = SD.ProductsAPIUrl + "/api/products",
                AccessToken = token
            });
        }
    }
}
