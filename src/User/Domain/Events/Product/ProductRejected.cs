using MediatR;
using System;

namespace UserApi.Domain.Events.Product
{
    public class ProductRejected : IRequest<Unit>
    {
        public Guid Guid { get; set; }
    }
}