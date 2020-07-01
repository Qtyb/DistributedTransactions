using MediatR;
using System;

namespace OrderApi.Domain.Events.Product
{
    public class ProductRejected : IRequest<Unit>
    {
        public Guid Guid { get; set; }
    }
}