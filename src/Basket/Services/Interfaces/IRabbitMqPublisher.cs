﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Services.Interfaces
{
    public interface IRabbitMqPublisher
    {
        void Publish<T>(T objectToSend, string routingKey);
    }
}
