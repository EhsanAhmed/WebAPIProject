using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPIProject.DTO;

namespace WebAPI.Repository
{
    public interface IProductRepository
    {
       List<Product> GetAll();
        Product GetById(int id);
        void Insert(Product entity);
        void Update(int id, Product entity);
        void Delete(int id);
        List<Product> GetByPropertyName(string name);
        List<Product> GetByPropertyPrice(int price);

    }
}