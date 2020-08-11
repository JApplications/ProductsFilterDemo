using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Web.Models
{
    public class ProductsViewModel
    {
        public List<ProductItemViewModel> Products { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string[] CommonWords { get; set; }
        public string[] AllSizes { get; set; }
    }

    public class ProductItemViewModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public List<string> Sizes { get; set; }
        public string Description { get; set; }
    }
}
