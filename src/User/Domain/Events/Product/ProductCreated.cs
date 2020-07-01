﻿using MediatR;
using System;

namespace UserApi.Domain.Events.Product
{
    public class ProductCreated : IRequest<Unit>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}