using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Services.Interfaces
{
    public interface IEventHandler<T> where T: class
    {
        void Handle(T @event);
    }
}
