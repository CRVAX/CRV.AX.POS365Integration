using System;

namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class ProductUpdateDto : BaseRequestDto
    {
        public ProductUpdateDto(BaseParams _base_params) : base(_base_params)
        {
            Product = new ProductInfomationUpdateDto();
        }

        public ProductInfomationUpdateDto Product { get; set; }
    }

    public class ProductInfomationUpdateDto
    {
        public long? Id { get; set; }

        public double Price { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }
    }
}
