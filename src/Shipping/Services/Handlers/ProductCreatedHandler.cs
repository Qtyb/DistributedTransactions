using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qtyb.Common.EventBus.Interfaces;
using ShippingApi.Data.Context;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Events.Product;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApi.Services.Handlers
{
    public class ProductCreatedHandler : IRequestHandler<ProductCreated>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCreatedHandler> _logger;

        public ProductCreatedHandler(
            IServiceScopeFactory serviceScopeFactory,
            IEventBusPublisher eventBusPublisher,
            IMapper mapper,
            ILogger<ProductCreatedHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(ProductCreated productCreated, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"---- Received {nameof(ProductCreated)} message: Product.Guid = [{productCreated.Guid}] ----");
            try
            {
                if (productCreated.Name.EndsWith('1'))
                    throw new System.Exception("PRODUCT ENDS WITH '1' !!! THAT IS NOT VALID");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ShippingContext>();

                    var product = _mapper.Map<Product>(productCreated);
                    context.Products.Add(product);

                    await context.SaveChangesAsync();
                }
                _logger.LogInformation($"---- Saved {nameof(ProductCreated)} message: Product.Guid = [{productCreated.Guid}] ----");
            }
            catch (System.Exception ex)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ShippingContext>();
                    var productRejected = new ProductRejected { Guid = productCreated.Guid };

                    var data = JsonSerializer.Serialize(productRejected);
                    var productEvent = new OutboxEvent(data, "ProductRejected");
                    context.OutboxEvents.Add(productEvent);
                }
            }
            return await Task.FromResult(Unit.Value);
        }
    }
}