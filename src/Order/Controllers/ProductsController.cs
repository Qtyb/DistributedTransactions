using AutoMapper;
using OrderApi.Data.Context;
using OrderApi.Data.Entities;
using OrderApi.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController
    {
        private readonly OrderContext _context;
        private readonly IMapper _mapper;

        public ProductsController(
            OrderContext context,
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