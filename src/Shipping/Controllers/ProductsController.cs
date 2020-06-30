using AutoMapper;
using ShippingApi.Data.Context;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ShippingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController
    {
        private readonly ShippingContext _context;
        private readonly IMapper _mapper;

        public ProductsController(
            ShippingContext context,
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