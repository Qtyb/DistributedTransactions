using MediatR;
using System;

namespace ProductApi.Domain.Events.Product
{
    public class ProductRejected : IRequest<Unit>
    {
        public Guid Guid { get; set; }
    }
}