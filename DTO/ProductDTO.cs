using Microsoft.AspNetCore.Http;
namespace WebAPIProject.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public IFormFile Img { get; set; }
    }
}
