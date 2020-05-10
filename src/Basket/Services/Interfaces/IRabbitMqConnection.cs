using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Services.Interfaces
{
    public interface IRabbitMqConnection
    {
        IModel Connect();
    }
}
