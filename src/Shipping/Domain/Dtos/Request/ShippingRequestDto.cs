using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApi.Domain.Dtos.Request
{
    public class ShippingRequestDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
