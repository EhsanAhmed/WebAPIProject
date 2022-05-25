using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository;
using WebAPIProject.DTO;
using WebAPIProject.Interfaces;
using WebAPIProject.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly APIDbContext context;
        private readonly IUriService uriService;
        private new readonly UserManager<ApplicationUser> User;
        private readonly List<string> allowedExtentios = new List<string> { ".jpg", ".png" };
        private long _maxAllwedImageSize = 2097152;

        public ProductController( IProductRepository _productRepository, APIDbContext context, IUriService uriService, UserManager<ApplicationUser> user)
        {
            this.productRepository = _productRepository;
            this.context = context;
            this.uriService = uriService;
            this.User = user;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await context.Products
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await context.Products.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Product>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
    
       // List<Product> products = productRepository.GetAll();
         //   return Ok(products);
        }

        [HttpGet("{id:int}", Name = "getRoute")]
        public IActionResult getByID(int id)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return BadRequest("Empty ");
            }
            return Ok(new Response<Product>(product));
        }
      

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO pro)
        {
            if(!allowedExtentios.Contains(Path.GetExtension(pro.Img.FileName).ToLower()))
                return BadRequest("Only .png and jpg Images are allowed");
            if(pro.Img.Length >_maxAllwedImageSize)
                return BadRequest("The Size is too Large, Allwed Size is 2MB");

              
            var datastream = new MemoryStream();
           await pro.Img.CopyToAsync(datastream);

            Product newPro = new Product()
            {
                Name = pro.Name,
                Price = pro.Price,
                Quantity = pro.Quantity,
                Img = datastream.ToArray()
            };
                if (ModelState.IsValid)
                {
                try
                {
                    productRepository.Insert(newPro);
                  
                    string url = Url.Link("getRoute", new { id = newPro.ID });
                    return Created(url, pro);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id,[FromForm] ProductDTO pro)
        {
            if (!allowedExtentios.Contains(Path.GetExtension(pro.Img.FileName).ToLower()))
                return BadRequest("Only .png and jpg Images are allowed");
            if (pro.Img.Length > _maxAllwedImageSize)
                return BadRequest("The Size is too Large, Allwed Size is 2MB");

            var datastream = new MemoryStream();
            await pro.Img.CopyToAsync(datastream);

            Product newPro = new Product()
            {
                Name = pro.Name,
                Price = pro.Price,
                Quantity = pro.Quantity,
                Img = datastream.ToArray()
            };
            if (ModelState.IsValid)
            {
                try
                {
                    productRepository.Update(id, newPro);

                    return StatusCode(204, "Data Saved");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }
        [HttpDelete]
        public  IActionResult Delete(int id)
        {
            productRepository.Delete(id);
            return Ok();    
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationFilter filter,string name)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
           //   return Ok(pagedReponse);

            int price;
            var pagedData=new List<Product>();
          //  List<Product> products;
            bool success = Int32.TryParse(name, out price);
            if (success)
            {
                 pagedData = await context.Products.Where(x => x.Price == price)
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();

               // products = productRepository.GetByPropertyPrice(price);
            }
            else
            {
                 pagedData = await context.Products.Where(x => x.Name == name)
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();

                //products = productRepository.GetByPropertyName(name);

            }
            var totalRecords = await context.Products.CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Product>(pagedData, validFilter, totalRecords, uriService, route);

            return Ok(pagedReponse);
        }
     
    }
}
