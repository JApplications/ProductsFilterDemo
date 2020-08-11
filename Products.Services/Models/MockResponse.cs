using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Services.Models
{
    public class MockResponse
    {
        public List<Product> Products{ get; set; }
        public ApiKeys ApiKeys { get; set; }
    }
}
