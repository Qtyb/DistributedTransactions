using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserApi.Data.Context;
using UserApi.Data.Entities;
using UserApi.Domain.Dtos.Request;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController
    {
        private readonly UserContext _context;
        private readonly IMapper _mapper;

        public ProductsController(
            UserContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductRequestDto productRequestDto)
        {
            var product = _mapper.Map<Product>(productRequestDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new OkObjectResult(product.Id);
        }
    }
}