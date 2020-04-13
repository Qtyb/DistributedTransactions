using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Domain.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}