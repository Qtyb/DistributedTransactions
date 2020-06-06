using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Context;
using OrderApi.Data.Entities;
using OrderApi.Domain.Dtos.Request;
using OrderApi.Domain.Dtos.Response;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly IMapper _mapper;

        public OrdersController(
            OrderContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
        {
            var oders =  await _context.Orders.ToListAsync();
            return _mapper.Map<List<OrderResponseDto>>(oders);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderResponseDto>(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderRequestDto orderRequestDto)
        {
            if (id != orderRequestDto.Id)
            {
                return BadRequest();
            }

            var order = _mapper.Map<Order>(orderRequestDto);
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!OrderExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> PostOrder(OrderRequestDto orderRequestDto)
        {
            var order = _mapper.Map<Order>(orderRequestDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderResponseDto>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderResponseDto>(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
