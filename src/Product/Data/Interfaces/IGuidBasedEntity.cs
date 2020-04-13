using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Data.Interfaces
{
    public interface IGuidBasedEntity
    {
        Guid Guid { get; set; }
    }
}