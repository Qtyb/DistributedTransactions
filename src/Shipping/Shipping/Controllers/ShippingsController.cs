using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Data.Context;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Dtos.Request;
using ShippingApi.Domain.Dtos.Response;

namespace ShippingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly ShippingContext _context;
        private readonly IMapper _mapper;

        public ShippingsController(
            ShippingContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingResponseDto>>> GetShippings()
        {
            var shippings =  await _context.Shippings.ToListAsync();
            return _mapper.Map<List<ShippingResponseDto>>(shippings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingResponseDto>> GetShipping(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);

            if (shipping == null)
            {
                return NotFound();
            }

            return _mapper.Map<ShippingResponseDto>(shipping);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipping(int id, ShippingRequestDto shippingRequestDto)
        {
            if (id != shippingRequestDto.Id)
            {
                return BadRequest();
            }

            var shipping = _mapper.Map<Shipping>(shippingRequestDto);
            _context.Entry(shipping).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ShippingResponseDto>> PostShipping(ShippingRequestDto shippingRequestDto)
        {
            var shipping = _mapper.Map<Shipping>(shippingRequestDto);
            _context.Shippings.Add(shipping);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipping", new { id = shipping.Id }, shipping);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ShippingResponseDto>> DeleteShipping(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping == null)
            {
                return NotFound();
            }

            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShippingResponseDto>(shipping);
        }

        private bool ShippingExists(int id)
        {
            return _context.Shippings.Any(e => e.Id == id);
        }
    }
}
