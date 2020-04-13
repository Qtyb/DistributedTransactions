using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Domain.Dtos.Request
{
    public class ProductRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}