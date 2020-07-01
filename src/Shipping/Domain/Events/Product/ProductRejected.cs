using MediatR;
using System;

namespace ShippingApi.Domain.Events.Product
{
    public class ProductRejected : IRequest<Unit>
    {
        public Guid Guid { get; set; }
    }
}