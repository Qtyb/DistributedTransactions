using AutoMapper;
using BasketApi.Data.Context;
using BasketApi.Data.Entites;
using BasketApi.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController
    {
        private readonly BasketContext _context;
        private readonly IMapper _mapper;

        public ProductsController(
            BasketContext context,
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