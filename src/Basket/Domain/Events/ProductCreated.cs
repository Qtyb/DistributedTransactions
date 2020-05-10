using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Domain.Events
{
    public class ProductCreated
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
