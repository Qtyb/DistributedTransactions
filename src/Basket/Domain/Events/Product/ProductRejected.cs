using MediatR;
using System;

namespace BasketApi.Domain.Events.Product
{
    public class ProductRejected : IRequest<Unit>
    {
        public Guid Guid { get; set; }
    }
}