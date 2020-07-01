using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderApi.Data.Context;
using OrderApi.Data.Entities;
using OrderApi.Domain.Events.Product;
using System.Threading;
using System.Threading.Tasks;

namespace OrderApi.Services.Handlers
{
    public class ProductCreatedHandler : IRequestHandler<ProductCreated>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCreatedHandler> _logger;

        public ProductCreatedHandler(
            IServiceScopeFactory serviceScopeFactory,
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
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OrderContext>();

                var product = _mapper.Map<Product>(productCreated);
                context.Products.Add(product);

                await context.SaveChangesAsync();
            }
            _logger.LogInformation($"---- Saved {nameof(ProductCreated)} message: Product.Guid = [{productCreated.Guid}] ----");

            return await Task.FromResult(Unit.Value);
        }
    }
}