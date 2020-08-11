using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Interfaces;
using Products.Services.Models;
using Products.Web.Models;

namespace Products.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("filter")]
        public ProductsViewModel Filter([FromQuery(Name = "maxprice")] decimal maxPrice, [FromQuery(Name = "size")] string size, [FromQuery(Name = "highlight")] string highlight)
        {
            ProductsViewModel model = null;

            var data = _productsService.GetAll();

            if (data != null && data.Count() > 0)
            {
                model = SetProductsViewModel(data);

                if (maxPrice > 0)
                {
                    data = data.Where(x => x.Price <= maxPrice).ToList();
                }

                if (!string.IsNullOrEmpty(size))
                {
                    List<string> sizeArr = size.Split(",").ToList();

                    data = data.Where(x => sizeArr.Any(y => x.Sizes.Contains(y)))?.ToList();
                }

                string[] highlights = highlight?.Split(",");

                model.Products = BindToViewModel(data, highlights);
            }

            return model;
        }

        private ProductsViewModel SetProductsViewModel(IEnumerable<Product> data)
        {
            var priceOrdered = data.OrderByDescending(x => x.Price);

            ProductsViewModel model = new ProductsViewModel
            {
                CommonWords = CalculateMostCommonWords(data).ToArray(),
                MaxPrice = priceOrdered.FirstOrDefault().Price,
                MinPrice = priceOrdered.LastOrDefault().Price,
                AllSizes = priceOrdered.SelectMany(x => x.Sizes).Distinct().ToArray()
            };

            return model;
        }

        private List<string> CalculateMostCommonWords(IEnumerable<Product> products)
        {
            string descriptions = string.Join(" ", products.Select(x => x.Description));

            var orderedWords = descriptions
                      .Split(' ')
                      .GroupBy(x => x)
                      .Select(x => new
                      {
                          KeyField = x.Key,
                          Count = x.Count()
                      })
                      .OrderByDescending(x => x.Count)
                      .Take(15)
                      .Skip(5)
                      .Select(y => y.KeyField)
                      .ToList();

            return orderedWords;
        }

        private List<ProductItemViewModel> BindToViewModel(IEnumerable<Product> products, string[] highlights)
        {
            List<ProductItemViewModel> list = null;

            if (products != null && products.Count() > 0)
            {
                list = new List<ProductItemViewModel>();

                foreach (var product in products)
                {
                    list.Add(new ProductItemViewModel
                    {
                        Description = SetHighlightsInDescriptions(product.Description, highlights),
                        Price = product.Price,
                        Sizes = product.Sizes,
                        Title = product.Title
                    });
                }
            }

            return list;
        }

        private string SetHighlightsInDescriptions(string description, string[] highlights)
        {
            StringBuilder sb = new StringBuilder(description);

            if (highlights != null && highlights.Length > 0)
            {
                foreach (var highlight in highlights)
                {
                    sb.Replace(highlight, $"<em>{highlight}</em>");
                }
            }

            return sb.ToString().ToLower();
        }
    }
}