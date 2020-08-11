using Products.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Services.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetAll();
    }
}
