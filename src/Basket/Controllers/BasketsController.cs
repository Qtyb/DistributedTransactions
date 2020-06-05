using AutoMapper;
using BasketApi.Data.Context;
using BasketApi.Data.Entites;
using BasketApi.Domain.Dtos.Request;
using BasketApi.Domain.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qtyb.Common.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly BasketContext _context;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IMapper _mapper;

        public BasketsController(
            BasketContext context,
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
            var basket = new Basket { Guid = Guid.NewGuid(), Id = 5, Name = "Send Test" };
            _eventBusPublisher.Publish(basket, "ProductCreated");
            return Ok();
        }

        [HttpGet("sendTxt")]
        public ActionResult SendTxt()
        {
            _eventBusPublisher.Publish("message send", "ProductCreated");
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketResponseDto>>> GetBaskets()
        {
            var baskets = await _context.Baskets.ToListAsync();
            return _mapper.Map<List<BasketResponseDto>>(baskets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasketResponseDto>> GetBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return _mapper.Map<BasketResponseDto>(basket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasket(int id, BasketRequestDto basketRequestDto)
        {
            if (id != basketRequestDto.Id)
            {
                return BadRequest();
            }

            var basket = _mapper.Map<Basket>(basketRequestDto);
            _context.Entry(basket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketExists(id))
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
        public async Task<ActionResult<BasketResponseDto>> PostBasket(BasketRequestDto basketRequestDto)
        {
            var basket = _mapper.Map<Basket>(basketRequestDto);
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BasketResponseDto>> DeleteBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();

            return _mapper.Map<BasketResponseDto>(basket);
        }

        private bool BasketExists(int id)
        {
            return _context.Baskets.Any(e => e.Id == id);
        }
    }
}