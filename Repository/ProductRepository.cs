using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPIProject.DTO;

namespace WebAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly APIDbContext context;
        public ProductRepository(APIDbContext aPIDbContext)
        {
            context = aPIDbContext;
        }

      
        public Product GetById(int id)
        {
            Product product = context.Products.FirstOrDefault(x => x.ID == id);

            return product;

        }
        public void Insert(Product entity)
        {
            Product product = new Product();
            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Quantity = entity.Quantity;
            product.Img = entity.Img;

            context.Products.Add(product);
            context.SaveChanges();
        }

        public void Update(int id, Product entity)
        {
            Product product = context.Products.FirstOrDefault(p => p.ID == id);

            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Quantity = entity.Quantity;
            product.Img = entity.Img;
            context.SaveChanges();

        }
        public void Delete(int id)
        {
            Product product = context.Products.FirstOrDefault(p => p.ID == id);
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            List<Product> products = context.Products.ToList();
            return products;
        }
        public List<Product> GetByPropertyName(string name)
        {
            List<Product> products = context.Products.Where(p => p.Name == name).ToList();  
            return products;
        }
        public List<Product> GetByPropertyPrice(int price)
        {
            List<Product> products = context.Products.Where(p => p.Price == price).ToList();
            return products;
        }
    }
}
