using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingApi.Data.Entities
{
    public class Shipping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}