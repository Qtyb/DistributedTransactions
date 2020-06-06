using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data.Context;
using ProductApi.Data.Entities;
using ProductApi.Domain.Dtos;
using ProductApi.Domain.Dtos.Request;
using ProductApi.Domain.Events.Product;
using Qtyb.Common.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IMapper _mapper;

        public ProductsController(
            ProductContext context,
            IEventBusPublisher eventBusPublisher,
            IMapper mapper)
        {
            _context = context;
            _eventBusPublisher = eventBusPublisher;
            _mapper = mapper;
        }

        [HttpGet("sendObj")]
        public ActionResult SendObj()
        {
            var @event = new ProductCreated { Guid = Guid.NewGuid(), Id = 5, Name = "Twarożek" };
            _eventBusPublisher.Publish(@event, "ProductCreated");
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductResponseDto>>(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();


            return _mapper.Map<ProductResponseDto>(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductRequestDto productRequestDto)
        {
            if (id != productRequestDto.Id)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productRequestDto);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProductExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> PostProduct(ProductRequestDto productRequestDto)
        {
            var product = _mapper.Map<Product>(productRequestDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductResponseDto>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();


            return _mapper.Map<ProductResponseDto>(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}