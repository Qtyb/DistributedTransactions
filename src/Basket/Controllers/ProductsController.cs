using AutoMapper;
using BasketApi.Data.Context;
using BasketApi.Data.Entites;
using BasketApi.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController
    {
        private readonly BasketContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;

        public ProductsController(
            BasketContext context,
            ILogger<ProductsController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductRequestDto productRequestDto)
        {
            _logger.LogInformation($"Hit PostProduct with guid {productRequestDto.Guid}");
            var product = _mapper.Map<Product>(productRequestDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"SavedProduct with guid {productRequestDto.Guid}");
            return new OkObjectResult(product.Id);
        }
    }
}