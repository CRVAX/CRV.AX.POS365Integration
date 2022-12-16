using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class ProductGetSuccessDto
    {
        public int __Count { get; set; }

        public List<ProductGetResultDto> Results { get; set; }
    }

    public class ProductGetResultDto
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public double Price { get; set; }
    }
}
