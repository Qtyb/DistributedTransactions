using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApi.Domain.Dtos.Request
{
    public class ProductRequestDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}