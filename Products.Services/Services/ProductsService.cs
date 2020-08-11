using Microsoft.Extensions.Logging;
using Products.Services.Interfaces;
using Products.Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Products.Services.Services
{
    public class ProductsService : IProductsService
    {
        private readonly string _url = "http://www.mocky.io/v2/5e307edf3200005d00858b49";
        protected readonly ILogger _logger;

        public ProductsService(ILogger<ProductsService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                HttpClient http = new HttpClient();
                var data = http.GetAsync(_url).Result?.Content?.ReadAsStringAsync()?.Result;

                _logger.LogInformation($"Response details: {Newtonsoft.Json.JsonConvert.SerializeObject(data)}");

                var mockResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MockResponse>(data);

                return mockResponse.Products;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return null;
        }
    }
}
